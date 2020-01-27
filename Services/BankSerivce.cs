using BankTransfer.Helpers;
using BankTransfer.Models;

namespace BankTransfer.Services 
{
    class BankSerivce
    {
        public void AddCurrAndExchangeRate(string currName, decimal exchangeRate,string bankId, Models.BanksManagement banksModel)
        {
            banksModel.Banks
               .Find(s => s.Id == bankId)
               .AcceptedCurrencies
               .Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
        }

        public void CreateUser(string userName, string passWord, string role,string bankId, Models.BanksManagement banksModel)
        {
            string accId = IdGenerator.CreateAccountId(userName);
            banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Add(new Account { Id = accId, Balance = 0, Status = true });
            Account account = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId);
            account.User.Name = userName;
            account.User.Password = passWord;
            account.User.Role = role;
            account.User.IsActive = true;
        }

        public void UpdateUser(string accId, string userName, string passWord, string role, string bankId, Models.BanksManagement banksModel)
        {
            User user = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .User;
            user.Name = userName;
            user.Role = role;
            user.Password = passWord;
        }

        public void DeleteUser(string accId, string bankId, Models.BanksManagement banksModel)
        {
            banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(s => s.Id == accId)
                      .User.IsActive = false;
        }

        public bool ValidateUser(string accId, string passWord, string bankId, Models.BanksManagement banksModel)
        {
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            User user = banksModel.Banks.Find(s => s.Id == bankId).Accounts.Find(s => s.Id == accId).User;
            if (index != -1 && user.IsActive != false && user.Password == passWord)
            {
                return true;
            }
            return false;
        }
    }
}
