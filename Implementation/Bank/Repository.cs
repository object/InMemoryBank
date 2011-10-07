using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Bank
{
    public class Repository
    {
        public List<User> Users { get; private set; }
        public List<PaymentFee> Fees { get; private set; }
        public List<Payment> Payments { get; private set; }

        public Repository()
        {
            this.Users = new List<User>();
            this.Fees = new List<PaymentFee>();
            this.Payments = new List<Payment>();
        }

        public User FindUser(string phoneNumber)
        {
            return this.Users.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefault();
        }

        public decimal GetPayerFee(PaymentType paymentType)
        {
            return this.Fees.Where(x => x.PaymentType == paymentType).Select(x => x.PayerFee).SingleOrDefault();
        }

        public decimal GetCollectorFee(PaymentType paymentType)
        {
            return this.Fees.Where(x => x.PaymentType == paymentType).Select(x => x.CollectorFee).SingleOrDefault();
        }
    }
}
