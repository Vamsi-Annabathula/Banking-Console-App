using BankTransfer.Helpers;
using BankTransfer.Models;
using BankTransfer.Models.User;

namespace BankTransfer.Services
{
    class UserService: IServices.IUser
    {
        BanksList banksModel;
        public UserService(BanksList banksList)
        {
            banksModel = banksList;
        }
        public string CreateUser(string userName, string passWord,string email, string address, long phoneNumber,  string bankId)
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
            account.User.EmailAddress = email;
            account.User.Address = address;
            account.User.PhoneNumber = phoneNumber;
            return AppConstants.Success;
        }

        public string CreateUser(string userName, string passWord, int role,string email, string address, long phoneNumber, string bankId)
        {
            string accId = IdGenerator.CreateAccountId(userName);
            banksModel.Banks
                      .Find(s => s.Id == bankId)
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
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .FindIndex(s => s.Id == accId);
            if (index != -1)
            {
                AccountHolder user = banksModel.Banks
                      .Find(s => s.Id == bankId)
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
                return AppConstants.Success;
            }
            else
            {
                return AppConstants.NoUser;
            }
            
        }

        public bool ValidateUser(string accId, string passWord, string bankId)
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

        public bool ValidateStaff(string userId, string passWord, string bankId)
        {
            int index = banksModel.Banks
                      .Find(s => s.Id == bankId)
                      .StaffList
                      .FindIndex(s => s.Id == userId);
            Staff staff = banksModel.Banks.Find(s => s.Id == bankId).StaffList.Find(s => s.Id == userId);
            if (index != -1 && staff.IsActive != false && staff.Password == passWord)
            {
                return true;
            }
            return false;
        }
    }
}
