using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation;
using Implementation.Bank;

namespace Specifications.Infrastructure
{
    class RepositoryHelper
    {
        internal static void EnsureUserDontExist(string phoneNumber)
        {
            Application.Instance.Repository.Users
                 .Where(u => u.PhoneNumber == phoneNumber).ToList()
                 .ForEach(u =>
                     u.PhoneNumber = Guid.NewGuid().ToString());
        }
        internal static void EnsureUserExists(User user)
        {
            Application.Instance.Repository.Users
                .Where(u => u.PhoneNumber == user.PhoneNumber).ToList()
                .ForEach(u =>
                    u.PhoneNumber = Guid.NewGuid().ToString());
            Application.Instance.Repository.Users.Add(user);
        }

        internal static void SetPaymentFees(IEnumerable<PaymentFee> fees)
        {
            Application.Instance.Repository.Fees.Clear();
            fees.ToList()
                .ForEach(x =>
                    Application.Instance.Repository.Fees.Add(x));
        }
    }
}
