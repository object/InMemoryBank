using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Implementation.Bank
{
    public class CommandProcessor
    {
        private ApplicationQueue<PaymentCommand> commandQueue;
        private ApplicationQueue<PaymentNotification> notificationQueue;
        private Repository repository;

        public CommandProcessor(
            ApplicationQueue<PaymentCommand> commandQueue,
            ApplicationQueue<PaymentNotification> notificationQueue,
            Repository repository)
        {
            this.commandQueue = commandQueue;
            this.notificationQueue = notificationQueue;
            this.repository = repository;

            this.commandQueue.SubscribeWithHandler(CommandHandler);
        }

        private bool CommandHandler(PaymentCommand command)
        {
            try
            {
                ProcessCommand(command);
                CreateNotifications(command).ForEach(SendNotification);
            }
            catch (Exception exception)
            {
                SendNotification(CreateNotification(command, exception));
            }
            return true;
        }

        public void ProcessCommand(PaymentCommand command)
        {
            var payer = this.repository.FindUser(command.PayerNumber);
            if (payer == null)
                throw new PayerNotRegisteredException();

            var collector = this.repository.FindUser(command.CollectorNumber);
            if (collector == null)
                throw new CollectorNotRegisteredException();

            command.Payment = new Payment()
            {
                TransferTime = DateTime.Now,
                PaymentType = command.PaymentType,
                Amount = command.Amount,
                Payer = payer,
                Collector = collector,
                PayerFee = this.repository.GetPayerFee(command.PaymentType),
                CollectorFee = this.repository.GetCollectorFee(command.PaymentType)
            };

            lock (repository.Payments)
            {
                RegisterPayment(command.Payment);
            }
        }

        private void RegisterPayment(Payment payment)
        {
            var newPayerBalance = payment.Payer.Balance - payment.Amount - payment.PayerFee;
            if (newPayerBalance < 0)
                throw new InsufficientFundsException();

            payment.Payer.Balance = newPayerBalance;
            payment.Collector.Balance += payment.Amount - payment.CollectorFee;
            repository.Payments.Add(payment);
        }

        public List<PaymentNotification> CreateNotifications(PaymentCommand command)
        {
            var notifications = new List<PaymentNotification>();
            notifications.Add(new PaymentNotification()
                                  {
                                      PhoneNumber = command.PayerNumber,
                                      Topic = NotificationTopic.PaymentSent,
                                      Command = command,
                                  });
            notifications.Add(new PaymentNotification()
            {
                PhoneNumber = command.CollectorNumber,
                Topic = NotificationTopic.PaymentReceived,
                Command = command,
            });
            return notifications;
        }

        public PaymentNotification CreateNotification(PaymentCommand command, Exception exception)
        {
            var topics = new Dictionary<Type, NotificationTopic>()
                             {
                                 {typeof (PayerNotRegisteredException), NotificationTopic.PayerNotRegistered},
                                 {typeof (CollectorNotRegisteredException), NotificationTopic.CollectorNotRegistered},
                                 {typeof (InsufficientFundsException), NotificationTopic.InsufficientFunds},
                             };

            if (topics.ContainsKey(exception.GetType()))
            {
                return new PaymentNotification()
                {
                    PhoneNumber = command.PayerNumber,
                    Topic = topics[exception.GetType()],
                    Command = command,
                };
            }

            return null;
        }

        private void SendNotification(PaymentNotification notification)
        {
            ThreadPool.QueueUserWorkItem(
                (x) =>
                {
                    if (notification != null)
                        this.notificationQueue.Enqueue(notification);
                });
        }
    }
}
