using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Menu
    {
        Bank bank = new Bank();
        private string userInput;
        private string readPath = "bankdata.txt";

        internal void RunBank()
        {
            bank = FileHandler.ReadData(readPath);
            PrintInfoStart();
            PrintMenu();
            while (true)
            {
                Console.WriteLine();
                Console.Write("Enter menu option: ");
                CheckUserInput();
                Console.WriteLine();
            }
        }

        private void PrintInfoStart()
        {
            var totalBalance = (from s in bank.Accounts
                              select s.Balance).Sum();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"**  \\ WELCOME TO MAX BANKAPP  //**");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Läser in " + readPath + "...");
            Console.Write("Number of customers: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(+FileHandler.CountOfCustomers);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Number of accounts: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(+FileHandler.CountOfAccounts);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Total balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(totalBalance + " SEK");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void PrintInfoEnd()
        {
            var totalSaldo = (from s in bank.Accounts
                select s.Balance).Sum();

            Console.WriteLine("Sparar till " + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt");
            Console.WriteLine("Number of customers: " + FileHandler.CountOfCustomers);
            Console.WriteLine("Number of accounts: " + FileHandler.CountOfAccounts);
            Console.WriteLine("Total balance: " + totalSaldo);
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" ****\\  MENU  //****");
            Console.WriteLine();
            Console.WriteLine("0) -Save and Exit-");
            Console.WriteLine("1) -Customer Search-");
            Console.WriteLine("2) -Show Customer Info-");
            Console.WriteLine("3) -Create Customer-");
            Console.WriteLine("4) -Delete Customer-");
            Console.WriteLine("5) -Create Account-"); 
            Console.WriteLine("6) -Delete Account-");
            Console.WriteLine("7) -Cash Deposit-");
            Console.WriteLine("8) -Cash Withdrawal-");
            Console.WriteLine("9) -Cash Transfer-");
            Console.WriteLine("10) -Account Info-");
            Console.WriteLine("11) -Apply for credit-");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"****\\  ADMIN  //****");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("12) -Add/Change account savings rate-");
            Console.WriteLine("13) -Change credit debt rate-");
            Console.WriteLine("14) -Add today's interest-");
            Console.WriteLine("15) -Charge credit customers- ");
            Console.WriteLine("16) -Clear Console-");
            Console.ResetColor();
        }

        
        private void CheckUserInput()
        {
            userInput = Console.ReadLine();

            if (userInput == "0")
            {
                FileHandler.SaveTransactionData(bank,"Transactions" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt");
                FileHandler.SaveData(bank, DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt"); 
                PrintInfoEnd();
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (userInput == "1")
            {
                Console.WriteLine("**Customer Search**");
                Console.WriteLine();
                Console.WriteLine("Search for either customer name or customer location: ");
                string userInput = Console.ReadLine().ToLower();
                bank.SearchCustomer(userInput);
            }
            if (userInput == "2")
            {
                Console.WriteLine("**Customer Info**");
                Console.WriteLine();
                Console.WriteLine("Enter either Customer ID or Account ID: ");
                int userInputId = Exceptions.InputInt(userInput);
                bank.CustomerInfo(userInputId);
            }
            if (userInput == "3")
            {
                Console.WriteLine("**New Customer**");
                Console.WriteLine();
                Console.WriteLine("Corporate-ID: ");
                string corporateId = Exceptions.CheckIfEmpty(userInput);
                Console.WriteLine("Name of Company: ");
                string name = Exceptions.CheckIfEmpty(userInput);
                Console.WriteLine("Street adress: ");
                string streetAddress = Exceptions.CheckIfEmpty(userInput);
                Console.WriteLine("City: ");
                string city = Exceptions.CheckIfEmpty(userInput);
                Console.WriteLine("Region: ");
                string region = Console.ReadLine();
                Console.WriteLine("Zip code: ");
                string zipCode = Exceptions.CheckIfEmpty(userInput);
                Console.WriteLine("Country: ");
                string country = Console.ReadLine();
                Console.WriteLine("Telephone: ");
                string phone = Console.ReadLine();

                bank.CreateCustomer(corporateId,name,streetAddress,city,region,zipCode,country,phone);
            }
            if (userInput == "4")
            {
                Console.WriteLine("**Delete Customer**");
                Console.WriteLine();
                Console.WriteLine("Enter the customer-ID you wish to delete");
                int userInputId = Exceptions.InputInt(userInput);
                bank.DeleteCustomer(userInputId);
            }
            if (userInput == "5")
            {
                Console.WriteLine("**New Account**");
                Console.WriteLine();
                Console.WriteLine("Enter customer-ID to generate a new account: ");
                int userInputId = Exceptions.InputInt(userInput);
                bank.CreateNewAccount(userInputId);
            }
            if (userInput == "6")
            {
                Console.WriteLine("**Delete Account**");
                Console.WriteLine();
                Console.WriteLine("Enter account to delete: ");
                int userInputAccountId = Exceptions.InputInt(userInput);
                bank.DeleteAccount(userInputAccountId);
            }

            if (userInput == "7")
            {
                Console.WriteLine("**Cash Deposit**");
                Console.WriteLine();
                Console.WriteLine("Enter account number for deposit: ");
                int accountId = Exceptions.InputInt(userInput);

                Console.WriteLine("Enter amount to deposit to account " + accountId + ": ");
                decimal depositAmount = Exceptions.InputDec(userInput);
                bank.MoneyDeposit(accountId,depositAmount);
            }

            if (userInput == "8")
            {
                Console.WriteLine("**Cash Withdrawal**");
                Console.WriteLine();
                Console.WriteLine("Enter account number for withdrawal: ");
                int accountId = Exceptions.InputInt(userInput);

                Console.WriteLine("Enter amount to withdraw from account " + accountId + ": ");
                decimal withdrawalAmount = Exceptions.InputDec(userInput);
                bank.MoneyWithdrawal(accountId, withdrawalAmount);
            }
            if (userInput == "9")
            {
                Console.WriteLine("**Cash Transfer**");
                Console.WriteLine();
                Console.WriteLine("Enter account to transfer from: ");
                int userInputAccountFrom = Exceptions.InputInt(userInput);

                Console.WriteLine("Enter account to transfer to: ");
                int userInputAccountTo = Exceptions.InputInt(userInput);

                Console.WriteLine("Enter amount to transfer: ");
                decimal amount = Exceptions.InputDec(userInput);

                bank.MoneyTransfer(userInputAccountFrom,userInputAccountTo, amount);
            }

            if (userInput == "10")
            {
                Console.WriteLine("**Account info**");
                Console.WriteLine();
                Console.WriteLine("Enter account ID: ");
                int userInputId = Exceptions.InputInt(userInput);
                bank.AccountInfo(userInputId);
            }

            if (userInput == "11")
            {
                Console.WriteLine("**Apply for credit**");
                Console.WriteLine();
                Console.WriteLine("Enter account number: ");
                int userInputId = Exceptions.InputInt(userInput);
                Console.WriteLine("How much credit do you want?: ");
                decimal amount = Exceptions.InputDec(userInput);
                bank.CreditApplication(userInputId, amount);
            }

            if (userInput == "12")
            {
                Console.WriteLine("**Add / Change account savings rate**");
                Console.WriteLine();
                Console.WriteLine("**Enter account number to add/change savings rate: ");
                int userInputId = Exceptions.InputInt(userInput);
                Console.WriteLine("**Enter new savings rate: ");
                decimal newSaveInterest = decimal.Parse(Console.ReadLine());
                bank.AddOrChangeAccountInterest(userInputId, newSaveInterest);
            }
            
            if (userInput == "13")
            {
                Console.WriteLine("**Change credit debt rate**");
                Console.WriteLine();
                Console.WriteLine("Enter account number: ");
                int userInputId = Exceptions.InputInt(userInput);
                Console.WriteLine("Enter new credit dept rate: ");
                decimal rateInput = Exceptions.InputDec(userInput);
                bank.ChangeCreditDeptInterest(userInputId, rateInput);
            }

            if (userInput == "14")
            {
                bank.PayRate();
                Console.WriteLine("Successful!");
            }

            if (userInput == "15")
            {
                bank.ChargeCreditDeptInterest();
                Console.WriteLine("Successful!");
            }

            if (userInput == "16")
            {
                Console.Clear();
                PrintInfoStart();
                PrintMenu();
            }
        }
    }
}
