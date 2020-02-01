using BankTransfer.Helpers;
using BankTransfer.Models;
using BankTransfer.Models.User;

namespace BankTransfer.Services
{
    class UserService: IServices.IUser
    {
        Bank bank;
        public UserService(Bank bankModel)
        {
            bank = bankModel;
        }
        public string CreateUser(string userName, string passWord,string email, string address, long phoneNumber,  string bankId)
        {
            string accId = IdGenerator.CreateAccountId(userName);
            bank
                .Accounts
                .Add(new Account { Id = accId, Balance = 0, IsActive = true });

            Account account = bank
                      .Accounts
                      .Find(s => s.Id == accId);
            account.User.Name = userName;
            account.User.Password = passWord;
            account.User.EmailAddress = email;
            account.User.Address = address;
            account.User.PhoneNumber = phoneNumber;
            return AppConstants.Success;
        }

        public string CreateUser(string userName, string passWord, int role,string email, string address, long phoneNumber, string bankId)
        {
            string accId = IdGenerator.CreateAccountId(userName);
            bank
                .StaffList
                .Add(new Staff { Id = accId, 
                          Address = address, 
                          EmailAddress = email, 
                          Name = userName, 
                          Password = passWord, 
                          PhoneNumber = phoneNumber,
                          IsActive = true,
                          StaffRole = (StaffDesignation)role
                      });
            return AppConstants.Success;
        }

        public string UpdateUser(string accId, string userName, string passWord,string email, string address, long phoneNumber, string bankId)
        {
            int index = bank
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            if (index != -1)
            {
                AccountHolder user = bank
                      .Accounts
                      .Find(s => s.Id == accId)
                      .User;
                user.Name = userName;
                user.Password = passWord;
                user.Address = address;
                user.EmailAddress = email;
                user.PhoneNumber = phoneNumber;
                return AppConstants.Success;
            }
            else
            {
                return AppConstants.NoUser;
            }
        }

        public string DeleteUser(string accId, string bankId)
        {
            int index = bank
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            if(index != -1)
            {
                bank
                      .Accounts
                      .Find(s => s.Id == accId)
                      .IsActive = false;
                return AppConstants.Success;
            }
            else
            {
                return AppConstants.NoUser;
            }
            
        }

        public bool ValidateUser(string accId, string passWord, string bankId)
        {
            int index = bank
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            Account acc = bank.Accounts.Find(s => s.Id == accId);
            if (index != -1 && acc.IsActive != false && acc.User.Password == passWord)
            {
                return true;
            }
            return false;
        }

        public bool ValidateStaff(string userId, string passWord, string bankId)
        {
            int index = bank
                      .StaffList
                      .FindIndex(s => s.Id == userId);
            Staff staff = bank.StaffList.Find(s => s.Id == userId);
            if (index != -1 && staff.IsActive != false && staff.Password == passWord)
            {
                return true;
            }
            return false;
        }
    }
}
