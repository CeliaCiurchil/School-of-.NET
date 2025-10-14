using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
    public partial class BankCustomer
    {
        public string ReturnFullName()
        {
            return FirstName + " " + LastName;
        }


        // Method to update the customer's name
        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }


        // Method to display customer information
        public string DisplayCustomerInfo()
        {
            return $"Customer ID: {CustomerID}, Name: {ReturnFullName()}";
        }

    }
}
