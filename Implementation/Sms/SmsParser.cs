using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation.Bank;

namespace Implementation.Sms
{
    internal class SmsParser
    {
        public PaymentCommand Parse(SmsMessage message)
        {
            string[] items = message.Message.Split(' ');
            var command = new PaymentCommand()
            {
                PayerNumber = message.PhoneNumber,
                CollectorNumber = items[2],
                Amount = decimal.Parse(items[1]),
                PaymentType = PaymentType.Private
            };
            return command;
        }
    }
}