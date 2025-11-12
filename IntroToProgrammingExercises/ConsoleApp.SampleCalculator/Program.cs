
//variable decalration

using System.Linq.Expressions;

int choice = 0, num1=0,num2=0;


while (choice != -1)
{
    try
    {
        Console.WriteLine("Please select -1 to exist");
        Console.WriteLine("Please enter the operation");
        Console.WriteLine("1. Add");
        Console.WriteLine("2. Subtract");
        Console.WriteLine("3. Multiply");
        Console.WriteLine("4. Substr");
        choice = Convert.ToInt32(Console.ReadLine());

        if (choice == -1)
        {
            break;
        }
        Console.WriteLine("Please enter the first number");
        num1 = int.Parse(Console.ReadLine());

        Console.WriteLine("Please enter the second number");
        num2 = Convert.ToInt32(Console.ReadLine());
       

        int answer = 0;
        switch (choice)
        {
            case 1:
                {
                    answer = num1 + num2;
                    break;
                }
            case 2:
                {
                    answer = num1 - num2;
                    break;
                }
            case 3:
                {
                    answer = num1 * num2;
                    break;
                }
            case 4:
                {
                    answer = num1 / num2;
                    
                    break;
                }
            default:
                {
                    throw new Exception("Invalid operation");

                }
        }
        Console.WriteLine($"The answer is {answer}\n");


    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Cannot divide by zero");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    finally
    {
        Console.WriteLine("Press to continue");
        Console.ReadLine();
        Console.Clear();
    }

}

Console.WriteLine("end, thanks");