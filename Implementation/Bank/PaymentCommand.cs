using System;

namespace Implementation.Bank
{
    public class PaymentCommand
    {
        public string PayerNumber { get; set; }
        public string CollectorNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
