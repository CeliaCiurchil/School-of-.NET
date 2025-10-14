
Console.WriteLine("Please select -1 to exist");
Console.WriteLine("Please enter the operation");
Console.WriteLine("1. Add");
Console.WriteLine("2. Subtract");
Console.WriteLine("3. Multiply");
Console.WriteLine("4. Substr");

int choice = Convert.ToInt32(Console.ReadLine());
while (choice != -1)
{

    Console.WriteLine("Please enter the first number");
    int num1 = int.Parse(Console.ReadLine());

    Console.WriteLine("Please enter the second number");
    int num2 = Convert.ToInt32(Console.ReadLine());


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
                Console.WriteLine("Not a valid option");
                break;
            }
    }

    Console.WriteLine($"The answer is {answer}\n");

    Console.WriteLine("Please select -1 to exist");
    Console.WriteLine("Please enter the operation");
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Subtract");
    Console.WriteLine("3. Multiply");
    Console.WriteLine("4. Substr");

    choice = Convert.ToInt32(Console.ReadLine());
}

Console.WriteLine("end, thanks");