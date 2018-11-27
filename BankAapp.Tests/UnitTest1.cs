using System;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        Bank bank = new Bank();

        private Account accountOne = new Account()
        {
            AccountId = 9988,
            Balance = 200,
            YearCreditDebtRate = 20,
            YearSavingsRate = 20
        };

        private Account accountTwo = new Account()
        {
            AccountId = 8877,
            Balance = 300,
            YearCreditDebtRate = 20,
            YearSavingsRate = 20
        };

        private Account accountThree = new Account()
        {
            AccountId = 7766,
            Balance = 300,
            YearCreditDebtRate = 20,
            YearSavingsRate = 20,
            Credit = 1000
        };

        private Account accountFour = new Account()
        {
            AccountId = 6677,
            Balance = -365,
            YearCreditDebtRate = 15,
            YearSavingsRate = 0,
            Credit = 1000
        };

        [TestMethod]
        public void TestWithdrawalNotExceedBalance()
        {
            bank.Accounts.Add(accountOne);

            decimal withdrawAmount = 500;
            decimal expected = 200;

            bank.MoneyWithdrawal(accountOne.AccountId, withdrawAmount);

            decimal actual = accountOne.Balance;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTransferNotExceedBalance()
        {
            bank.Accounts.Add(accountOne);
            bank.Accounts.Add(accountTwo);

            decimal transferAmount = 300;
            decimal expected = 200;

            bank.MoneyTransfer(accountOne.AccountId, accountTwo.AccountId, transferAmount);

            decimal actual = accountOne.Balance;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestDepositNegativeAmount()
        {
            bank.Accounts.Add(accountOne);

            decimal depositAmount = -100;
            decimal expected = 200;

            bank.MoneyDeposit(accountOne.AccountId, depositAmount);

            decimal actual = accountOne.Balance;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithdrawNegativeAmount()
        {
            bank.Accounts.Add(accountOne);

            decimal withdrawAmount = -100;
            decimal expected = 200;

            bank.MoneyWithdrawal(accountOne.AccountId, withdrawAmount);

            decimal actual = accountOne.Balance;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWithdrawFromAccountWithCredit()
        {
            bank.Accounts.Add(accountThree);

            decimal withdrawAmount = 1200;
            decimal expected = -900;

            bank.MoneyWithdrawal(accountThree.AccountId, withdrawAmount);

            decimal actual = accountThree.Balance;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTransferFromAccountWithCredit()
        {
            bank.Accounts.Add(accountTwo);
            bank.Accounts.Add(accountThree);

            decimal transferAmount = 1200;
            decimal expected = -900;

            bank.MoneyTransfer(accountThree.AccountId, accountTwo.AccountId, transferAmount);

            decimal actual = accountThree.Balance;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestChargeCredit()
        {
            bank.Accounts.Add(accountFour);

            decimal expected = -365.15M;

            bank.ChargeCreditDeptInterest();

            decimal actual = accountFour.Balance;

            Assert.AreEqual(expected, actual);
        }
    }
}