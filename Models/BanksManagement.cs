using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Models
{
    class BanksManagement
    {
        public BanksManagement()
        {
            Banks = new List<Bank>();
        }
        public List<Bank> Banks { get; set; }
    }
}
