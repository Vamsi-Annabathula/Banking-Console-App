using BankTransfer.Helpers;
using BankTransfer.IServices;
using BankTransfer.Models;
using System;

namespace BankTransfer.Services 
{   
    public class AccountService: IAccount
    {
        private TransactionService transactionService;
        public AccountService( )
        {
            transactionService = new TransactionService();
        }
        public bool Deposit(string depositCurr, int amount, string accId, string bankId, Bank bank)
        {
            string transacId;
            Account account = bank.Accounts.Find(s => s.Id == accId);
            if (depositCurr != bank.Currency.Name)
            {
                decimal convertedAmount = Convert.ToDecimal(amount) * bank.AcceptedCurrencies.Find(s => s.Name == depositCurr).ExchangeRate;

                bank.Accounts.Find(s => s.Id == accId).Balance += convertedAmount;

                transacId = IdGenerator.CreateTransacId(bank.Id, accId);

                transactionService.AddTransaction(transacId, string.Format("Deposit {0}", convertedAmount), accId, accId, convertedAmount, (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"), bankId, bankId, account);

                return true;
            }
            else
            {
                bank.Accounts.Find(s => s.Id == accId).Balance += amount;

                transacId = IdGenerator.CreateTransacId(bank.Id, accId);

                transactionService.AddTransaction(transacId, string.Format("Deposit {0}", amount), accId, accId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"), bankId, bankId, account);

                return true;
            }
        }

        public string WithDraw(string accId, int amount, string bankId, Bank bank)
        {
            string transacId;
            Account account = bank.Accounts.Find(s => s.Id == accId);
            decimal accBal = account.Balance;
            if (accBal < amount)
            {
                return AppConstants.InsufficientBal;
            }

            accBal -= amount;

            transacId = IdGenerator.CreateTransacId(bankId, accId);

            transactionService.AddTransaction(transacId, string.Format("Withdraw {0}", amount), accId, accId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Withdraw"), bankId, bankId, account);

            return AppConstants.CollectAmount;
        }

        public string TransferFunds(string senderId, string toBankId, string receiverId, int amount, string frombankId, MasterBank banksModel)
        {
            Bank frombankModel = banksModel.Banks.Find(s => s.Id == frombankId);
            Bank tobankModel = banksModel.Banks.Find(s => s.Id == toBankId);
            Account senderAccount = banksModel.Banks.Find(s => s.Id == frombankId).Accounts.Find(s => s.Id == senderId);
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

                    transactionService.AddTransaction(fromTransacId, string.Format("Transfer {0} from {1} to {2}", amount, senderId, receiverId), senderId, receiverId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Transfer"), frombankId, toBankId, senderAccount);

                    return "Transfer Successful";
                }
            }
            return "Entered User doesnt exit to transfer";
        }
        public decimal ViewBalance(string accId,string bankId, Bank bank)
        {
            return bank.Accounts.Find(s => s.Id == accId).Balance;
        }
    }
}
