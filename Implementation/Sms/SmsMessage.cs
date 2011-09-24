using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Sms
{
    public class SmsMessage
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.PhoneNumber, this.Message);
        }

        public override bool Equals(object obj)
        {
            var message = obj as SmsMessage;
            if (message != null)
            {
                return this.Message == message.Message && this.PhoneNumber == message.PhoneNumber;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.PhoneNumber.GetHashCode() ^ this.Message.GetHashCode();
        }
    }
}
