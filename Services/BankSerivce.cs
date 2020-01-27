using BankTransfer.Helpers;
using BankTransfer.Models;

namespace BankTransfer.Services 
{
    class BankSerivce
    {
        public void AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId, BanksLlist banksModel)
        {
            banksModel.Banks
               .Find(s => s.Id == bankId)
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
        }

        public void AddBank(BanksLlist banksList, string id, string name, string currency, decimal sameIMPS, decimal sameRTGS, decimal otherIMPS, decimal otherRTGS)
        {
            banksList.Banks.Add(new Models.Bank() { Id = id, Name = name, Currency = currency, IMPSToSameBank = sameIMPS, RTGSToSameBank = sameRTGS, IMPSToOtherBanks = otherIMPS, RTGSToOtherBanks = otherRTGS });
        }
    }
}
