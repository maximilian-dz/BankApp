using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Account
    {
        public int AccountId { get; set;}
        public int CustomerId { get; set; }
        public decimal Balance { get; set; } 
        public decimal Credit { get; set; }
        public decimal YearSavingsRate { get; set; }
        public decimal YearCreditDebtRate { get; set; }
        
        public Account()
        {

        }
    }
}
