using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Helpers
{
    static class IdGenerator
    {
        public static string CreateAccountId(string userName)
        {
            if(userName.Length >= 3)
            {
                return userName.Substring(0, 3) + DateTime.Now.ToString("MMddyyyyHmm");
            }
            else
            {
                return null;
            }
        }
        public static string CreateUserId(string userName)
        {
            if (userName.Length >= 3)
            {
                return userName.Substring(0, 3) + DateTime.Now.ToString("Hmm");
            }
            else
            {
                return null;
            }
        }
        public static string CreateTransacId(string bankId, string accId)
        {
            return $"TXN{bankId}{accId}{DateTime.Now.ToString("MMddyyyyHmm")}";
        }
    }
}
