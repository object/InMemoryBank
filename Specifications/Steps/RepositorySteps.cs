using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Implementation;
using Implementation.Bank;
using Implementation.Sms;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using Specifications.Infrastructure;

namespace Specifications.Steps
{
    [Binding]
    class RepositorySteps
    {
        [Given(@"following users are registered")]
        public void GivenFollowingUsersAreRegistered(Table table)
        {
            table.CreateSet<User>().ToList()
                .ForEach(x => RepositoryHelper.EnsureUserExists(x));
        }

        [Given(@"user with phone number (\w+) is not registered")]
        public void GivenUserWithPhoneNumberIsNotRegistered(string phoneNumber)
        {
            RepositoryHelper.EnsureUserDontExist(phoneNumber);
        }

        [Given(@"user with phone number (\w+) is registered")]
        public void GivenUserWithPhoneNumberIsRegistered(string phoneNumber)
        {
            RepositoryHelper.EnsureUserExists(new User() { PhoneNumber = phoneNumber });
        }

        [Given(@"payment fee is as follows")]
        public void GivenPaymentFeeIsAsFollows(Table table)
        {
            var fees = table.CreateSet<PaymentFee>();
            RepositoryHelper.SetPaymentFees(fees);
        }
    }
}
