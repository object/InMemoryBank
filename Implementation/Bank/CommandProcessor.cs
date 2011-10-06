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

        private void CommandHandler(PaymentCommand command)
        {
            try
            {
                ProcessCommand(command);
            }
            catch (Exception exception)
            {
                ThreadPool.QueueUserWorkItem(
                    (x) => this.notificationQueue.Enqueue(CreateNotification(command, exception)));
            }
        }

        public void ProcessCommand(PaymentCommand command)
        {
            throw new PayerNotRegisteredException();
        }

        public PaymentNotification CreateNotification(PaymentCommand command, Exception exception)
        {
            return new PaymentNotification()
            {
                PhoneNumber = command.PayerNumber,
                Topic = NotificationTopic.PayerNotRegistered,
                Command = command,
            };
        }
    }
}
