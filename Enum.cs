using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer
{
    public enum Role
    {
        Staff = 1,
        User
    }
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }
}
