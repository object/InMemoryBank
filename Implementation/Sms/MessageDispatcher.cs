using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Sms
{
    public class MessageDispatcher
    {
        public static bool DispatchMessage(SmsMessage message)
        {
            throw new InvalidOperationException("Message dispatcher is not available in test environment");
        }
    }
}
