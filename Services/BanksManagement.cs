using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Services
{
    class BanksManagement
    {
        public void AddBank(Models.BanksManagement banksList, string id, string name, string currency, decimal sameIMPS, decimal sameRTGS, decimal otherIMPS, decimal otherRTGS)
        {
            banksList.Banks.Add(new Models.Bank() { Id = id, Name = name, Currency = currency, SameBankIMPS = sameIMPS, SameBankRTGS = sameRTGS, OtherBankIMPS = otherIMPS, OtherBankRTGS = otherRTGS});
        }
    }
}
