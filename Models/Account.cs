using BankTransfer.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<Transaction>();
            User = new AccountHolder();
        }
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }
        
        public AccountHolder User { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
