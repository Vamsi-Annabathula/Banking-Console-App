using BankTransfer.Helpers;
using BankTransfer.Models;
using BankTransfer.Services;
using BankTransfer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BankTransfer.UI
{
    public class App
    {
        public BanksList ListOfBanks;

        public App()
        {
            ListOfBanks = new BanksList();
        }
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
                    Console.WriteLine("\vWant to continue creating Users ( y/n ):");
                } while (InputHandler.GetInput<string>() != AppConstants.No);

                Console.WriteLine("Want to Set Up other new Bank (y/n):");
            } while (InputHandler.GetInput<string>() != AppConstants.No);
            Console.WriteLine("***************************User LogIn********************************");
            Console.WriteLine("Want to LogIn: (y/n)");
            while (InputHandler.GetInput<string>() != AppConstants.No)
            {
                UserLogin();
            } 
            Console.WriteLine(JsonSerializer.Serialize(ListOfBanks, typeof(BanksList)));
        }   
        public void SetUpBank()
        {
            string check;
            Bank bankModel = new Bank();
            BankSerivce bankService = new BankSerivce(ListOfBanks);

            Console.WriteLine("\vEnter Bank Name");
            bankModel.Name = InputHandler.GetInput<string>();
            bankModel.Id = IdGenerator.CreateAccountId(bankModel.Name);
            Console.WriteLine("Generated id: {0}", bankModel.Id);
     
            Console.WriteLine("\vWant to use default service charge rates (y/n): ");
            if(InputHandler.GetInput<string>() == AppConstants.No)
            {
                Console.WriteLine("Enter service rates for same bank");
                Console.Write("RTGS: ");
                bankModel.RTGSToSameBank = InputHandler.GetInput<decimal>();
                Console.Write("IMPS: ");
                bankModel.IMPSToSameBank = InputHandler.GetInput<decimal>();
                Console.WriteLine("Enter serive rate for other banks");
                Console.Write("RTGS: ");
                bankModel.RTGSToOtherBanks = InputHandler.GetInput<decimal>();
                Console.Write("IMPS: ");
                bankModel.IMPSToOtherBanks = InputHandler.GetInput<decimal>();
                Console.WriteLine("\vEnter default accepted currency for your bank: ");
                bankModel.Currency.Name = InputHandler.GetInput<string>();
                bankModel.Currency.ExchangeRate = 0;
                List<Currency> currencies = new List<Currency>();
                Console.WriteLine("\vWant to add accepted Currencies and their exchnage rates for you bank : (y/n)");
                check = InputHandler.GetInput<string>();
                while (check == AppConstants.Yes)
                {
                    Console.WriteLine("Enter Currency: ");
                    string currName = InputHandler.GetInput<string>();
                    Console.WriteLine("Enter Excahnge Rate: ");
                    decimal exchangeRate = InputHandler.GetInput<decimal>();
                    currencies.Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
                    Console.WriteLine("\vWant to continue adding accepted Currencies and their exchnage rates for you bank : (y/n)");
                    check = InputHandler.GetInput<string>();
                }
                bankService.AddBank(ListOfBanks, bankModel, currencies);
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
                check = InputHandler.GetInput<string>();
                if (check == AppConstants.Yes)
                {
                    while (check == AppConstants.Yes)
                    {
                        Console.WriteLine("Enter Currency: ");
                        string currName = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Excahnge Rate: ");
                        decimal exchangeRate = InputHandler.GetInput<decimal>();
                        currencies.Add(new Currency { Name = currName, ExchangeRate = exchangeRate });
                        Console.WriteLine("\vWant to continue adding accepted Currencies and their exchnage rates for you bank : (y/n)");
                        check = InputHandler.GetInput<string>();
                    }
                    Console.WriteLine("\vSelect one of the currrencies as default accepted currency for your bank: ");
                    currencies.ForEach(s => Console.WriteLine("Currency Name: {0}", s.Name));
                    bankModel.Currency.Name = InputHandler.GetInput<string>();
                }
                else
                {
                    currencies.Add(new Currency { Name = AppConstants.BankCurrency, ExchangeRate = 0 });
                }
                bankService.AddBank(ListOfBanks, bankModel, currencies);
                Console.WriteLine("\vNew Bank set up successful");
            }
        }

        public void CreateUser()
        {
            int iter = 1;
            string bankId, userName;
            UserService userService = new UserService(ListOfBanks);

            Console.WriteLine("\vSelect in which bank you want create bank staff or User Account: ");
            ListOfBanks.Banks.ForEach(s => Console.WriteLine($"{iter++} - {s.Id}"));
            List<int> inputValidateList = new List<int>();
            inputValidateList.AddRange(Enumerable.Range(1, iter - 1));
            bankId = ListOfBanks.Banks[InputHandler.GetInput<int>(inputValidateList)-1].Id;
            if (ListOfBanks.Banks.FindIndex(s => s.Id == bankId) != -1)
            {
                int selectedOption = (int)UserRole.Staff;
                Console.WriteLine("\vSelect User role");
                foreach (var role in Enum.GetNames(typeof(UserRole)))
                {
                    Console.WriteLine("{0}. {1}", selectedOption++, role);
                }

                inputValidateList = new List<int>{ 1, 2 };
                int selectedRolenum = InputHandler.GetInput<int>(inputValidateList);

                switch (selectedRolenum)
                {
                    case 1:
                        string email, address, password;
                        long phoneNumber;

                        Console.WriteLine("\vEnter User Name: ");
                        userName = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = InputHandler.GetInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = InputHandler.GetInput<string>();
                        selectedOption = (int)StaffDesignation.Manager;
                        Console.WriteLine("Select Staff position");
                        foreach (var position in Enum.GetNames(typeof(StaffDesignation)))
                        {
                            Console.WriteLine("{0}. {1}", selectedOption++, position);
                        }
                        inputValidateList = new List<int> { 1, 2 };
                        selectedRolenum = InputHandler.GetInput<int>(inputValidateList);

                        userService.CreateUser(userName, password, selectedOption, email, address, phoneNumber, bankId);

                        Console.WriteLine($"Account Id = {IdGenerator.CreateAccountId(userName)}");
                        break;
                    case 2:
                        Console.WriteLine("\vEnter User Name: ");
                        userName = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = InputHandler.GetInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = InputHandler.GetInput<string>();
                        userService.CreateUser(userName, password, email, address, phoneNumber, bankId);

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
                }
        }

        public void UserLogin()
        {
            string bankId;
            do
            {
                Console.WriteLine("\vEnter Bank Id: ");
                bankId = InputHandler.GetInput<string>();
                if (ListOfBanks.Banks.FindIndex(s => s.Id == bankId) != -1)
                {
                    Console.WriteLine("\vSelect an Option to Log In");
                    Console.WriteLine("1. Staff\n2. Customer");

                    List<int> inputValidateList = new List<int> { 1, 2 };
                    int seletedOption = InputHandler.GetInput<int>(inputValidateList);
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
                Console.WriteLine("\vWant to continue (y/n): ");
            } while (Convert.ToBoolean(string.Compare(InputHandler.GetInput<string>(), AppConstants.No)));
        }

        public void StaffLogIn(string bankId)
        {
            string accId;
            UserService userService = new UserService(ListOfBanks);
            Console.WriteLine("\vEnter Account Id and password to login");
            Console.WriteLine("Account Id: ");
            accId = InputHandler.GetInput<string>();
            Console.WriteLine("Password: ");
            if (userService.ValidateStaff(accId, InputHandler.GetInput<string>(), bankId))
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
            UserService userService = new UserService(ListOfBanks);
            Console.WriteLine("\vEnter Account Id and password to login");
            Console.WriteLine("Account Id: ");
            accId = InputHandler.GetInput<string>();
            Console.WriteLine("Password: ");
            if (userService.ValidateUser(accId, InputHandler.GetInput<string>(), bankId))
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
            BankSerivce bankService = new BankSerivce(ListOfBanks);
            UserService userService = new UserService(ListOfBanks);
            TransactionService transactionService = new TransactionService(ListOfBanks);
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("\vwant to continue with this account Managements ( y/n ): ");
            while (InputHandler.GetInput<string>() == AppConstants.Yes)
            {
                Console.WriteLine("\vSelect one of the below options: \n1. Create user account. \n2. Update User. \n3. Delete User" +
                    "\n4. Add new accepted currency and exchange rates. \n5. Update service rates for same bank\n6. Update service rates for other banks" + 
                    "7. View an user transaction history. \n8. Revert transasction. \n");


                List<int> inputValidateList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
                int seletedOption = InputHandler.GetInput<int>(inputValidateList);

                switch (seletedOption)
                {
                    case 1:
                        string userName,email, address, password;
                        long phoneNumber;
                        Console.WriteLine("\vEnter User Name: ");
                        userName = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = InputHandler.GetInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = InputHandler.GetInput<string>();
                        Console.WriteLine(userService.CreateUser(userName, password, email, address, phoneNumber, bankId));
                        break;
                    case 2:
                        string accId;
                        Console.WriteLine("\vEnter Account Id: ");
                        accId = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter User Name: ");
                        userName = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Password: ");
                        password = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Email Address: ");
                        email = InputHandler.GetInput<string>();
                        Console.WriteLine("Enter Phone Number: ");
                        phoneNumber = InputHandler.GetInput<long>();
                        Console.WriteLine("Enter Residential Address: ");
                        address = InputHandler.GetInput<string>();
                        Console.WriteLine(userService.UpdateUser( accId, userName, password, email, address, phoneNumber, bankId));
                        break;

                    case 3:
                        Console.WriteLine("\vEnter AccountId to delete user account");
                        Console.WriteLine(userService.DeleteUser(InputHandler.GetInput<string>(), bankId));
                        break;

                    case 4:
                        Console.WriteLine("\vEnter new currency name and its exchange rate:");
                        Console.WriteLine(bankService.AddCurrAndExchangeRate(InputHandler.GetInput<string>(), InputHandler.GetInput<decimal>(), bankId));
                        break;

                    case 5:
                        Console.WriteLine("\vEnter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForSameBank(InputHandler.GetInput<decimal>(), InputHandler.GetInput<decimal>(), bankId));
                        break;
                    case 6:
                        Console.WriteLine("\vEnter RTGS, IMPS: ");
                        Console.WriteLine(bankService.UpdateServiceChargeForOtherBanks(InputHandler.GetInput<decimal>(), InputHandler.GetInput<decimal>(), bankId));
                        break;
                    case 7:
                        Console.WriteLine("\vEnter Account Id to view transaction: ");
                        transactions = transactionService.GetAlltransactions(InputHandler.GetInput<string>(), bankId);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    case 8:
                        Console.WriteLine("\vEnter Account Id and Transaction Id to revert that transasction");
                        Console.WriteLine(transactionService.RevertTransaction(InputHandler.GetInput<string>(), InputHandler.GetInput<string>(), bankId));
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
            AccountService accountService = new AccountService(ListOfBanks);
            TransactionService transaction = new TransactionService(ListOfBanks);
            List<Transaction> transactions = new List<Transaction>();

            Console.WriteLine("\vWant to continue with this account Managements ( y/n ): ");
            while (InputHandler.GetInput<string>() == AppConstants.Yes)
            {
                Console.WriteLine("\vSelect one of the below options: \n1. Deposit amount. \n2. Withdraw amount. \n3. Transfer Funds" +
                    "\n4. View transaction history\n5. View Balance");

                List<int> inputValidateList = new List<int> { 1, 2, 3, 4, 5 };
                int seletedOption = InputHandler.GetInput<int>(inputValidateList);

                switch (seletedOption)
                {
                    case 1:
                        Console.WriteLine("\vEnter in which currency you want deposit, amount:\n");
                        accountService.Deposit(InputHandler.GetInput<string>(), InputHandler.GetInput<int>(), accId, bankId);
                        Console.WriteLine("Successful");
                        break;

                    case 2:
                        Console.WriteLine("\vEnter amount to be withdrawn: \n");
                        Console.WriteLine(accountService.WithDraw(accId, InputHandler.GetInput<int>(), bankId));
                        break;

                    case 3:
                        Console.WriteLine("\vEnter bank Id, account Id and amount to transfer your savings");
                        Console.WriteLine(accountService.TransferFunds(accId, InputHandler.GetInput<string>(), InputHandler.GetInput<string>(), InputHandler.GetInput<int>(), bankId));
                        break;

                    case 4:
                        transactions = transaction.GetAlltransactions(accId, bankId);
                        transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Description}"));
                        break;

                    case 5:
                        Console.WriteLine(accountService.ViewBalance(accId, bankId));
                        break;

                    default:
                        Console.WriteLine("select options from 1  to 4 only\n");
                        break;
                }
            }
        }
    }
}
