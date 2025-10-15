using System.Globalization;

string s1="Literal string";

String s2 = "Literal string";

string s3;

string? s4 = null;

string s5 = string.Empty; //""

string sentence = "she said,\"help\"\r\n next line";//escape sequence

const string path = @"C:\temp\newfolder\file.txt";

string rawliteral = """she said "hello" """;

string ss = string.Format("hello {0}",path);
Console.WriteLine(ss);

if (!string.IsNullOrEmpty(sentence))
{
    Console.WriteLine(ss.ToLower());
}

//substring
string sub = s1.Substring(2,5);

//split strings'
var splitString = rawliteral.Split(" ");

for (int i = 0; i < splitString.Length; i++)
{
    Console.WriteLine(splitString[i]);
}

//replace
string replaces = s1.Replace('s','l');
string replaces2 = s1.Replace("string","");

Console.WriteLine(replaces2);

int value = 100;
string salary = value.ToString("C");//value.ToString(); write it like currency
Console.WriteLine(salary);
