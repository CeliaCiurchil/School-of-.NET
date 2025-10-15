
using System.Diagnostics.CodeAnalysis;

void Print(string name, int? count)
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



