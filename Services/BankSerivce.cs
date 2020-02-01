using BankTransfer.Helpers;
using BankTransfer.IServices;
using BankTransfer.Models;
using System.Collections.Generic;

namespace BankTransfer.Services 
{
    public class BankSerivce: IBank
    {
       
        public string AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId, Bank bank)
        {
            bank
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS,string bankId, Bank bank)
        {
            bank.RTGSToSameBank = RTGS;
            bank.IMPSToSameBank = IMPS;
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId, Bank bank)
        {
            bank.RTGSToOtherBanks = RTGS;
            bank.IMPSToOtherBanks = IMPS;
            return AppConstants.Success;
        }
        
    }
}
