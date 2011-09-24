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

    class Steps
    {
        [Given(@"user with phone number (\w+) is not registered")]
        public void GivenUserWithPhoneNumberIsNotRegistered(string phoneNumber)
        {
            RepositoryHelper.EnsureUserDontExist(phoneNumber);
        }

        [When(@"user sends SMS")]
        public void WhenUserSendsSMS(Table table)
        {
            var message = table.CreateSet<SmsMessage>().Single();
            Application.Instance.ReceivedMessages.Enqueue(message);
            Thread.Sleep(100);
        }

        [Then(@"following SMS should be sent")]
        public void ThenFollowingSMSShouldBeSent(Table table)
        {
            table.CreateSet<SmsMessage>().ToList()
                .ForEach(message =>
                    Assert.Contains(message, Application.Instance.SentMessages));
        }
    }
}
