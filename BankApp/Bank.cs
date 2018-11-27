using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Bank
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Bank()
        {


        }

        public void SearchCustomer(string userInput)
        {
            var customers = Customers.Where(c =>
                    c.Name.ToLower().Contains(userInput) || c.City.ToLower().Contains(userInput)).ToList();

            if (customers.Count > 0)
            {
                foreach (var customer in customers)
                {
                    Console.Write(customer.CustomerId + ": ");
                    Console.Write(customer.Name + ", ");
                    Console.WriteLine(customer.City);
                }
            }

            else
            {
                Console.WriteLine("Your search did not return any results..");
            }
        }

        public void CustomerInfo(int userInput) 
        {
            var account = Accounts.Where(a => a.CustomerId.Equals(userInput) || a.AccountId.Equals(userInput)).ToList();
            int customerId;

            if (account.Count > 0)
            {
                customerId = account[0].CustomerId;
                decimal totalAmount = 0;

                var customer = Customers.Single(c => c.CustomerId.Equals(customerId));
                var accounts = Accounts.Where(a => a.CustomerId.Equals(customerId));

                Console.WriteLine();
                Console.WriteLine("Customer ID: " + customer.CustomerId);
                Console.WriteLine("Corporate-ID: " + customer.CorporateId);
                Console.WriteLine("Name: " + customer.Name);
                Console.WriteLine("Address: {0}, {1}, {2} ", customer.StreetAddress, customer.ZipCode, customer.City);
                Console.WriteLine();

                foreach (var value in accounts)
                {
                    Console.Write("Account " + value.AccountId + ": ");
                    Console.WriteLine(value.Balance + " SEK");
                    totalAmount += value.Balance;
                }

                Console.WriteLine("Total balance: {0} SEK", totalAmount);
            }
            else
            {
                Console.WriteLine("Customer or Account-ID not found");
            }
        }

        public Customer CreateCustomer(
            string corpId, string name, string street, string city, string region, string zipcode, string country,
            string phone)
        {
            var getLastCustomerId = Customers.Select(c => c.CustomerId).OrderBy(x => x).Last();

            Customer newCustomer = new Customer
            {
                CustomerId = getLastCustomerId + 1,
                CorporateId = corpId,
                Name = name,
                StreetAddress = street,
                City = city,
                Region = region,
                ZipCode = zipcode,
                Country = country,
                Phone = phone
            };

            Customers.Add(newCustomer);

            Console.WriteLine("Welcome to MaxBank {0}. Your registration is complete. Your new customer-ID is {1}.",
                              newCustomer.Name, newCustomer.CustomerId);

            FileHandler.CountOfCustomers++;

            CreateNewAccount(newCustomer.CustomerId);
            return newCustomer;
        }

        public void DeleteCustomer(int customerId)
        {
            var accounts = Accounts.Where(a => customerId.Equals(a.CustomerId)).ToList();
            
            if (accounts.Count == 0)
            {
                var customer = Customers.SingleOrDefault(c => customerId.Equals(c.CustomerId));

                if (customer == null)
                {
                    Console.WriteLine("Customer not found.");
                }

                else
                {
                    Console.WriteLine("Customer {0} has been sucessfully removed.", customer.CustomerId);
                    Customers.Remove(customer);
                    FileHandler.CountOfCustomers--;
                }
            }

            else
            {
                Console.WriteLine("Denied!");
                Console.WriteLine(
                    "Customer has active accounts. In order to delete customer you need to remove accounts.");
            }
        }

        public void CreateNewAccount(int customerId)
        {
            var getLastAccountId  = Accounts.Select(a => a.AccountId).OrderBy(x => x).Last();

            var customers = Customers.SingleOrDefault(c => c.CustomerId.Equals(customerId));

            if (customers != null)
            {
                var newAccount = new Account
                {
                    AccountId = getLastAccountId + 1,
                    CustomerId = customerId,
                    Balance = 0
                };
                Accounts.Add(newAccount);

                Console.WriteLine("A new account has been successfully created for customer " + customerId + ".");
                Console.WriteLine("Your new account number is {0}.", getLastAccountId + 1);
                FileHandler.CountOfAccounts++;
            }

            else
            {
                Console.WriteLine("Customer-id not found.");
            }
        }

        public void DeleteAccount(int accountId) 
        {
            var accounts = Accounts.Where(c => accountId.Equals(c.AccountId)).ToList();
            
            foreach (var item in accounts)
            {
                if (item.Balance == 0)
                {
                    Console.WriteLine("Account " + item.AccountId + " Removed.");
                    Accounts.Remove(item);
                    FileHandler.CountOfAccounts--;
                }

                else
                {
                    Console.WriteLine("You can only remove an account if balance is 0 SEK.");
                }
            }
        }

        public void MoneyDeposit(int accountId, decimal amountToDeposit)
        {
            if (amountToDeposit > 0)
            {
                var accounts = Accounts.Where(a => accountId.Equals(a.AccountId)).ToList();
                foreach (var item in accounts)
                {
                    item.Balance += amountToDeposit;
                    Console.WriteLine("Deposit accepted");
                    Console.WriteLine("Current balance on account " + accountId + " is " + item.Balance);
                    SaveTransaction(accountId, accountId, amountToDeposit, "Deposit");
                }
            }
            else Console.WriteLine("Invalid amount. You can not deposit a negative number. Start over!");
        }

        public void MoneyWithdrawal(int accountId, decimal amountToWithdraw)
        {
            var accounts = Accounts.Where(a => accountId.Equals(a.AccountId)).ToList();

            foreach (var account in accounts)
            {
                if (amountToWithdraw > 0)
                {
                    if ((account.Balance + account.Credit) - amountToWithdraw >= 0)
                    {
                        account.Balance -= amountToWithdraw;
                        Console.WriteLine("Withdrawal accepted");
                        Console.WriteLine("Current balance on account " + accountId + " is " + account.Balance);
                        SaveTransaction(accountId, accountId, amountToWithdraw, "Withdrawal");
                    }

                    else
                    {
                        Console.WriteLine("You have insufficient funds. Current balance on account " + accountId +
                                          " is " + account.Balance);
                        Console.WriteLine("Credit on account {0} is {1} SEK", account.AccountId, account.Credit);
                    }
                }
                else Console.WriteLine("Invalid amount. You can not withdraw a negative number. Start over!");
            }
        }

        public void MoneyTransfer(int accountFrom, int accountTo, decimal amount)
        {
            if (amount > 0)
            {
                var query = Accounts.Where(c => c.AccountId.Equals(accountFrom)).ToList();
                var query2 = Accounts.Where(c => c.AccountId.Equals(accountTo)).ToList();

                foreach (var account in query)
                {
                    if ((account.Balance + account.Credit) - amount >= 0)
                    {
                        account.Balance -= amount;
                        Console.WriteLine("Transfer accepted.");
                        Console.WriteLine("{0} SEK transfered from {1} to {2}.", amount, accountFrom, accountTo);

                        foreach (var item in query2)
                        {
                            item.Balance += amount;
                        }

                        SaveTransaction(accountFrom, accountTo, amount, "Transfer");
                    }
                    else
                    {
                        Console.WriteLine("Transfer rejected.");
                        Console.WriteLine("Account {0} has insufficent funds. Current balance on account {1} is {2}."
                            , accountFrom, accountFrom, account.Balance);
                        Console.WriteLine("Credit on account {0} is {1} SEK", account.AccountId, account.Credit);
                    }
                }
            }
            else Console.WriteLine("Invalid amount. You can not transfer a negative number. Start over!");
        }

        public Transaction SaveTransaction(int accountSender, int accountReceiver, decimal amount, string typeOfTransfer)
        {
            Transaction transaction = new Transaction();

            var getBalanceSender = Accounts.SingleOrDefault(c => c.AccountId.Equals(accountSender));
            var getBalanceReceiver = Accounts.SingleOrDefault(c => c.AccountId.Equals(accountReceiver));

            transaction.TypeOfTransfer = typeOfTransfer;
            transaction.TransferDate = DateTime.Now;
            transaction.AccountSender = accountSender;
            transaction.AccountReceiver = accountReceiver;
            transaction.Amount = amount;
            transaction.BalanceSender = getBalanceSender.Balance;
            transaction.BalanceReceiver = getBalanceReceiver.Balance;
            Transactions.Add(transaction);
            FileHandler.CountOfTransactions++;
            return transaction;
        }

        public void AccountInfo(int accountId)
        {
            var account = Accounts.SingleOrDefault(a => a.AccountId.Equals(accountId));

            if (account != null)
            {
                Console.WriteLine("**Transfer history for {0}**", accountId);

                var accountTransaction = Transactions.Where(t => t.AccountSender.Equals(accountId) || t.AccountReceiver.Equals(accountId));

                foreach (var transaction in accountTransaction)

                {
                    if (transaction.TypeOfTransfer == "Transfer")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Date: {0} ", transaction.TransferDate);
                        Console.WriteLine("{0} SEK transfered from {1} to {2}.", transaction.Amount,
                                            transaction.AccountSender,transaction.AccountReceiver);
                    }

                    if (transaction.TypeOfTransfer == "Deposit")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Date: {0} ", transaction.TransferDate);
                        Console.WriteLine("{0} SEK Deposited", transaction.Amount);
                    }

                    if (transaction.TypeOfTransfer == "Withdrawal")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Date: {0} ", transaction.TransferDate);
                        Console.WriteLine("{0} SEK Withdrawn", transaction.Amount);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Current balance on {0} is {1} SEK.", account.AccountId, account.Balance);
                Console.WriteLine("Account credit: {0}", account.Credit);
                Console.WriteLine("Account savings rate: {0}%", account.YearSavingsRate);
                Console.WriteLine("Account credit dept rate: {0}%", account.YearCreditDebtRate);
            }

            else
            {
                Console.WriteLine("Account-Id not found!");
            }
        }

        public void AddOrChangeAccountInterest(int userInputId, decimal newSaveRate)
        {
            var findAccount = Accounts.Where(a => a.AccountId.Equals(userInputId)).ToList();

            if (findAccount.Count == 0)
            {
                Console.WriteLine("Account not found!");
            }

            else
            {
                if (newSaveRate > 0)
                {
                    foreach (var account in findAccount)
                    {
                        account.YearSavingsRate = newSaveRate;
                        Console.WriteLine("Savings interest for account {0} successfully changed to {1}%", userInputId, newSaveRate);
                    }
                }

                else
                {
                    Console.WriteLine("Invalid savings rate. Can not be a negative number");
                }
            }
        }

        public void PayRate()
        {
            foreach (var account in Accounts) 
            {
                if (account.YearSavingsRate > 0 && account.Balance > 0)
                {
                    decimal dayRate = Math.Round((((account.YearSavingsRate / 100) * account.Balance)) / 365, 2);
                    account.Balance += dayRate;
                    SaveTransaction(account.AccountId, account.AccountId, dayRate, "Interest");
                }
            }
        }

        public void CreditApplication(int accountId, decimal amount)
        {
            var findAccount = Accounts.Where(a => a.AccountId.Equals(accountId)).ToList();

                foreach (var account in findAccount)
                {
                    account.Credit = amount;
                    account.YearCreditDebtRate = 20M;
                    Console.Write("Your application has been approved. Account {0} now has {1} credit", account.AccountId, account.Credit );
                    Console.WriteLine("Credit dept interest is: {0}", account.YearCreditDebtRate);
                }
        }

        public void ChangeCreditDeptInterest(int accountId, decimal rateInput)
        {
            var findAccount = Accounts.Where(a => a.AccountId.Equals(accountId)).ToList();
            foreach (var account in findAccount)
            {
                if (rateInput > 0)
                {
                    account.YearCreditDebtRate = rateInput;
                    Console.WriteLine("Credit dept rate for account {0} successfully changed to {1}%", accountId, rateInput);
                }
                else
                {
                    Console.WriteLine("Invalid credit dept rate. Can not be a negative number");
                }
            }
        }

        public void ChargeCreditDeptInterest()
        {
            var accounts = Accounts.Where(a => a.Credit > 0);

            foreach (var account in accounts)
            {
                if (account.Balance < 0)
                {
                    decimal dayRate = Math.Round((((account.YearCreditDebtRate / 100) * account.Balance)) / 365, 2);
                    account.Balance += dayRate;
                    SaveTransaction(account.AccountId, account.AccountId, dayRate, "Interest");
                }
            }
        }
    }
}
