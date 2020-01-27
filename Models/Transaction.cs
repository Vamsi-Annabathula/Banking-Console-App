using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class Transaction
    {
        public string Id { get; set; }

        public string SenderAccId { get; set; }

        public decimal Amount { get; set; }

        public string ReceiverAccId { get; set; }

        public string SenderBankId { get; set; }

        public string ReceiverBankId { get; set; }

        public string Description { get; set; }

        public Type TypeEnum { get; set; }

        public enum Type
        {
            Deposit,
            Withdraw,
            Transfer
        }
    }
}
