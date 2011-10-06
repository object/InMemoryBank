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
        public void ProcessMessage_UnregisteredUser_PayerNotRegisteredException()
        {
            var commandProcessor = new CommandProcessor(Application.Instance.CommandQueue,
                                                        Application.Instance.Repository);

            var command = new PaymentCommand {PayerNumber = "12345678", CollectorNumber = "98765432"};

            Assert.Throws<PayerNotRegisteredException>(() => commandProcessor.ProcessCommand(command));
        }
    }
}
