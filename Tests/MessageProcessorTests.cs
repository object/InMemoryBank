using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation;
using Implementation.Bank;
using Implementation.Sms;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MessageProcessorTests
    {
        [Test]
        public void ProcessMessage_UnregisteredUser_PayerNotRegisteredException()
        {
            var messageProcessor = new MessageProcessor(Application.Instance.ReceivedMessages,
                                                        Application.Instance.SentMessages,
                                                        Application.Instance.Repository);

            var message = new SmsMessage { PhoneNumber = "12345678", Message = "PAY 10 95473893" };

            Assert.Throws<PayerNotRegisteredException>(() => messageProcessor.ProcessMessage(message));
        }
    }
}
