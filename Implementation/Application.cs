using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation.Bank;
using Implementation.Sms;

namespace Implementation
{
    public sealed class Application
    {
        static readonly Application instance = new Application();
        private MessageProcessor messageProcessor;

        public readonly ApplicationQueue<SmsMessage> ReceivedMessages = new ApplicationQueue<SmsMessage>();
        public readonly ApplicationQueue<SmsMessage> SentMessages = new ApplicationQueue<SmsMessage>();
        public readonly Repository Repository = new Repository();

        static Application()
        {
        }

        Application()
        {
            this.messageProcessor = new MessageProcessor(this.ReceivedMessages, this.SentMessages, this.Repository);
        }

        public static Application Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
