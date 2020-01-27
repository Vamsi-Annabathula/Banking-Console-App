using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class Bank
    {
        public Bank()
        {
            Accounts = new List<Account>();

            AcceptedCurrencies = new List<Currency>();
        }

        public string Name { get; set; }

        public string Id { get; set; }

        public decimal SameBankRTGS { get; set; }

        public decimal OtherBankRTGS { get; set; }

        public decimal SameBankIMPS { get; set; }

        public decimal OtherBankIMPS { get; set; }

        public string Currency { get; set; }

        public List<Account> Accounts { get; set; }

        public List<Currency> AcceptedCurrencies { get; set; }
    }
}
