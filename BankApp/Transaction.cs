using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Transaction
    {
        public DateTime TransferDate { get; set; } = new DateTime();
        public decimal Amount { get; set; }
        public int AccountSender { get; set; }
        public int AccountReceiver { get; set; }
        public decimal BalanceSender{ get; set; }
        public decimal BalanceReceiver { get; set; }
        public string TypeOfTransfer { get; set; }

        public Transaction()
        {
            
        }
    }
}
