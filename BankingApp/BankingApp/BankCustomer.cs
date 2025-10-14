using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
    public partial class BankCustomer
    {
        private string _firstName="Joe";
        private string _lastName = "Doe";

        public readonly string CustomerID;
        private static int s_nextCustomerID;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public BankCustomer()
        {
            CustomerID = (s_nextCustomerID++).ToString("D10");//decimal nim 10 digit , pad with 0 on the left
        }

        static BankCustomer()
        {
            //initialise the static id
            Random random = new Random();
            s_nextCustomerID = random.Next(10000000, 20000000);
        }

        public BankCustomer(BankCustomer customer)
        {
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.CustomerID= (s_nextCustomerID++).ToString("D10");
        }

        public BankCustomer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerID = (s_nextCustomerID++).ToString("D10");
        }

        
        //public BankCustomer(string firstName, string lastName, string customerIdNumber)
        //{
        //    FirstName = firstName;
        //    LastName = lastName;
        //    CustomerID = customerIdNumber;
        //} you ar enot allowed to have an externally provided ID


    }
}
