using BankTransfer.Helpers;
using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Services 
{   
    class AccountService
    {
        public void AddTransaction(string transacId, string desc, string transacFromAccId, string transacToAccId, decimal amount, Transaction.Type type, string fromBankId, string toBankId, BanksLlist banksModel)
        {
            banksModel.Banks.Find(s => s.Id == fromBankId).Accounts.Find(s => s.Id == transacFromAccId).Transactions.Add(new Transaction() { Id = transacId, Description = desc, SenderAccId = transacFromAccId, ReceiverAccId = transacToAccId, Amount = amount, TypeEnum = type, SenderBankId = fromBankId, ReceiverBankId = toBankId});
        }

        public List<Transaction> GetAlltransactions(string accId, string bankId, Models.BanksLlist banksModel)
        {
            List<Transaction> _ = new List<Transaction>();
            try{
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

        public bool Deposit(string depositCurr, int amount, string accId, string bankId, Models.BanksLlist banksModel)
        {
            string transacId;
            Bank bankModel = banksModel.Banks.Find(s => s.Id == bankId);
            if (depositCurr != bankModel.Currency)
            {
                decimal convertedAmount = Convert.ToDecimal(amount) * bankModel.AcceptedCurrencies.Find(s => s.Name == depositCurr).ExchangeRate;

                bankModel.Accounts.Find(s => s.Id == accId).Balance += convertedAmount;

                transacId = IdGenerator.CreateTransacId(bankModel.Id, accId);

                AddTransaction(transacId, string.Format("Deposit {0}", convertedAmount), accId, accId, convertedAmount, (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Deposit"), bankId, bankId, banksModel);

                return true;
            }
            else
            {
                bankModel.Accounts.Find(s => s.Id == accId).Balance += amount;

                transacId = IdGenerator.CreateTransacId(bankModel.Id, accId);

                AddTransaction(transacId, string.Format("Deposit {0}", amount), accId, accId, amount, (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Deposit"), bankId, bankId, banksModel);

                return true;
            }
        }

        public string WithDraw(string accId, int amount, string bankId, Models.BanksLlist banksModel)
        {
            string transacId;
            Bank bankModel = banksModel.Banks.Find(s => s.Id == bankId);
            decimal accBal = bankModel.Accounts.Find(s => s.Id == accId).Balance;
            if (accBal < amount)
            {
                return DefaultValue.InsufficientBal;
            }

            accBal -= amount;

            transacId = IdGenerator.CreateTransacId(bankId, accId);

            AddTransaction(transacId, string.Format("Withdraw {0}", amount), accId, accId, amount, (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Withdraw"), bankId, bankId, banksModel);

            return DefaultValue.CollectAmount;
        }

        public string TransferFunds(string senderId, string toBankId, string receiverId, int amount, string frombankId, Models.BanksLlist banksModel)
        {
            Bank frombankModel = banksModel.Banks.Find(s => s.Id == frombankId);
            Bank tobankModel = banksModel.Banks.Find(s => s.Id == toBankId);

            if (frombankModel.Accounts.FindIndex(s => s.Id == senderId) != -1 && tobankModel.Accounts.FindIndex(s => s.Id == receiverId) != -1)
            {
                if (frombankModel.Accounts.Find(s => s.Id == senderId).Balance < amount)
                {
                    return "Insufficient Funds to send";
                }
                else
                {
                    frombankModel.Accounts.Find(s => s.Id == senderId).Balance -= amount;

                    tobankModel.Accounts.Find(s => s.Id == receiverId).Balance += amount;

                    string fromTransacId = IdGenerator.CreateTransacId(frombankId, frombankModel.Accounts.Find(s => s.Id == senderId).User.Name);

                    AddTransaction(fromTransacId, string.Format("Transfer {0} from {1} to {2}", amount, senderId, receiverId), senderId, receiverId, amount, (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Transfer"), frombankId, toBankId, banksModel);

                    return "Transfer Successful";
                }
            }
            return "Entered User doesnt exit to transfer";
        }

        public string RevertTransaction(string accId, string transacId, string bankId, Models.BanksLlist banksModel)
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

                if (transac.TypeEnum == (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Transfer"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance += amount;
                    bankModel.Accounts.Find(s => s.Id == toAccId).Balance -= amount;
                    return DefaultValue.RevertSuccess;
                }
                else if (transac.TypeEnum == (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Deposit"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance -= amount;
                }
                else if (transac.TypeEnum == (Transaction.Type)Enum.Parse(typeof(Transaction.Type), "Withdraw"))
                {
                    bankModel.Accounts.Find(s => s.Id == fromAccId).Balance += amount;
                }
            }
            return "No such transaction for given account Id exits";
        }

    }
}
