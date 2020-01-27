using BankTransfer.Helpers;
using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer
{
    class UserService
    {
        public void CreateUser(string userName, string passWord, string role, string bankId, Models.BanksLlist banksModel)
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
            account.User.RoleEnum = (User.Role)Enum.Parse(typeof(User.Role),role);
        }

        public void UpdateUser(string accId, string userName, string passWord, string role, string bankId, Models.BanksLlist banksModel)
        {
            User user = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .User;
            user.Name = userName;
            user.Password = passWord;
            user.RoleEnum = (User.Role)Enum.Parse(typeof(User.Role), role);
        }

        public void DeleteUser(string accId, string bankId, Models.BanksLlist banksModel)
        {
            banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .IsActive = false;
        }

        public bool ValidateUser(string accId, string passWord, string bankId, Models.BanksLlist banksModel)
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
