using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Implementation.Bank;
using Implementation.Sms;

namespace Implementation
{
    public class MessageProcessor
    {
        private ApplicationQueue<SmsMessage> incomingMessageQueue;
        private ApplicationQueue<SmsMessage> outgoingMessageQueue;
        private Repository repository;

        public MessageProcessor(
            ApplicationQueue<SmsMessage> incomingMessageQueue,
            ApplicationQueue<SmsMessage> outgoingMessageQueue,
            Repository repository)
        {
            this.incomingMessageQueue = incomingMessageQueue;
            this.outgoingMessageQueue = outgoingMessageQueue;
            this.repository = repository;

            this.incomingMessageQueue.SubscribeWithHandler(ProcessMessage);
        }

        public void ProcessMessage(SmsMessage message)
        {
            throw new PayerNotRegisteredException();
        }
    }
}
