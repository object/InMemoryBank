using System;

namespace Implementation.Bank
{
    public class Payment
    {
        public DateTime TransferTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public User Payer { get; set; }
        public User Collector { get; set; }
        public decimal Amount { get; set; }
        public decimal PayerFee { get; set; }
        public decimal CollectorFee { get; set; }
    }
}
