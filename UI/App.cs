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
        private readonly BanksList listOfBanks = new BanksList();
        
        public void StartApp()
        {
            Console.WriteLine("Please Set Up Bank");
            do
            {
                SetUpBank();
                Console.WriteLine("Please Create User: ");
                do
                {
                    CreateUser();
                } while (NullHandler.HandleInput<string>() != AppConstants.No);

                Console.WriteLine("Want to Set Up other new Bank (y/n):");
            } while (NullHandler.HandleInput<string>() != AppConstants.No);
            Console.WriteLine("***************************User LogIn********************************");
            do
            {
                UserLogin();
            } while (NullHandler.HandleInput<string>() != AppConstants.No);
        }   
        public void SetUpBank()
        {
            string check;
            Bank bankModel = new Bank();
            BankSerivce bankService = new BankSerivce();

            Console.WriteLine("\vEnter Bank Name");
            bankModel.Name = NullHandler.HandleInput<string>();
            bankModel.Id = IdGenerator.CreateAccountId(bankModel.Name);
            Console.WriteLine("Generated id: {0}", bankModel.Id);
     
            Console.WriteLine("\vWant to use default service charge rates (y/n): ");
            if(NullHandler.HandleInput<string>() == AppConstants.No)
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
                List<Currency> currencies = new List<Currency>();
                Console.WriteLine("\vWant to add accepted Currencies and their exchnage rates for you bank : (y/n)");
                check = NullHandler.HandleInput<string>();
                if (check == AppConstants.Yes)
                {
                    while (check == AppConstants.Yes)
                    {
                        Console.WriteLine("Enter Currency: ");
                        string currName = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Excahnge Rate: ");
                        decimal exchangeRate = NullHandler.HandleInput<decimal>();
                        currencies.Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
                        Console.WriteLine("\vWant to continue adding accepted Currencies and their exchnage rates for you bank : (y/n)");
                        check = NullHandler.HandleInput<string>();
                    }
                    Console.WriteLine("\vSelect one of the currrencies as default accepted currency for your bank: ");
                    currencies.ForEach(s => Console.WriteLine("Currency Name: {0}", s.Name));
                    bankModel.Currency.Name = NullHandler.HandleInput<string>();
                }
                else
                {
                    currencies.Add(new Currency { Name = AppConstants.BankCurrency, ExchangeRate = 0});
                }
                bankService.AddBank(listOfBanks, bankModel, currencies);
                Console.WriteLine("\vNew Bank set up successful");
            }
            else
            {
                bankModel.RTGSToSameBank = AppConstants.SameBankRTGS;
                bankModel.IMPSToSameBank = AppConstants.SameBankIMPS;
                bankModel.RTGSToOtherBanks = AppConstants.OtherBankRTGS;
                bankModel.IMPSToOtherBanks = AppConstants.OtherBankIMPS;

                List<Currency> currencies = new List<Currency>();
                Console.WriteLine("\vWant to add accepted Currencies and their exchnage rates for you bank : (y/n)");
                check = NullHandler.HandleInput<string>();
                if (check == AppConstants.Yes)
                {
                    while (check == AppConstants.Yes)
                    {
                        Console.WriteLine("Enter Currency: ");
                        string currName = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Excahnge Rate: ");
                        decimal exchangeRate = NullHandler.HandleInput<decimal>();
                        currencies.Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
                        Console.WriteLine("\vWant to continue adding accepted Currencies and their exchnage rates for you bank : (y/n)");
                        check = NullHandler.HandleInput<string>();
                    }
                    Console.WriteLine("\vSelect one of the currrencies as default accepted currency for your bank: ");
                    currencies.ForEach(s => Console.WriteLine("Currency Name: {0}", s.Name));
                    bankModel.Currency.Name = NullHandler.HandleInput<string>();
                }
                else
                {
                    currencies.Add(new Currency { Name = AppConstants.BankCurrency, ExchangeRate = 0 });
                }
                bankService.AddBank(listOfBanks, bankModel, currencies);
                Console.WriteLine("\vNew Bank set up successful");
            }
        }

        public void CreateUser()
        {
            int iter = 0;
            string bankId, userName, check;
            UserService userService = new UserService();

            Console.WriteLine("\vSelect in which bank you want create bank staff or User Account: ");
            listOfBanks.Banks.ForEach(s => Console.WriteLine($"{iter++} - {s.Id}"));
            bankId = listOfBanks.Banks[NullHandler.HandleInput<int>()].Id;
            do
            {
                if (listOfBanks.Banks.FindIndex(s => s.Id == bankId) != -1)
                {
                    int selectedOption = (int)UserRole.Staff;
                    Console.WriteLine("\vSelect User role");
                    foreach (var role in Enum.GetNames(typeof(UserRole)))
                    {
                        Console.WriteLine("{0}. {1}", selectedOption++, role);
                    }
                    int selectedRolenum = NullHandler.HandleInput<int>();

                    switch (selectedRolenum)
                    {
                        case 1:
                            string email, address, password;
                            long phoneNumber;

                            Console.WriteLine("\vEnter User Name: ");
                            userName = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Password: ");
                            password = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Email Address: ");
                            email = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Phone Number: ");
                            phoneNumber = NullHandler.HandleInput<long>();
                            Console.WriteLine("Enter Residential Address: ");
                            address = NullHandler.HandleInput<string>();
                            selectedOption = (int)StaffDesignation.Manager;
                            Console.WriteLine("Select Staff position");
                            foreach (var position in Enum.GetNames(typeof(StaffDesignation)))
                            {
                                Console.WriteLine("{0}. {1}", selectedOption++, position);
                            }
                            selectedRolenum = NullHandler.HandleInput<int>();

                            userService.CreateUser(userName, password, selectedOption, email, address, phoneNumber, bankId, listOfBanks);

                            Console.WriteLine($"Account Id = {IdGenerator.CreateAccountId(userName)}");
                            break;
                        case 2:
                            Console.WriteLine("\vEnter User Name: ");
                            userName = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Password: ");
                            password = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Email Address: ");
                            email = NullHandler.HandleInput<string>();
                            Console.WriteLine("Enter Phone Number: ");
                            phoneNumber = NullHandler.HandleInput<long>();
                            Console.WriteLine("Enter Residential Address: ");
                            address = NullHandler.HandleInput<string>();
                            userService.CreateUser(userName, password, email, address, phoneNumber, bankId, listOfBanks);

                            Console.WriteLine(string.Format("Account Id = {0}\n", IdGenerator.CreateAccountId(userName)));
                            break;
                        default:
                            Console.WriteLine("Select between given options");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entered bank Id doesnt exit");
                    break;
                }
                Console.WriteLine("\vWant to continue creating Users ( y/n ):");
                check = NullHandler.HandleInput<string>();
            } while (check != AppConstants.No);
        }

        public void UserLogin()
        {
            string bankId;
            do
            {
                Console.WriteLine("\vEnter Bank Id: ");
                bankId = NullHandler.HandleInput<string>();
                if (listOfBanks.Banks.FindIndex(s => s.Id == bankId) != -1)
                {
                    Console.WriteLine("\vSelect an Option to Log In");
                    Console.WriteLine("1. Staff\n2. Customer");
                    int seletedOption = NullHandler.HandleInput<int>();
                    switch (seletedOption)
                    {
                        case 1:
                            StaffLogIn(bankId);
                            Console.WriteLine("\vwant to logIn into an account( y/n ):");
                            break;
                        case 2:
                            CustomerLogIn(bankId);
                            Console.WriteLine("\vwant to logIn into an account( y/n ):");
                            break;
                        default:
                            Console.WriteLine("\vSelect between the given options only");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\vEntered bank Id doesnt exit");
                    break;
                }
            } while (Convert.ToBoolean(string.Compare(NullHandler.HandleInput<string>(), AppConstants.No)));
        }

        public void StaffLogIn(string bankId)
        {
            string accId;
            UserService userService = new UserService();
            Console.WriteLine("\vEnter Account Id and password to login");
            Console.WriteLine("Account Id: ");
            accId = NullHandler.HandleInput<string>();
            Console.WriteLine("Password: ");
            if (userService.ValidateStaff(accId, NullHandler.HandleInput<string>(), bankId, listOfBanks))
            {
                Console.WriteLine("\vLog In successful");
                //when logged user is normal user
                StaffActions(bankId);
            }
            else
            {
                Console.WriteLine("\vWrong credentials please try again latter");
            }
        }

        public void CustomerLogIn(string bankId)
        {
            string accId;
            UserService userService = new UserService();
            Console.WriteLine("\vEnter Account Id and password to login");
            Console.WriteLine("Account Id: ");
            accId = NullHandler.HandleInput<string>();
            Console.WriteLine("Password: ");
            if (userService.ValidateUser(accId, NullHandler.HandleInput<string>(), bankId, listOfBanks))
            {
                Console.WriteLine("\vLog In successful");
                //when logged user is normal user
                    UserActions(bankId, accId);
            }
            else
            {
                Console.WriteLine("\vWrong credentials please try again latter");
            }
        }

        public void StaffActions(string bankId)
        {
            BankSerivce bankService = new BankSerivce();
            UserService userService = new UserService();
            TransactionService transactionService = new TransactionService();
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("\vwant to continue with this account Managements ( y/n ): ");
            while (NullHandler.HandleInput<string>() == AppConstants.Yes)
            {
                Console.WriteLine("\vSelect one of the below options: \n1. Create user account. \n2. Update User. \n3. Delete User" +
                    "\n4. Add new accepted currency and exchange rates. \n5. Update service rates for same bank\n6. Update service rates for other banks" + 
                    "7. View an user transaction history. \n8. Revert transasction. \n");

                switch (NullHandler.HandleInput<int>())
                {
                    case 1:
                        string userName,email, address, password;
                        long phoneNumber;
                        Console.WriteLine("\vEnter User Name: ");
                        userName = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = NullHandler.HandleInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = NullHandler.HandleInput<string>();
                        Console.WriteLine(userService.CreateUser(userName, password, email, address, phoneNumber, bankId, listOfBanks));
                        break;
                    case 2:
                        string accId;
                        Console.WriteLine("\vEnter Account Id: ");
                        accId = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter User Name: ");
                        userName = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = NullHandler.HandleInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = NullHandler.HandleInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = NullHandler.HandleInput<string>();
                        Console.WriteLine(userService.UpdateUser( accId, userName, password, email, address, phoneNumber, bankId, listOfBanks));
                        break;

                    case 3:
                        Console.WriteLine("\vEnter AccountId to delete user account");
                        Console.WriteLine(userService.DeleteUser(NullHandler.HandleInput<string>(), bankId, listOfBanks));
                        break;

                    case 4:
                        Console.WriteLine("\vEnter new currency name and its exchange rate:");
                        Console.WriteLine(bankService.AddCurrAndExchangeRate(NullHandler.HandleInput<string>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;

                    case 5:
                        Console.WriteLine("\vEnter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForSameBank(NullHandler.HandleInput<decimal>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;
                    case 6:
                        Console.WriteLine("\vEnter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForOtherBanks(NullHandler.HandleInput<decimal>(), NullHandler.HandleInput<decimal>(), bankId, listOfBanks));
                        break;
                    case 7:
                        Console.WriteLine("\vEnter Account Id to view transaction: ");
                        transactions = transactionService.GetAlltransactions(NullHandler.HandleInput<string>(), bankId, listOfBanks);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    case 8:
                        Console.WriteLine("\vEnter Account Id and Transaction Id to revert that transasction");
                        Console.WriteLine(transactionService.RevertTransaction(NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), bankId, listOfBanks));
                        break;

                    default:
                        Console.WriteLine("\vSelect options from 1 to 6 only");
                        break;
                }
                Console.WriteLine("\vWant to continue with account management ( y/n ):");
            }
        }

        public void UserActions(string bankId, string accId)
        {
            AccountService accountService = new AccountService();
            TransactionService transaction = new TransactionService();
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("\vWant to continue with this account Managements ( y/n ): ");
            while (NullHandler.HandleInput<string>() == AppConstants.Yes)
            {
                Console.WriteLine("\vSelect one of the below options: \n1. Deposit amount. \n2. Withdraw amount. \n3. Transfer Funds" +
                    "\n4. View transaction history\n5. View Balance");

                switch (NullHandler.HandleInput<int>())
                {
                    case 1:
                        Console.WriteLine("\vEnter in which currency you want deposit, amount:\n");
                        accountService.Deposit(NullHandler.HandleInput<string>(), NullHandler.HandleInput<int>(), accId, bankId, listOfBanks);
                        Console.WriteLine("Successful");
                        break;

                    case 2:
                        Console.WriteLine("\vEnter amount to be withdrawn: \n");
                        Console.WriteLine(accountService.WithDraw(accId, NullHandler.HandleInput<int>(), bankId, listOfBanks));
                        break;

                    case 3:
                        Console.WriteLine("\vEnter bank Id, account Id and amount to transfer your savings");
                        Console.WriteLine(accountService.TransferFunds(accId, NullHandler.HandleInput<string>(), NullHandler.HandleInput<string>(), NullHandler.HandleInput<int>(), bankId, listOfBanks));
                        break;

                    case 4:
                        transactions = transaction.GetAlltransactions(accId, bankId, listOfBanks);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    case 5:
                        Console.WriteLine(accountService.ViewBalance(accId, bankId, listOfBanks));
                        break;

                    default:
                        Console.WriteLine("select options from 1  to 4 only\n");
                        break;
                }
            }
        }
    }
}
