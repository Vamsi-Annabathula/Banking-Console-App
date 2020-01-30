using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IAccount
    {
        bool Deposit(string depositCurr, int amount, string accId, string bankId, BanksList banksModel);

        string WithDraw(string accId, int amount, string bankId, BanksList banksModel);

        string TransferFunds(string senderId, string toBankId, string receiverId, int amount, string frombankId, BanksList banksModel);

    }
}
