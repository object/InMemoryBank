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
    }
}
