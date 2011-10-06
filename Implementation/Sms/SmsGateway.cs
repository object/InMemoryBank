﻿using System;
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
            this.notificationQueue.SubscribeWithHandler(Notify);
        }

        public void ReceiveMessage(SmsMessage message)
        {
            var parser = new SmsParser();
            var command = parser.Parse(message);
            ThreadPool.QueueUserWorkItem((x) => this.commandQueue.Enqueue(command));
        }

        public void SendMessage(SmsMessage message)
        {
            ThreadPool.QueueUserWorkItem((x) => this.outgoingMessageQueue.Enqueue(message));
        }

        public void Notify(PaymentNotification notification)
        {
            var message = new SmsMessage()
            {
                Message = "In order to use InMemory Bank you need to register. Command is cancelled.",
                PhoneNumber = notification.PhoneNumber
            };
            SendMessage(message);
        }
    }
}
