﻿using BankTransfer.Helpers;
using BankTransfer.IServices;
using BankTransfer.Models;
using System;

namespace BankTransfer.Services 
{   
    public class AccountService: IAccount
    {
        TransactionService transaction;
        public void AccountServices()
        {
            transaction = new TransactionService();
        }
        public bool Deposit(string depositCurr, int amount, string accId, string bankId, BanksList banksModel)
        {
            string transacId;
            Bank bankModel = banksModel.Banks.Find(s => s.Id == bankId);
            if (depositCurr != bankModel.Currency)
            {
                decimal convertedAmount = Convert.ToDecimal(amount) * bankModel.AcceptedCurrencies.Find(s => s.Name == depositCurr).ExchangeRate;

                bankModel.Accounts.Find(s => s.Id == accId).Balance += convertedAmount;

                transacId = IdGenerator.CreateTransacId(bankModel.Id, accId);

                transaction.AddTransaction(transacId, string.Format("Deposit {0}", convertedAmount), accId, accId, convertedAmount, (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"), bankId, bankId, banksModel);

                return true;
            }
            else
            {
                bankModel.Accounts.Find(s => s.Id == accId).Balance += amount;

                transacId = IdGenerator.CreateTransacId(bankModel.Id, accId);

                transaction.AddTransaction(transacId, string.Format("Deposit {0}", amount), accId, accId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Deposit"), bankId, bankId, banksModel);

                return true;
            }
        }

        public string WithDraw(string accId, int amount, string bankId, BanksList banksModel)
        {
            string transacId;
            Bank bankModel = banksModel.Banks.Find(s => s.Id == bankId);
            decimal accBal = bankModel.Accounts.Find(s => s.Id == accId).Balance;
            if (accBal < amount)
            {
                return AppConstants.InsufficientBal;
            }

            accBal -= amount;

            transacId = IdGenerator.CreateTransacId(bankId, accId);

            transaction.AddTransaction(transacId, string.Format("Withdraw {0}", amount), accId, accId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Withdraw"), bankId, bankId, banksModel);

            return AppConstants.CollectAmount;
        }

        public string TransferFunds(string senderId, string toBankId, string receiverId, int amount, string frombankId, BanksList banksModel)
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

                    transaction.AddTransaction(fromTransacId, string.Format("Transfer {0} from {1} to {2}", amount, senderId, receiverId), senderId, receiverId, amount, (TransactionType)Enum.Parse(typeof(TransactionType), "Transfer"), frombankId, toBankId, banksModel);

                    return "Transfer Successful";
                }
            }
            return "Entered User doesnt exit to transfer";
        }

    }
}
