using System;

namespace Implementation.Bank
{
    public class PaymentFee
    {
        public PaymentType PaymentType { get; set; }
        public decimal PayerFee { get; set; }
        public decimal CollectorFee { get; set; }
    }
}
