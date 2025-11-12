using System;
try
{
    Console.WriteLine("Enter your age: ");
    int age = int.Parse(Console.ReadLine());
    Console.WriteLine($"You are {age} years old.");
}
catch (Exception)
{
    Console.WriteLine("Invalid input. Please enter a valid number.");
}

finally
{
    Console.WriteLine("Thanks for using the age checker.");
}