using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models.User
{
    public class Staff: IUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string EmailAddress { get; set; }

        public long PhoneNumber { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public StaffDesignation StaffRole { get; set; }
    }
}
