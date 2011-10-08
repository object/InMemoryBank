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

        private SmsGateway smsGateway;
        private CommandProcessor commandProcessor;

        public readonly ApplicationQueue<SmsMessage> ReceivedMessages = new ApplicationQueue<SmsMessage>();
        public readonly ApplicationQueue<SmsMessage> SentMessages = new ApplicationQueue<SmsMessage>();
        public readonly ApplicationQueue<PaymentCommand> CommandQueue = new ApplicationQueue<PaymentCommand>();
        public readonly ApplicationQueue<PaymentNotification> NotificationQueue = new ApplicationQueue<PaymentNotification>();
        public readonly Repository Repository = new Repository();

        static Application()
        {
        }

        Application()
        {
            this.smsGateway = new SmsGateway(this.ReceivedMessages, this.SentMessages, this.CommandQueue, this.NotificationQueue); 
            this.commandProcessor = new CommandProcessor(this.CommandQueue, this.NotificationQueue, this.Repository);
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
