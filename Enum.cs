using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer
{
    public enum UserRole
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
    public enum StaffPosition
    {
        Manager,
        Employee
    }
}
