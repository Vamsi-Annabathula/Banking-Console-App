using BankTransfer.Models.User;
using System.Collections.Generic;

namespace BankTransfer.Models
{
    public class Bank
    {
        public Bank()
        {
            Accounts = new List<Account>();

            StaffList = new List<Staff>();

            AcceptedCurrencies = new List<Currency>();
        }

        public string Name { get; set; }

        public string Id { get; set; }

        public decimal RTGSToSameBank { get; set; }

        public decimal IMPSToSameBank { get; set; }

        public decimal RTGSToOtherBanks { get; set; }

        public decimal IMPSToOtherBanks { get; set; }

        public string Currency { get; set; }

        public List<Account> Accounts { get; set; }

        public List<Staff> StaffList { get; set; }

        public List<Currency> AcceptedCurrencies { get; set; }
    }
}
