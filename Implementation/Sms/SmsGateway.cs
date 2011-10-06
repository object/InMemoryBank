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

        public SmsGateway(
            ApplicationQueue<SmsMessage> incomingMessageQueue,
            ApplicationQueue<SmsMessage> outgoingMessageQueue,
            ApplicationQueue<PaymentCommand> commandQueue)
        {
            this.incomingMessageQueue = incomingMessageQueue;
            this.outgoingMessageQueue = outgoingMessageQueue;
            this.commandQueue = commandQueue;

            this.incomingMessageQueue.SubscribeWithHandler(ReceiveMessage);
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
    }
}
