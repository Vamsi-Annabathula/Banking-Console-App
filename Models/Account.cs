using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
            User = new User();
        }
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }
        
        public User User { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
