using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
    public static class BankCustomerExtensions
    {
        public static bool isValidCustomerID(this BankCustomer customer)
        {
            return customer.CustomerID.Length == 10;
        }

        public static string GreetCustomer(this BankCustomer customer)
        {
            return $"Hello, {customer.ReturnFullName()}!";
        }
    }


    public static class BankAccountExtensions
    {
        // Extension method to check if the account is overdrawn
        public static bool IsOverdrawn(this BankAccount account)
        {
            return account.Balance < 0;
        }

        // Extension method to check if a specified amount can be withdrawn
        public static bool CanWithdraw(this BankAccount account, double amount)
        {
            return account.Balance >= amount;
        }
    }

}
