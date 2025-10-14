using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
    public class BankAccount
    {
        public int AccountNumber { get; }
        public double Balance { get; private set; } = 0; // should only be set inside the class

        // Public read-only static properties
        public static double InterestRate { get; private set; }
        public static double TransactionRate { get; private set; }
        public static double MaxTransactionFee { get; private set; }
        public static double OverdraftRate { get; private set; }
        public static double MaxOverdraftFee { get; private set; }

        public string AccountType { get; set; } = "Checking";
        public string CustomerID { get; }

        private static int s_nextAccountNumber = 1;

        static BankAccount()
        {
            Random random = new Random();
            s_nextAccountNumber = random.Next(10000000, 20000000);
            InterestRate = 0.00; // Default interest rate for checking accounts
            TransactionRate = 0.01; // Default transaction rate for wire transfers and cashier's checks
            MaxTransactionFee = 10; // Maximum transaction fee for wire transfers and cashier's checks
            OverdraftRate = 0.05; // Default penalty rate for an overdrawn checking account (negative balance)
            MaxOverdraftFee = 10; // Maximum overdraft fee for an overdrawn checking account
        }


        // Copy constructor for BankAccount
        public BankAccount(BankAccount existingAccount)
        {
            this.AccountNumber = s_nextAccountNumber++;
            this.CustomerID = existingAccount.CustomerID;
            this.Balance = existingAccount.Balance;
            this.AccountType = existingAccount.AccountType;
        }


        public BankAccount(string customerIdNumber, double balance = 200, string accountType = "Checking")
        {
            this.AccountNumber = s_nextAccountNumber++;
            this.CustomerID = customerIdNumber;
            this.Balance = balance;
            this.AccountType = accountType;
        }


        // Method to deposit money into the account
        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
            }
        }

        public bool Withdraw(double amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;

        }

        public bool Transfer(BankAccount targetAccount, double amount)
        {
            if (this.Withdraw(amount))
            {
                targetAccount.Deposit(amount);
                return true;
            }
            return false;
        }


        // Method to apply interest to the account

        // Method to apply interest
        public void ApplyInterest(double years)
        {
            Balance += AccountCalculations.CalculateCompoundInterest(Balance, InterestRate, years);
        }

        // Method to issue a cashier's check
        public bool IssueCashiersCheck(double amount)
        {
            if (amount > 0 && Balance >= amount + BankAccount.MaxTransactionFee)
            {
                Balance -= amount;
                Balance -= AccountCalculations.CalculateTransactionFee(amount, BankAccount.TransactionRate, BankAccount.MaxTransactionFee);
                return true;
            }
            return false;
        }

        // Method to apply a refund
        public void ApplyRefund(double refund)
        {
            Balance += refund;
        }



        // Method to display account information
        public string DisplayAccountInfo()
        {
            return $"Account Number: {AccountNumber}, Type: {AccountType}, Balance: {Balance}, Interest Rate: {InterestRate}, Customer ID: {CustomerID}";
        }


    }
}
