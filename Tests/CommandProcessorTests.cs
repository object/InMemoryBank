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
    public class CommandProcessorTests
    {
        [Test]
        public void ProcessCommand_UnregisteredUser_PayerNotRegisteredException()
        {
            var commandProcessor = new CommandProcessor(Application.Instance.CommandQueue,
                                                        Application.Instance.NotificationQueue,
                                                        Application.Instance.Repository);

            var command = new PaymentCommand {PayerNumber = "12345678", CollectorNumber = "98765432"};

            Assert.Throws<PayerNotRegisteredException>(() => commandProcessor.ProcessCommand(command));
        }

        [Test]
        public void CreateNotification_PayerNotRegisteredException()
        {
            var commandProcessor = new CommandProcessor(Application.Instance.CommandQueue,
                                                        Application.Instance.NotificationQueue,
                                                        Application.Instance.Repository);

            var command = new PaymentCommand {PayerNumber = "12345678", CollectorNumber = "98765432"};

            var notification = commandProcessor.CreateNotification(command, new PayerNotRegisteredException());

            Assert.AreEqual(NotificationTopic.PayerNotRegistered, notification.Topic);
        }
    }
}
