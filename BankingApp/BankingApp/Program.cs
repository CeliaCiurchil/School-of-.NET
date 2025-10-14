using BankingApp;
using System.Globalization;

string firstName = "Jane";
string lastName = "Smith";

BankCustomer customer1 = new BankCustomer();

firstName = "Alice";
BankCustomer customer2 = new BankCustomer(firstName,lastName);


firstName = "Sandy";
lastName = "Zoeng";
BankCustomer customer3 = new BankCustomer(firstName, lastName);

Console.WriteLine($"BankCustomer 1: {customer1.FirstName} {customer1.LastName} {customer1.CustomerID}");
Console.WriteLine($"BankCustomer 2: {customer2.FirstName} {customer2.LastName} {customer2.CustomerID}");
Console.WriteLine($"BankCustomer 3: {customer3.FirstName} {customer3.LastName} {customer3.CustomerID}");


BankAccount account1 = new BankAccount(customer1.CustomerID);
BankAccount account2 = new BankAccount(customer2.CustomerID, 1500, "Checking");
BankAccount account3 = new BankAccount(customer3.CustomerID, 2500, "Checking");

// Demonstrate the use of BankCustomer properties
customer1.FirstName = "Thomas";
customer1.LastName = "Margand";
//customer1.CustomerId = "1234567890"; // This line will not compile

Console.WriteLine($"Updated BankCustomer 1: {customer1.FirstName} {customer1.LastName} {customer1.CustomerID}");


Console.WriteLine($"Account 1: Account # {account1.AccountNumber}, type {account1.AccountType}, balance {account1.Balance}, rate {BankAccount.InterestRate}, customer ID {account1.CustomerID}");
Console.WriteLine($"Account 2: Account # {account2.AccountNumber}, type {account2.AccountType}, balance {account2.Balance}, rate {BankAccount.InterestRate}, customer ID {account2.CustomerID}");
Console.WriteLine($"Account 3: Account # {account3.AccountNumber}, type {account3.AccountType}, balance {account3.Balance}, rate {BankAccount.InterestRate}, customer ID {account3.CustomerID}");

Console.WriteLine(customer1.GreetCustomer());
Console.WriteLine(customer1.ReturnFullName());


// Step 4: Demonstrate the use of BankAccount methods
Console.WriteLine("\nDemonstrating BankAccount methods...");

// Deposit
double depositAmount = 500;
Console.WriteLine($"Depositing {depositAmount.ToString("C", CultureInfo.CurrentCulture)} into Account 1...");
account1.Deposit(depositAmount);
Console.WriteLine($"Account 1 after deposit: Balance = {account1.Balance.ToString("C", CultureInfo.CurrentCulture)}");

// Withdraw
double withdrawalAmount = 200;
Console.WriteLine($"Withdrawing {withdrawalAmount.ToString("C", CultureInfo.CurrentCulture)} from Account 2...");
bool withdrawSuccess = account2.Withdraw(withdrawalAmount);
Console.WriteLine($"Account 2 after withdrawal: Balance = {account2.Balance.ToString("C", CultureInfo.CurrentCulture)}, Withdrawal successful: {withdrawSuccess}");

// Transfer
double transferAmount = 300;
Console.WriteLine($"Transferring {transferAmount.ToString("C", CultureInfo.CurrentCulture)} from Account 3 to Account 1...");
bool transferSuccess = account3.Transfer(account1, transferAmount);
Console.WriteLine($"Account 3 after transfer: Balance = {account3.Balance.ToString("C", CultureInfo.CurrentCulture)}, Transfer successful: {transferSuccess}");
Console.WriteLine($"Account 1 after receiving transfer: Balance = {account1.Balance.ToString("C", CultureInfo.CurrentCulture)}");

// Demonstrate the use of utility methods in AccountCalculations
Console.WriteLine("\nDemonstrating utility methods in AccountCalculations...");

// Calculate compound interest for account1
double principal = account1.Balance;
double rate = BankAccount.InterestRate;
double time = 1; // 1 year
double compoundInterest = AccountCalculations.CalculateCompoundInterest(principal, rate, time);
Console.WriteLine($"Compound interest on account1 balance of {principal.ToString("C", CultureInfo.CurrentCulture)} at {rate * 100:F2}% for {time} year: {compoundInterest.ToString("C", CultureInfo.CurrentCulture)}");

// Validate account number for account1
int accountNumber = account1.AccountNumber;
bool isValidAccountNumber = AccountCalculations.ValidateAccountNumber(accountNumber);
Console.WriteLine($"Is account number {accountNumber} valid? {isValidAccountNumber}");

// Calculate transaction fee using rates and max fee values from the bank account class
double transactionAmount = 800;
double transactionFee = AccountCalculations.CalculateTransactionFee(transactionAmount, BankAccount.TransactionRate, BankAccount.MaxTransactionFee);
Console.WriteLine($"Transaction fee for a {transactionAmount.ToString("C", CultureInfo.CurrentCulture)} wire transfer at a {BankAccount.TransactionRate * 100:F2}% rate and a max fee of {BankAccount.MaxTransactionFee.ToString("C", CultureInfo.CurrentCulture)} is: {transactionFee.ToString("C", CultureInfo.CurrentCulture)}");

// Calculate overdraft fee using rates and max fee values from the bank account class
double overdrawnAmount = 500;
double overdraftFee = AccountCalculations.CalculateOverdraftFee(overdrawnAmount, BankAccount.OverdraftRate, BankAccount.MaxOverdraftFee);
Console.WriteLine($"Overdraft fee for an account that's {overdrawnAmount.ToString("C", CultureInfo.CurrentCulture)} overdrawn, using a penalty rate of {BankAccount.OverdraftRate * 100:F2}% and a max fee of {BankAccount.MaxOverdraftFee.ToString("C", CultureInfo.CurrentCulture)} is: {overdraftFee.ToString("C", CultureInfo.CurrentCulture)}");

// Exchange currency
double originalCurrencyProvided = 100;
double currentExchangeRate = 1.2;
double foreignCurrencyReceived = AccountCalculations.ReturnForeignCurrency(originalCurrencyProvided, currentExchangeRate);
Console.WriteLine($"The foreign currency received after exchanging {originalCurrencyProvided.ToString("C", CultureInfo.CurrentCulture)} at an exchange rate of {currentExchangeRate:F2} is: {foreignCurrencyReceived.ToString("C", CultureInfo.CurrentCulture)}");

// Apply interest
Console.WriteLine("\nApplying interest to Account 1...");
double timePeriodYears = 30 / 365.0;    // calculate the interest accrued during the past 30 days
account1.ApplyInterest(timePeriodYears);
Console.WriteLine($"Account 1 after applying interest: Balance = {account1.Balance.ToString("C", CultureInfo.CurrentCulture)}, Interest Rate = {BankAccount.InterestRate:P2}, Time Period = {timePeriodYears:F2} years");

// Issue cashier's check
Console.WriteLine("Issue cashier's check from account 2...");
double checkAmount = 500;
bool receivedCashiersCheck = account2.IssueCashiersCheck(checkAmount);
Console.WriteLine($"Account 2 after requesting cashier's check: Received cashier's check: {receivedCashiersCheck}, Balance = {account2.Balance.ToString("C", CultureInfo.CurrentCulture)}, Transaction Amount = {checkAmount.ToString("C", CultureInfo.CurrentCulture)}, Transaction Fee Rate = {BankAccount.TransactionRate:P2}, Max Transaction Fee = {BankAccount.MaxTransactionFee.ToString("C", CultureInfo.CurrentCulture)}");

// Apply refund
Console.WriteLine("Applying refund to Account 3...");
double refundAmount = 50;
account3.ApplyRefund(refundAmount);
Console.WriteLine($"Account 3 after applying refund: Balance = {account3.Balance.ToString("C", CultureInfo.CurrentCulture)}, Refund Amount = {refundAmount.ToString("C", CultureInfo.CurrentCulture)}");


// Step 7: Demonstrate the use of named and optional parameters
Console.WriteLine("\nDemonstrating named and optional parameters...");
string customerId = customer1.CustomerID;
double openingBalance = 1500;

// specify a customer ID and use the default optional parameters (balance and accountType)
BankAccount account4 = new BankAccount(customerId);
Console.WriteLine(account4.DisplayAccountInfo());

// specify the customer ID and opening balance and use default account type
BankAccount account5 = new BankAccount(customer1.CustomerID, openingBalance);
Console.WriteLine(account5.DisplayAccountInfo());

// specify the customer ID and use a named parameter to specify account type
BankAccount account6 = new BankAccount(customer2.CustomerID, accountType: "Checking");
Console.WriteLine(account6.DisplayAccountInfo());

// use named parameters to specify all parameters
BankAccount account7 = new BankAccount(accountType: "Checking", balance: 5000, customerIdNumber: customer2.CustomerID);
Console.WriteLine(account7.DisplayAccountInfo());

BankCustomer customer4 = new BankCustomer { FirstName="Mikaela"};
Console.WriteLine($"BankCustomer 4: {customer4.FirstName} {customer4.LastName} {customer4.CustomerID}");

BankCustomer customer5 = new BankCustomer(customer4);