using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.IServices
{
    public interface IUser
    {
        string CreateUser(string userName, string passWord, string email, string address, long phoneNumber, string bankId  );

        string CreateUser(string userName, string passWord, int role, string email, string address, long phoneNumber, string bankId  );

        string UpdateUser(string accId, string userName, string passWord, string email, string address, long phoneNumber, string bankId );

        string DeleteUser(string accId, string bankId);

        bool ValidateStaff(string accId, string passWord, string bankId );
    }
}
