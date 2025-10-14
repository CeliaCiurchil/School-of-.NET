int age;
char gender;
int retirementAge;



Console.Write("Enter your age: ");
age = int.Parse(Console.ReadLine());

Console.Write("Enter your gender (M/F): ");
gender = char.Parse(Console.ReadLine());

Console.Write("Enter your retirement age: ");
retirementAge = int.Parse(Console.ReadLine());

int workingYearsLeft = retirementAge - age;

Console.WriteLine($"You have {workingYearsLeft} working years left until retirement\n");
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
