using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.IServices
{
    public interface IMasterBank
    {
        void AddBank(MasterBank banksList, Bank bank, List<Currency> currencies);
    }
}
