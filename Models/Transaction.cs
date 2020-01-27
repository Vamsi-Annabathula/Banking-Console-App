using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class Transaction
    {
        public string Id { get; set; }

        public string FromAccId { get; set; }

        public decimal Amount { get; set; }

        public string ToAccId { get; set; }

        public string FromBankId { get; set; }

        public string ToBankId { get; set; }

        public string Desc { get; set; }

        public bool DebitFrom { get; set; }

        public bool CreditTo { get; set; }
    }
}
