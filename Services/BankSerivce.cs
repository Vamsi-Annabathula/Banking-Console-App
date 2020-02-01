using BankTransfer.Helpers;
using BankTransfer.IServices;
using BankTransfer.Models;
using System.Collections.Generic;

namespace BankTransfer.Services 
{
    public class BankSerivce: IBank
    {
        Bank bank;
        public BankSerivce(Bank banksModel)
        {
            bank = banksModel;
        }
        public string AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId)
        {
            bank
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS,string bankId)
        {
            bank.RTGSToSameBank = RTGS;
            bank.IMPSToSameBank = IMPS;
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId)
        {
            bank.RTGSToOtherBanks = RTGS;
            bank.IMPSToOtherBanks = IMPS;
            return AppConstants.Success;
        }
        public void AddBank(BanksList banksList, Bank bank, List<Currency> currencies)
        {
            banksList.Banks.Add(new Bank() { 
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
