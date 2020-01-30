using BankTransfer.Helpers;
using BankTransfer.IServices;
using BankTransfer.Models;

namespace BankTransfer.Services 
{
    public class BankSerivce: IBank
    {
        public string AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId, BanksList banksModel)
        {
            banksModel.Banks
               .Find(s => s.Id == bankId)
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS,string bankId, BanksList banksModel)
        {
            Bank bank = banksModel.Banks.Find(s => s.Id == bankId);
            bank.RTGSToSameBank = RTGS;
            bank.IMPSToSameBank = IMPS;
            return AppConstants.Success;
        }
        public string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId, BanksList banksModel)
        {
            Bank bank = banksModel.Banks.Find(s => s.Id == bankId);
            bank.RTGSToOtherBanks = RTGS;
            bank.IMPSToOtherBanks = IMPS;
            return AppConstants.Success;
        }
        public void AddBank(BanksList banksList, string id, string name, string currency, decimal sameIMPS, decimal sameRTGS, decimal otherIMPS, decimal otherRTGS)
        {
            banksList.Banks.Add(new Bank() { Id = id, Name = name, Currency = currency, IMPSToSameBank = sameIMPS, RTGSToSameBank = sameRTGS, IMPSToOtherBanks = otherIMPS, RTGSToOtherBanks = otherRTGS });
        }
    }
}
