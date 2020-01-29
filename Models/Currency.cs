using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    public class Currency 
    {
        public string Name { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
