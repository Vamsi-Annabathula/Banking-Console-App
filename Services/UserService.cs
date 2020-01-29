using BankTransfer.Helpers;
using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Services
{
    class UserService
    {
        public string CreateUser(string userName, string passWord, string role, string bankId, BanksLlist banksModel)
        {
            string accId = IdGenerator.CreateAccountId(userName);
            banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Add(new Account { Id = accId, Balance = 0, IsActive = true });
            Account account = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId);
            account.User.Name = userName;
            account.User.Password = passWord;
            account.User.RoleEnum = (Role)Enum.Parse(typeof(Role),role);
            return DefaultValue.Success;
        }

        public string UpdateUser(string accId, string userName, string passWord, string bankId, BanksLlist banksModel)
        {
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            if (index != -1)
            {
                User user = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .User;
                user.Name = userName;
                user.Password = passWord;
                return DefaultValue.Success;
            }
            else
            {
                return DefaultValue.NoUser;
            }
        }

        public string DeleteUser(string accId, string bankId, BanksLlist banksModel)
        {
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            if(index != -1)
            {
                banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .IsActive = false;
                return DefaultValue.Success;
            }
            else
            {
                return DefaultValue.NoUser;
            }
            
        }

        public bool ValidateUser(string accId, string passWord, string bankId, BanksLlist banksModel)
        {
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            Account acc = banksModel.Banks.Find(s => s.Id == bankId).Accounts.Find(s => s.Id == accId);
            if (index != -1 && acc.IsActive != false && acc.User.Password == passWord)
            {
                return true;
            }
            return false;
        }
    }
}
