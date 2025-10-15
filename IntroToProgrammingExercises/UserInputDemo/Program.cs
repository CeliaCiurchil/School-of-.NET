using System.Diagnostics.CodeAnalysis;
using System.Globalization;

string? name = null;
//Console.Write($"name: {name.Length}");

int age;
char gender;
int retirementAge;

DateOnly dob = new DateOnly();

Console.Write("Enter your date of birth: (dd/mm/yyyy)");
dob = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
age = DateTime.Now.Year - dob.Year;


Console.Write("Enter your gender (M/F): ");
gender = char.Parse(Console.ReadLine());

Console.Write("Enter your retirement age: ");
retirementAge = int.Parse(Console.ReadLine());

int workingYearsLeft = retirementAge - age;
int estimatedRetirementDate = DateTime.Now.AddYears(workingYearsLeft).Year;
Console.WriteLine($"You have {workingYearsLeft} working years left until retirement\n Year {estimatedRetirementDate}");
double num=1.3;
int num2=2;
double num3 = num+num2;

switch (num)
{
    case 1:
        Console.WriteLine("num1 is 1");
        break;
    case 2:
        Console.WriteLine("num1 is 2");
        break;
    default:
        Console.WriteLine("num1 is neither 1 nor 2");
        break;

}
