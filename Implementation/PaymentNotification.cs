using System;

namespace Implementation.Bank
{
    public class PaymentNotification
    {
        public string PhoneNumber { get; set; }
        public NotificationTopic Topic { get; set; }
        public PaymentCommand Command { get; set; }
    }
}
