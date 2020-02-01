using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IAccount
    {
        bool Deposit(string depositCurr, int amount, string accId, string bankId, Bank bank);

        string WithDraw(string accId, int amount, string bankId, Bank bank);

        string TransferFunds(string senderId, string toBankId, string receiverId, int amount, string frombankId, MasterBank banksList);

        decimal ViewBalance(string accId, string bankId, Bank bank);

    }
}
