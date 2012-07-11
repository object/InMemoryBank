using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Implementation.Bank;

namespace Implementation.Sms
{
    public class SmsGateway
    {
        private ApplicationQueue<SmsMessage> incomingMessageQueue;
        private ApplicationQueue<SmsMessage> outgoingMessageQueue;
        private ApplicationQueue<PaymentCommand> commandQueue;
        private ApplicationQueue<PaymentNotification> notificationQueue;

        public SmsGateway(
            ApplicationQueue<SmsMessage> incomingMessageQueue,
            ApplicationQueue<SmsMessage> outgoingMessageQueue,
            ApplicationQueue<PaymentCommand> commandQueue,
            ApplicationQueue<PaymentNotification> notificationQueue)
        {
            this.incomingMessageQueue = incomingMessageQueue;
            this.outgoingMessageQueue = outgoingMessageQueue;
            this.commandQueue = commandQueue;
            this.notificationQueue = notificationQueue;

            this.incomingMessageQueue.SubscribeWithHandler(ReceiveMessage);
            this.outgoingMessageQueue.SubscribeWithHandler(MessageDispatcher.DispatchMessage);
            this.notificationQueue.SubscribeWithHandler(Notify);
        }

        public bool ReceiveMessage(SmsMessage message)
        {
            var parser = new SmsParser();
            var command = parser.Parse(message);
            ThreadPool.QueueUserWorkItem((x) => this.commandQueue.Enqueue(command));
            return true;
        }

        public void SendMessage(SmsMessage message)
        {
            ThreadPool.QueueUserWorkItem((x) => this.outgoingMessageQueue.Enqueue(message));
        }

        public bool Notify(PaymentNotification notification)
        {
            var formatter = GetNotificationFormatter();
            var text = formatter[notification.Topic](notification);
            var message = new SmsMessage() { Message = text, PhoneNumber = notification.PhoneNumber };
            SendMessage(message);
            return true;
        }

        private static Dictionary<NotificationTopic, Func<PaymentNotification, string>> GetNotificationFormatter()
        {
            return new Dictionary<NotificationTopic, Func<PaymentNotification, string>>
                       {
                           {
                               NotificationTopic.PaymentSent, x => string.Format(
                                    "You paid {0} to {1}. Your new balance is {2}. Thank you for using InMemory Bank.",
                                    x.Command.Amount, x.Command.Payment.Collector.PhoneNumber,
                                    x.Command.Payment.Payer.Balance)
                               },
                           {
                               NotificationTopic.PaymentReceived, x => string.Format(
                                    "You received {0} from {1}. Your new balance is {2}. Thank you for using InMemory Bank.",
                                    x.Command.Amount, x.Command.Payment.Payer.PhoneNumber, x.Command.Payment.Collector.Balance)
                               },
                           {
                               NotificationTopic.PayerNotRegistered, x => 
                                    "In order to use InMemory Bank you need to register. Command is cancelled."
                               },
                           {
                               NotificationTopic.CollectorNotRegistered, x => string.Format(
                                   "You can not send money to unregistered user ({0}). Command is cancelled.", 
                                    x.Command.CollectorNumber)
                               },
                           {
                               NotificationTopic.InsufficientFunds, x => string.Format(
                                   "Not enough funds to pay {0} to {1}. Your current balance is {2}. Command is cancelled.", 
                                    x.Command.Amount, x.Command.CollectorNumber, x.Command.Payment.Payer.Balance)
                               },
                       };

        }
    }
}
