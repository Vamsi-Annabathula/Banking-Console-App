using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Helpers
{
    static class IdGenerator
    {
        public static string CreateAccountId(string userName)
        {
           return userName.Substring(0, 3) + DateTime.Now.ToString("MMddyyyyHmm");
        }
        public static string CreateUserId(string userName)
        {
            return userName.Substring(0, 3) + DateTime.Now.ToString("Hmm");
            
        }
        public static string CreateTransacId(string bankId, string accId)
        {
            return $"TXN{bankId}{accId}{DateTime.Now.ToString("MMddyyyyHmm")}";
        }
    }
}
