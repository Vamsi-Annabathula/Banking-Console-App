using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    public class BanksList
    {
        public BanksList()
        {
            Banks = new List<Bank>();
        }
        public List<Bank> Banks { get; set; }
    }
}
