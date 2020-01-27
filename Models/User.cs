using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class User
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public Role RoleEnum { get; set; }

        public enum Role
        {
            Staff = 1,
            User
        }
    }
}
