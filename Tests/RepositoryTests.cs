using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Implementation.Bank;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void FindUser_Existing()
        {
            var repository = new Repository();
            string phoneNumber = "12345678";
            repository.Users.Add(new User {PhoneNumber = phoneNumber});
            Assert.IsNotNull(repository.FindUser(phoneNumber));
        }

        [Test]
        public void FindUser_NonExisting()
        {
            var repository = new Repository();
            Assert.IsNull(repository.FindUser("00000000"));
        }
    }
}
