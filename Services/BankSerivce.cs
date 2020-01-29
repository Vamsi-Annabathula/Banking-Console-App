using BankTransfer.Helpers;
using BankTransfer.Models;

namespace BankTransfer.Services 
{
    public class BankSerivce
    {
        public string AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId, BanksLlist banksModel)
        {
            banksModel.Banks
               .Find(s => s.Id == bankId)
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
            return DefaultValue.Success;
        }
        public string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS,string bankId, BanksLlist banksModel)
        {
            Bank bank = banksModel.Banks.Find(s => s.Id == bankId);
            bank.RTGSToSameBank = RTGS;
            bank.IMPSToSameBank = IMPS;
            return DefaultValue.Success;
        }
        public string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId, BanksLlist banksModel)
        {
            Bank bank = banksModel.Banks.Find(s => s.Id == bankId);
            bank.RTGSToOtherBanks = RTGS;
            bank.IMPSToOtherBanks = IMPS;
            return DefaultValue.Success;
        }
        public void AddBank(BanksLlist banksList, string id, string name, string currency, decimal sameIMPS, decimal sameRTGS, decimal otherIMPS, decimal otherRTGS)
        {
            banksList.Banks.Add(new Bank() { Id = id, Name = name, Currency = currency, IMPSToSameBank = sameIMPS, RTGSToSameBank = sameRTGS, IMPSToOtherBanks = otherIMPS, RTGSToOtherBanks = otherRTGS });
        }
    }
}
