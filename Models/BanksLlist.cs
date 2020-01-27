using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class BanksLlist
    {
        public BanksLlist()
        {
            Banks = new List<Bank>();
        }
        public List<Bank> Banks { get; set; }
    }
}
