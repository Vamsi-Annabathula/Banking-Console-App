using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models.User
{
    public interface IUser
    {
        string Name { get; set; }

        string Password { get; set; }

        string EmailAddress { get; set; }

        long PhoneNumber { get; set; }

        string Address { get; set; }

    }
}
