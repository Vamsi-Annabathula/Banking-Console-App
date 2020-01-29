using System;
using System.Collections.Generic;
using System.Text;
using BankTransfer.Models;

namespace BankTransfer.Services
{
    public class TransactionService
    {
        public void AddTransaction(string transacId, string desc, string transacFromAccId, string transacToAccId, decimal amount, TransactionType type, string fromBankId, string toBankId, BanksLlist banksModel)
        {
            banksModel.Banks.Find(s => s.Id == fromBankId).Accounts.Find(s => s.Id == transacFromAccId).Transactions.Add(new Transaction() { Id = transacId, Description = desc, SenderAccId = transacFromAccId, ReceiverAccId = transacToAccId, Amount = amount, TypeEnum = type, SenderBankId = fromBankId, ReceiverBankId = toBankId });
        }

        public List<Transaction> GetAlltransactions(string accId, string bankId, BanksLlist banksModel)
        {
            List<Transaction> _ = new List<Transaction>();
            try
            {
                _ = banksModel.Banks
                   .Find(s => s.Id == bankId)
                   .Accounts
                   .Find(s => s.Id == accId)
                   .Transactions;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No Transactions present");
            }
            return _;
        }

        public string RevertTransaction(string accId, string transacId, string bankId, BanksLlist banksModel)
        {
            Bank bankModel = banksModel.Banks.Find(s => s.Id == bankId);
            Account acc = bankModel.Accounts.Find(s => s.Id == accId);
            Transaction transac = acc.Transactions.Find(s => s.Id == transacId);

            int index = bankModel.Accounts.Find(s => s.Id == acc.Id).Transactions.FindIndex(s => s.Id == transacId);
            if (index != -1)
            {
                string fromAccId = transac.SenderAccId;
                string toAccId = transac.ReceiverAccId;
                decimal amount = bankModel.Accounts.Find(s => s.Id == fromAccId).Transactions.Find(s => s.Id == fromAccId).Amount;

                if (transac.TypeEnum == (TransactionType)Enum.Parse(typeof(TransactionType), "Transfer"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance += amount;
                    bankModel.Accounts.Find(s => s.Id == toAccId).Balance -= amount;
                    return DefaultValue.RevertSuccess;
                }
                else if (transac.TypeEnum == (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance -= amount;
                }
                else if (transac.TypeEnum == (TransactionType)Enum.Parse(typeof(TransactionType), "Withdraw"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance += amount;
                }
            }
            return "No such transaction for given account Id exits";
        }
    }
}
