using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Services
{
    public class MasterBankService
    {
        public void AddBank(MasterBank banksList, Bank bank, List<Currency> currencies)
        {
            banksList.Banks.Add(new Bank()
            {
                Id = bank.Id,
                Name = bank.Name,
                Currency = bank.Currency,
                IMPSToSameBank = bank.IMPSToSameBank,
                RTGSToSameBank = bank.RTGSToSameBank,
                IMPSToOtherBanks = bank.IMPSToOtherBanks,
                RTGSToOtherBanks = bank.IMPSToOtherBanks,
                AcceptedCurrencies = currencies
            });
        }
    }
}
