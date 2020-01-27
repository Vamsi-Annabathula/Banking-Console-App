using System;
using System.Collections.Generic;
using BankTransfer;
using BankTransfer.Helpers;
using BankTransfer.Services;

namespace BankTransfer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            App app = new App();
            app.StartApp();
        }
    }
    public class App
    {
        BankSerivce bankService = new BankSerivce();
        Models.Bank bankModel = new Models.Bank();
        Models.BanksManagement listOfBanks = new Models.BanksManagement();
        BanksManagement banksService = new BanksManagement();
        AccountService account = new AccountService();
        List<Models.Transaction> transactions = new List<Models.Transaction>();

        public enum Options
        {
            Use_Default_Bank = 1,
            Set_Up_Bank,
            Create_User,
            User_LogIn,
            Exit
        }
        
        public enum Role
        {
            Staff = 1,
            User
        }
        public void StartApp()
        {
            string accId, bankId, userName;
            int input;
            do
            {
                int i = (int)Options.Use_Default_Bank;
                Console.WriteLine("Select any one Option");
                foreach (var e in Enum.GetNames(typeof(Options)))
                {
                    Console.WriteLine("{0}. {1}", i++, e);
                }
                input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        bankModel.Name = DefaultValue.BankName;
                        bankModel.Currency = DefaultValue.BankCurrency;
                        bankModel.SameBankRTGS = DefaultValue.SameBankRTGS;
                        bankModel.SameBankIMPS = DefaultValue.SameBankIMPS;
                        bankModel.OtherBankRTGS = DefaultValue.OtherBankRTGS;
                        bankModel.OtherBankIMPS = DefaultValue.OtherBankIMPS;
                        bankModel.Id = IdGenerator.CreateAccountId(bankModel.Name);
                        banksService.AddBank(listOfBanks, bankModel.Id, bankModel.Name, bankModel.Currency, bankModel.SameBankIMPS, bankModel.SameBankRTGS, bankModel.OtherBankIMPS, bankModel.OtherBankRTGS);
                        bankService.AddCurrAndExchangeRate(bankModel.Name, 0, IdGenerator.CreateAccountId(bankModel.Name), listOfBanks);
                        Console.WriteLine("Default Bank set successfull");
                        break;

                    case 2:
                        string check;
                        Console.WriteLine("Enter Bank Name");
                        bankModel.Name = Console.ReadLine();
                        bankModel.Id = IdGenerator.CreateAccountId(bankModel.Name);
                        Console.WriteLine("Generated id: {0}", bankModel.Id);

                        Console.WriteLine("Enter Bank service Rates");
                        Console.WriteLine("Enter service rates for same bank");
                        Console.Write("RTGS: ");
                        bankModel.SameBankRTGS = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("IMPS: ");
                        bankModel.SameBankIMPS = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter serive rate for other banks");
                        Console.Write("RTGS: ");
                        bankModel.OtherBankRTGS = Convert.ToDecimal(Console.ReadLine());
                        Console.Write("IMPS: ");
                        bankModel.OtherBankIMPS = Convert.ToDecimal(Console.ReadLine());

                        banksService.AddBank(listOfBanks, bankModel.Id, bankModel.Name, bankModel.Currency, bankModel.SameBankIMPS, bankModel.SameBankRTGS, bankModel.OtherBankIMPS, bankModel.OtherBankRTGS);

                        Console.WriteLine("Enter y/n to add Accepted currencies: (y/n)");
                        check = Console.ReadLine();
                        if (check != DefaultValue.No)
                        {
                            while (check == DefaultValue.Yes)
                            {
                                Console.WriteLine("Enter Bank's accepted currencies");
                                Console.WriteLine("Enter Currency: ");
                                string currName = Console.ReadLine();
                                Console.WriteLine("Enter its Excahnge Rate: ");
                                int exchangeRate = Convert.ToInt32(Console.ReadLine());
                                bankService.AddCurrAndExchangeRate(currName, exchangeRate, bankModel.Id, listOfBanks);
                            }
                        }
                        else
                        {
                            bankService.AddCurrAndExchangeRate(DefaultValue.BankCurrency, 0, bankModel.Id, listOfBanks);
                        }

                        Console.WriteLine("Select one of the currrencies for your bank");

                        listOfBanks.Banks.Find(s => s.Id == bankModel.Id).AcceptedCurrencies.ForEach(s => Console.WriteLine("Currency Name: {0}", s.Name));
                        bankModel.Currency = Console.ReadLine();
                        Console.WriteLine("New Bank set up successful");
                        break;

                    case 3:

                        Console.WriteLine("\nPlease select y/n to continue creating Users or to stop creating Users ( y/n ):");
                        string Check = Console.ReadLine();
                        if(Check != DefaultValue.No)
                        {
                            Console.WriteLine("Select in which bank you want create bank staff: ");
                            listOfBanks.Banks.ForEach(s => Console.WriteLine(s.Id));
                        }

                        bankId = Console.ReadLine();
                        while (Check != DefaultValue.No)
                        {
                            i = (int)Role.Staff;
                            Console.WriteLine("Select User role");
                            foreach (var e in Enum.GetNames(typeof(Role)))
                            {
                                Console.WriteLine("{0}. {1}", i++, e);
                            }
                            int selectedRolenum = Convert.ToInt32(Console.ReadLine());
                            
                            switch (selectedRolenum)
                            {
                                case 1:
                                    Console.WriteLine("Enter User Name, Password");
                                    userName = Console.ReadLine();
                                    var _ = (Role)selectedRolenum;

                                    bankService.CreateUser(userName, Console.ReadLine(), _.ToString(), bankId, listOfBanks);

                                    Console.WriteLine($"Account Id = {IdGenerator.CreateAccountId(userName)}");
                                    break;
                                case 2:
                                    Console.WriteLine("Enter User Name and Password");
                                    userName = Console.ReadLine();
                                    _ = (Role)selectedRolenum;

                                    bankService.CreateUser(userName, Console.ReadLine(), _.ToString(), bankId, listOfBanks);

                                    Console.WriteLine(string.Format("Account Id = {0}\n", IdGenerator.CreateAccountId(userName)));
                                    break;
                                default:
                                    Console.WriteLine("Select between given options");
                                    break;
                            }
                            Console.WriteLine("Please select y/n to continue creating Users or to stop creating Users ( y/n ):");
                            Check = Console.ReadLine();
                        }
                        break;
                    case 4:
                        Console.WriteLine("\nNow enter yes to Log In or no for ending this program ( y/n ): ");
                       
                        while (Convert.ToBoolean(string.Compare(Console.ReadLine(), DefaultValue.No)))
                        {
                            Console.WriteLine("Enter Bank Id: ");
                            bankId = Console.ReadLine();
                            
                            Console.WriteLine("Enter Account Id and password to login");
                            accId = Console.ReadLine();
                            string role = listOfBanks.Banks
                                  .Find(s => s.Id == bankId)
                                  .Accounts
                                  .Find(e => e.Id == accId)
                                  .User.Role;
                            if (bankService.ValidateUser(accId, Console.ReadLine(), bankId, listOfBanks))
                            {
                                Console.WriteLine("Log In successful");

                                //When logged user is Bank staff
                                if (role == "Staff")
                                {
                                    Console.WriteLine("Enter yes to continue with this account Managements or no to exit/ logout from account ( y/n ):");

                                    while (Console.ReadLine() == DefaultValue.Yes)
                                    {
                                        Console.WriteLine("Select one of the below options: \n1. Create User. \n2. Update User. \n3. Delete User" +
                                            "\n4. Add new Currency and exchange Rates. \n5. View an user transaction history. \n6. Revert transasction. \n");

                                        switch (Convert.ToInt32(Console.ReadLine()))
                                        {
                                            case 1:
                                                i = (int)Role.Staff;
                                                Console.WriteLine("Select User role");
                                                foreach (var e in Enum.GetNames(typeof(Role)))
                                                {
                                                    Console.WriteLine("{0}. {1}", i++, e);
                                                }
                                                int selectedRolenum = Convert.ToInt32(Console.ReadLine());
                                                switch (selectedRolenum)
                                                {
                                                    case 1:
                                                        Console.WriteLine("Enter User name, Password to create");
                                                        var _ = (Role)selectedRolenum;
                                                        bankService.CreateUser(Console.ReadLine(), Console.ReadLine(), _.ToString(), bankId, listOfBanks);
                                                        break;
                                                    case 2:
                                                        Console.WriteLine("Enter User name, Password to create");
                                                        _ = (Role)selectedRolenum;
                                                        bankService.CreateUser(Console.ReadLine(), Console.ReadLine(), _.ToString(), bankId, listOfBanks);
                                                        break;
                                                }                                                   
                                                
                                                break;

                                            case 2:
                                                Console.WriteLine("Enter Account Id, User Name, Password, Role to Update");
                                                bankService.UpdateUser(Console.ReadLine(), Console.ReadLine(), Console.ReadLine(), Console.ReadLine(), bankId, listOfBanks);
                                                break;

                                            case 3:
                                                Console.WriteLine("Enter AccountId to delete User Account");
                                                bankService.DeleteUser(Console.ReadLine(), bankId, listOfBanks);
                                                break;

                                            case 4:
                                                Console.WriteLine("Enter Currency name and its exchange rate:");
                                                bankService.AddCurrAndExchangeRate(Console.ReadLine(), Convert.ToDecimal(Console.ReadLine()), bankId, listOfBanks);
                                                break;

                                            case 5:
                                                Console.WriteLine("Enter Account Id to view transaction Id:\n");
                                                transactions = account.GetAlltransactionsOfUser(Console.ReadLine(), bankId, listOfBanks);
                                                transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Desc}"));
                                                break;

                                            case 6:
                                                Console.WriteLine("Enter Account Id and Transaction Id to revert that transasction");
                                                Console.WriteLine(account.RevertTransaction(Console.ReadLine(), Console.ReadLine(), bankId, listOfBanks));
                                                break;

                                            default:
                                                Console.WriteLine("Select options from 1 to 6 only");
                                                break;
                                        }
                                        Console.WriteLine("Enter yes to continue with this account Managements or no to exit/logout from account ( y/n ):");
                                    }
                                }
                            }
                            //when logged user is normal user
                            else if (role == "User")
                            {
                                Console.WriteLine("Enter yes to continue with this account Managements or no to exit/ logout from account ( y/n ):");

                                while (Console.ReadLine() == DefaultValue.Yes)
                                {
                                    Console.WriteLine("Select one of the below options: \n1. Deposit amount. \n2. Withdraw amount. \n3. Transfer Funds" +
                                        "\n4. View transaction history \n");

                                    switch (Convert.ToInt32(Console.ReadLine()))
                                    {
                                        case 1:
                                            Console.WriteLine("Enter in which currency you want deposit, amount:\n");
                                            account.Deposit(Console.ReadLine(), Convert.ToInt32(Console.ReadLine()), accId, bankId, listOfBanks);
                                            Console.WriteLine("Successful");
                                            break;

                                        case 2:
                                            Console.WriteLine("Enter amount to be withdrawn: \n");
                                            Console.WriteLine(account.WithDraw(accId, Convert.ToInt32(Console.ReadLine()), bankId, listOfBanks));
                                            break;

                                        case 3:
                                            Console.WriteLine("Enter bank Id, account Id and amount to transfer your savings");
                                            Console.WriteLine(account.TransferFunds(accId, Console.ReadLine(), Console.ReadLine(), Convert.ToInt32(Console.ReadLine()), bankId, listOfBanks));
                                            break;

                                        case 4:
                                            transactions = account.GetAlltransactionsOfUser(accId, bankId, listOfBanks);
                                            transactions.ForEach(e => Console.WriteLine($"Transaction Id: {e.Id} and Transaction is :{e.Desc}"));
                                            break;

                                        default:
                                            Console.WriteLine("select options from 1  to 4 only\n");
                                            break;
                                    }
                                }

                            }

                            else
                            {
                                Console.WriteLine("Wrong credentials please try again latter");
                            }
                            Console.WriteLine("Now Enter yes to Log In or exit for ending this program ( y/n ):");
                        }
                        break;
                }
                Console.WriteLine("Want to continue? (y/n): ");
                } while (Console.ReadLine() != "n" && input != 5) ;
                Console.WriteLine("Thank you for using our services");

            }
    }
}
        
    

        
    

