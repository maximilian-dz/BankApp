using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class FileHandler
    {
        public static int CountOfCustomers;
        public static int CountOfAccounts;
        public static int CountOfTransactions;
        
        public static Bank ReadData(string path)
        {
            var bank = new Bank();

            using (var reader = new StreamReader(path))
            {
                CountOfCustomers = int.Parse(reader.ReadLine());

                for (int i = 0; i < CountOfCustomers; i++)
                {
                    string line = reader.ReadLine();
                    var columns = line.Split(new[] {';'});

                    var customer = new Customer
                    {
                        CustomerId = int.Parse(columns[0]),
                        CorporateId = columns[1],
                        Name = columns[2],
                        StreetAddress = columns[3],
                        City = columns[4],
                        Region = columns[5],
                        ZipCode = columns[6],
                        Country = columns[7],
                        Phone = columns[8]
                    };

                    bank.Customers.Add(customer);
                }

                    CountOfAccounts = int.Parse(reader.ReadLine());

                for (int i = 0; i < CountOfAccounts; i++)
                {
                    string line = reader.ReadLine();
                    var columns = line.Split(new[] { ';' }/*, StringSplitOptions.RemoveEmptyEntries*/);

                    var account = new Account
                    {
                        AccountId = int.Parse(columns[0]),
                        CustomerId = int.Parse(columns[1]),
                        Balance = decimal.Parse(columns[2], CultureInfo.InvariantCulture)
                    };

                    bank.Accounts.Add(account);
                }
            }

            return bank;
        }
        
        public static Bank SaveData(Bank bank, string path)
        {
            using (var writer = new StreamWriter(path))
            { 
                
                writer.WriteLine(CountOfCustomers);
                foreach (Customer customer in bank.Customers)
                {
                    writer.Write(customer.CustomerId + ";");
                    writer.Write(customer.CorporateId + ";");
                    writer.Write(customer.Name + ";"); 
                    writer.Write(customer.StreetAddress + ";"); 
                    writer.Write(customer.City + ";");
                    writer.Write(customer.Region + ";");
                    writer.Write(customer.ZipCode + ";");
                    writer.Write(customer.Country + ";");
                    writer.Write(customer.Phone); 
                    writer.WriteLine();
                }

                writer.WriteLine(CountOfAccounts);
                foreach (Account account in bank.Accounts)
                {
                    writer.Write(account.AccountId + ";");
                    writer.Write(account.CustomerId + ";");
                    writer.Write(account.Balance.ToString(CultureInfo.InvariantCulture) + ";");
                    writer.WriteLine();
                }
            }

            return bank;
        }

        public static Bank SaveTransactionData(Bank bank, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(CountOfTransactions);
                foreach (Transaction transaction in bank.Transactions)
                {
                    writer.Write("Date:"+ transaction.TransferDate + ";");
                    writer.Write(transaction.TypeOfTransfer + ";");
                    writer.Write("Amount:" + transaction.Amount + ";");
                    writer.Write("From:" + transaction.AccountSender + ";");
                    writer.Write("To:" + transaction.AccountReceiver + ";");
                    writer.Write("BalanceOn" + transaction.AccountSender + "=" 
                                 +transaction.BalanceSender.ToString(CultureInfo.InvariantCulture) + ";");
                    writer.Write("BalanceOn" + transaction.AccountReceiver + "=" 
                                 + transaction.BalanceReceiver.ToString(CultureInfo.InvariantCulture) + ";");
                    writer.WriteLine();
                }
            }
            return bank;
        }
    }
}
