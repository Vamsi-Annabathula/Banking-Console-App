using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    public class Transaction
    {
        public string Id { get; set; }
       
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string SenderAccId { get; set; }

        public string ReceiverAccId { get; set; }

        public string SenderBankId { get; set; }

        public string ReceiverBankId { get; set; }

        public string Description { get; set; }
    }
}
