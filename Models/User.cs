using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class User
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

    }
}
