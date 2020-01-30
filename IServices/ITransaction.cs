using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface ITransaction
    {
        void AddTransaction(string transacId, string desc, string transacFromAccId, string transacToAccId, decimal amount, TransactionType type, string fromBankId, string toBankId, BanksList banksModel);

        List<Transaction> GetAlltransactions(string accId, string bankId, BanksList banksModel);

        string RevertTransaction(string accId, string transacId, string bankId, BanksList banksModel);
    }
}
