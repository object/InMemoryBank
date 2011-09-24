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

        internal static void EnsureUserExists(string phoneNumber)
        {
            EnsureUserDontExist(phoneNumber);

            Application.Instance.Repository.Users.Add(new User() {PhoneNumber = phoneNumber});
        }
    }
}
