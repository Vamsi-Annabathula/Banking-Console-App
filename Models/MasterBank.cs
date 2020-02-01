using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    public class MasterBank
    {
        public MasterBank()
        {
            Banks = new List<Bank>();
        }
        public List<Bank> Banks { get; set; }
    }
}
