using System;
using System.Collections.Generic;
using System.Text;
using BankTransfer.IServices;
using BankTransfer.Models;

namespace BankTransfer.Services
{
    public class TransactionService: ITransaction
    {
       
        public void AddTransaction(string transacId, string desc, string transacFromAccId, string transacToAccId, decimal amount, TransactionType type, string fromBankId, string toBankId, Account account)
        {
            account.Transactions.Add(new Transaction() { Id = transacId, Description = desc, SenderAccId = transacFromAccId, ReceiverAccId = transacToAccId, Amount = amount, Type = type, SenderBankId = fromBankId, ReceiverBankId = toBankId });
        }

        public List<Transaction> GetAlltransactions(string accId, string bankId, Account account)
        {
            List<Transaction> _ = new List<Transaction>();
            try
            {
                _ = account
                   .Transactions;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("No Transactions present");
            }
            return _;
        }

        public string RevertTransaction(string accId, string transacId, string bankId, Account account)
        {
            Transaction transac = account.Transactions.Find(s => s.Id == transacId);

            int index = account.Transactions.FindIndex(s => s.Id == transacId);
            if (index != -1)
            {
                string fromAccId = transac.SenderAccId;
                string toAccId = transac.ReceiverAccId;
                decimal amount = account.Transactions.Find(s => s.Id == fromAccId).Amount;

                if (transac.Type == (TransactionType)Enum.Parse(typeof(TransactionType), "Transfer"))
                {
                    account.Balance += amount;
                    account.Balance -= amount;
                    return AppConstants.RevertSuccess;
                }
                else if (transac.Type == (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"))
                {
                    account.Balance -= amount;
                }
                else if (transac.Type == (TransactionType)Enum.Parse(typeof(TransactionType), "Withdraw"))
                {
                    account.Balance += amount;
                }
            }
            return "No such transaction for given account Id exits";
        }
    }
}
