
using System.Diagnostics.CodeAnalysis;

int[]? numbers = new int[5];//arrays are fixed size youo cannot append
numbers.Append(6);
for(int i=0;i<numbers.Length;i++)
{
    Console.WriteLine(numbers[i]);
}

List<int> grades = new List<int>(5) { 0,0,0,0};//•	The 5 is for performance (pre-allocating space), not a limit.
grades.Add(90);



for(int i=0;i<grades.Count;i++)
{
    Console.WriteLine(grades[i]);
}

foreach(var item in grades) //foreach for this syntax
{
    Console.WriteLine(item);
}



void Print(string? name, int? count)
{
    if (name is null)
    {
        Console.WriteLine("Name is null");
        return;
    }
    Console.WriteLine($"Name length: {name.Length}");
    if (!count.HasValue)
    {
        count = 1;
    }
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine($"Hello, {name}!");
    }
}

Print(null, null);

var person = new Person("John");
public class Person
{
    public required string Name { get; set; }
    public int Age { get; set; }

    [SetsRequiredMembers] //wihtout this we get error CS9035
    public Person(string name)
    {
        Name = name;
    }
}




