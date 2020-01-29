using BankTransfer.Helpers;
using BankTransfer.Models;
using BankTransfer.Services;
using BankTransfer.Utility;
using System;
using System.Collections.Generic;

namespace BankTransfer.UI
{
    public class App
    {
        BanksLlist listOfBanks = new BanksLlist();
        public void StartApp()
        {
            int input;
            do
            {
                Console.WriteLine("Select any one Option");
                Console.WriteLine("1. Set Up Bank\n2. Create User\n3. User LogIn");
                input = NullHandler.HandleInput<int>();              
                switch (input)
                {
                    case 1:
                        SetUpBank();
                        break;
                    case 2:
                        CreateUser();
                        break;
                    case 3:
                        UserLogin();
                        break;
                    }
                
                Console.WriteLine("Want to continue? (y/n): ");
            } while (NullHandler.HandleInput<string>() != DefaultValue.No && input != 4);
            Console.WriteLine("Thank you for using our services\n\n**********************************");
        }

        public void SetUpBank()
        {
            string check;
            Bank bankModel = new Bank();
            BankSerivce bankService = new BankSerivce();
            Console.WriteLine("Enter Bank Name");
            bankModel.Name = NullHandler.HandleInput<string>();
            Console.WriteLine(bankModel.Name);
            bankModel.Id = IdGenerator.CreateAccountId(bankModel.Name);
            Console.WriteLine("Generated id: {0}", bankModel.Id);
     
            Console.WriteLine("Want to use default service charge rates (y/n): ");
            if(NullHandler.HandleInput<string>() == DefaultValue.No)
            {
                Console.WriteLine("Enter service rates for same bank");
                Console.Write("RTGS: ");
                bankModel.RTGSToSameBank = NullHandler.HandleInput<decimal>();
                Console.Write("IMPS: ");
                bankModel.IMPSToSameBank = NullHandler.HandleInput<decimal>();
                Console.WriteLine("Enter serive rate for other banks");
                Console.Write("RTGS: ");
                bankModel.RTGSToOtherBanks = NullHandler.HandleInput<decimal>();
                Console.Write("IMPS: ");
                bankModel.IMPSToOtherBanks = NullHandler.HandleInput<decimal>();

                bankService.AddBank(listOfBanks, bankModel.Id, bankModel.Name, bankModel.Currency, bankModel.IMPSToSameBank, bankModel.RTGSToSameBank, bankModel.IMPSToOtherBanks, bankModel.RTGSToOtherBanks);
            }
            else
            {
                bankModel.RTGSToSameBank = DefaultValue.SameBankRTGS;
                bankModel.IMPSToSameBank = DefaultValue.SameBankIMPS;
                bankModel.RTGSToOtherBanks = DefaultValue.OtherBankRTGS;
                bankModel.IMPSToOtherBanks = DefaultValue.OtherBankIMPS;
                bankService.AddBank(listOfBanks, bankModel.Id, bankModel.Name, bankModel.Currency, bankModel.IMPSToSameBank, bankModel.RTGSToSameBank, bankModel.IMPSToOtherBanks, bankModel.RTGSToOtherBanks);
            }

            Console.WriteLine("Want to add accepted Currencies and their exchnage rates for you bank : (y/n)");
            check = NullHandler.HandleInput<string>();
            if (check == DefaultValue.Yes)
            {
                while (check == DefaultValue.Yes)
                {
                    Console.WriteLine("Enter Currency: ");
                    string currName = NullHandler.HandleInput<string>();
                    Console.WriteLine("Enter Excahnge Rate: ");
                    decimal exchangeRate = NullHandler.HandleInput<decimal>();
                    Console.WriteLine(bankService.AddCurrAndExchangeRate(currName, exchangeRate, bankModel.Id, listOfBanks));
                    Console.WriteLine("Want to continue adding accepted Currencies and their exchnage rates for you bank : (y/n)");
                    check = NullHandler.HandleInput<string>();
                }
                Console.WriteLine("Select one of the currrencies as default accepted currency for your bank: ");
                listOfBanks.Banks.Find(s => s.Id == bankModel.Id).AcceptedCurrencies.ForEach(s => Console.WriteLine("Currency Name: {0}", s.Name));
                bankModel.Currency = NullHandler.HandleInput<string>();
            }
            else
            {
                bankService.AddCurrAndExchangeRate(DefaultValue.BankCurrency, 0, bankModel.Id, listOfBanks);
            }
            Console.WriteLine("New Bank set up successful");
        }

        public void CreateUser()
        {
            string bankId, userName, check;
            UserService userService = new UserService();

            Console.WriteLine("Select in which bank you want create bank staff or User Account: ");
            listOfBanks.Banks.ForEach(s => Console.WriteLine(s.Id));

            bankId = NullHandler.HandleInput<string>();
            do
            {
                int selectedOption = (int)Role.Staff;
                Console.WriteLine("Select User role");
                foreach (var role in Enum.GetNames(typeof(Role)))
                {
                    Console.WriteLine("{0}. {1}", selectedOption++, role);
                }
                int selectedRolenum = NullHandler.HandleInput<int>();

                switch (selectedRolenum)
                {
                    case 1:
                        Console.WriteLine("Enter User Name: ");
                        userName = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Password: ");
                        var _ = (Role)selectedRolenum;

                        userService.CreateUser(userName, NullHandler.HandleInput<string>(), _.ToString(), bankId, listOfBanks);

                        Console.WriteLine($"Account Id = {IdGenerator.CreateAccountId(userName)}");
                        break;
                    case 2:
                        Console.WriteLine("Enter User Name: ");
                        userName = NullHandler.HandleInput<string>();
                        _ = (Role)selectedRolenum;
                        Console.WriteLine("Enter Password: ");
                        userService.CreateUser(userName, NullHandler.HandleInput<string>(), _.ToString(), bankId, listOfBanks);

                        Console.WriteLine(string.Format("Account Id = {0}\n", IdGenerator.CreateAccountId(userName)));
                        break;
                    default:
                        Console.WriteLine("Select between given options");
                        break;
                }
                Console.WriteLine("Want to continue creating Users ( y/n ):");
                check = NullHandler.HandleInput<string>();
            } while (check != DefaultValue.No);
        }

        public void UserLogin()
        {
            string bankId, accId;
            UserService userService = new UserService();

            do
            {
                Console.WriteLine("Enter Bank Id: ");
                bankId = NullHandler.HandleInput<string>();

                Console.WriteLine("Enter Account Id and password to login");
                Console.WriteLine("Account Id: ");
                accId = NullHandler.HandleInput<string>();
                Console.WriteLine("Password: ");
                string role = listOfBanks.Banks
                      .Find(s => s.Id == bankId)
                      .Accounts
                      .Find(e => e.Id == accId)
                      .User.RoleEnum.ToString();

                if (userService.ValidateUser(accId, NullHandler.HandleInput<string>(), bankId, listOfBanks))
                {
                    Console.WriteLine("Log In successful");

                    //When logged user is Bank staff
                    if (role == Role.Staff.ToString())
                    {
                        StaffActions(bankId);
                    }
                    //when logged user is normal user
                    else if (role == Role.User.ToString())
                    {
                        UserActions(bankId, accId);
                    }
                }
                else
                {
                    Console.WriteLine("Wrong credentials please try again latter");
                }
                Console.WriteLine("Now Enter yes to Log In or exit for ending this program ( y/n ):");
            } while (Convert.ToBoolean(string.Compare(NullHandler.HandleInput<string>(), DefaultValue.No)));
        }

        public void StaffActions(string bankId)
        {
            BankSerivce bankService = new BankSerivce();
            UserService userService = new UserService();
            TransactionService transaction = new TransactionService();
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("want to continue with this account Managements ( y/n ): ");
            while (NullHandler.HandleInput<string>() == DefaultValue.Yes)
            {
                Console.WriteLine("Select one of the below options: \n1. Create user account. \n2. Update User. \n3. Delete User" +
                    "\n4. Add new accepted currency and exchange rates. \n5. Update service rates for same bank\n6. Update service rates for other banks" + 
                    "7. View an user transaction history. \n8. Revert transasction. \n");

                switch (NullHandler.HandleInput<int>())
                {
                    case 1:
                        Console.WriteLine("Enter User name, Password ");
                        Console.WriteLine(userService.CreateUser(NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), Role.User.ToString(), bankId, listOfBanks));
                        break;
                    case 2:
                        Console.WriteLine("Enter Account Id, User Name, Password to Update");
                        Console.WriteLine(userService.UpdateUser(NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), bankId, listOfBanks));
                        break;

                    case 3:
                        Console.WriteLine("Enter AccountId to delete user account");
                        Console.WriteLine(userService.DeleteUser(NullHandler.HandleInput<string>(), bankId, listOfBanks));
                        break;

                    case 4:
                        Console.WriteLine("Enter new currency name and its exchange rate:");
                        Console.WriteLine(bankService.AddCurrAndExchangeRate(NullHandler.HandleInput<string>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;

                    case 5:
                        Console.WriteLine("Enter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForSameBank(NullHandler.HandleInput<decimal>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;
                    case 6:
                        Console.WriteLine("Enter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForOtherBanks(NullHandler.HandleInput<decimal>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;
                    case 7:
                        Console.WriteLine("Enter Account Id to view transaction: ");
                        transactions = transaction.GetAlltransactions(NullHandler.HandleInput<string>(), bankId, listOfBanks);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    case 8:
                        Console.WriteLine("Enter Account Id and Transaction Id to revert that transasction");
                        Console.WriteLine(transaction.RevertTransaction(NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), bankId, listOfBanks));
                        break;

                    default:
                        Console.WriteLine("Select options from 1 to 6 only");
                        break;
                }
                Console.WriteLine("Want to continue with this account ( y/n ):");
            }
        }

        public void UserActions(string bankId, string accId)
        {
            AccountService accountService = new AccountService();
            TransactionService transaction = new TransactionService();
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("Want to continue with this account Managements ( y/n ): ");
            while (NullHandler.HandleInput<string>() == DefaultValue.Yes)
            {
                Console.WriteLine("Select one of the below options: \n1. Deposit amount. \n2. Withdraw amount. \n3. Transfer Funds" +
                    "\n4. View transaction history \n");

                switch (NullHandler.HandleInput<int>())
                {
                    case 1:
                        Console.WriteLine("Enter in which currency you want deposit, amount:\n");
                        accountService.Deposit(NullHandler.HandleInput<string>(), NullHandler.HandleInput<int>(), accId, bankId, listOfBanks);
                        Console.WriteLine("Successful");
                        break;

                    case 2:
                        Console.WriteLine("Enter amount to be withdrawn: \n");
                        Console.WriteLine(accountService.WithDraw(accId, NullHandler.HandleInput<int>(), bankId, listOfBanks));
                        break;

                    case 3:
                        Console.WriteLine("Enter bank Id, account Id and amount to transfer your savings");
                        Console.WriteLine(accountService.TransferFunds(accId, NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), NullHandler.HandleInput<int>(), bankId, listOfBanks));
                        break;

                    case 4:
                        transactions = transaction.GetAlltransactions(accId, bankId, listOfBanks);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    default:
                        Console.WriteLine("select options from 1  to 4 only\n");
                        break;
                }
            }
        }
    }
}
