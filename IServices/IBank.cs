using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IBank
    {
        string AddCurrAndExchangeRate(string currName, decimal exchangeRate, string bankId, Bank bank);

        string UpdateServiceChargeForSameBank(decimal RTGS, decimal IMPS, string bankId, Bank bank);

        string UpdateServiceChargeForOtherBanks(decimal RTGS, decimal IMPS, string bankId, Bank bank);
    }
}
