using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IBank
    {
        string AddCurrAndExchangeRate(string currName, decimal exchangeRate, string bankId, BanksList banksModel);

        string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS, string bankId, BanksList banksModel);

        string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId, BanksList banksModel);

        void AddBank(BanksList banksList, Bank bank, List<Currency> currencies);
    }
}
