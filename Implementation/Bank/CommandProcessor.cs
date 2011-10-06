﻿using System;
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
                    (x) =>
                        {
                            var notification = CreateNotification(command, exception);
                            if (notification != null)
                                this.notificationQueue.Enqueue(notification);
                        });
            }
        }

        public void ProcessCommand(PaymentCommand command)
        {
            var payer = this.repository.FindUser(command.PayerNumber);
            if (payer == null)
                throw new PayerNotRegisteredException();

            var collector = this.repository.FindUser(command.CollectorNumber);
            if (collector == null)
                throw new CollectorNotRegisteredException();

            throw new NotImplementedException();
        }

        public PaymentNotification CreateNotification(PaymentCommand command, Exception exception)
        {
            var topics = new Dictionary<Type, NotificationTopic>()
                             {
                                 {typeof (PayerNotRegisteredException), NotificationTopic.PayerNotRegistered},
                                 {typeof (CollectorNotRegisteredException), NotificationTopic.CollectorNotRegistered},
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
    }
}
