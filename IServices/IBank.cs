using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IBank
    {
        string AddCurrAndExchangeRate(string currName, decimal exchangeRate, string bankId );

        string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS, string bankId);

        string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId);

        void AddBank(BanksList banksList, Bank bank, List<Currency> currencies);
    }
}
