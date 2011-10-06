using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Bank
{
    public class Repository
    {
        public List<User> Users { get; private set; }

        public Repository()
        {
            this.Users = new List<User>();
        }

        public User FindUser(string phoneNumber)
        {
            return this.Users.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefault();
        }
    }
}
