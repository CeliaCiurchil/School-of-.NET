//Empty DateTime object
//DateTime dateTime = new DateTime();

//var dateofBirth = new DateTime(1990, 1, 1);

//Console.WriteLine($"Date of birth: {dateofBirth}");

//var exactDate = new DateTime(2023, 6, 15, 14, 30, 0,DateTimeKind.Local);
//Console.WriteLine($"Date of birth: {exactDate}");

//Console.WriteLine($"Day: {dateofBirth.Day}");
//Console.WriteLine($"Month: {dateofBirth.Month}");
//Console.WriteLine($"Year: {dateofBirth.Year}");
//Console.WriteLine($"Day of week: {dateofBirth.DayOfWeek}");
//Console.WriteLine($"Day of year: {dateofBirth.DayOfYear}");
//Console.WriteLine($"Hour: {exactDate.Hour}");
//Console.WriteLine($"Minute: {exactDate.Minute}");
//Console.WriteLine($"Second: {exactDate.Second}");
//Console.WriteLine($"Kind: {exactDate.Kind}");
//Console.WriteLine($"Ticks: {exactDate.Ticks}");//number of 100 nanosecond intervals since 1/1/0001 12:00am

////create from timestamp
using System.Globalization;

DateTime now = DateTime.Now;
//Console.WriteLine($"Now: {now}");

//Console.WriteLine("what is your dob YYYY/MM/DD");
//DateTime dob = DateTime.Parse(Console.ReadLine());

//Console.WriteLine($"You are born on {dob.Year}");

////change format DateTime
//Console.WriteLine(dob.ToString("dd-MM-yyyy"));
//Console.WriteLine(dob.ToString("MM/dd/yyyy"));
//Console.WriteLine(dob.ToString("dddd, dd MMMM yyyy"));

//// DateTime Offset and TimeZone offset
//var utcNow = DateTimeOffset.UtcNow;
//var tz = TimeZoneInfo.Local.GetUtcOffset(utcNow);

//var dto = new DateTimeOffset(now, tz);
//Console.WriteLine($"DateTimeOffset: {dto}");
//Console.WriteLine($"UTC DateTimeOffset: {dto.UtcDateTime}");

//var IndiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
//var IndiaDateTime = TimeZoneInfo.ConvertTimeFromUtc(dto.UtcDateTime, IndiaTimeZone);
//Console.WriteLine("Action complete at {0} IST", IndiaDateTime);

var dateOnly = new DateOnly(2023, 6, 15);
Console.WriteLine($"DateOnly: {dateOnly}");

var dateOnlyNow = DateOnly.FromDateTime(DateTime.Now);

string dd = "01-01-2003";

var theDateOnly = DateOnly.ParseExact(dd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
Console.WriteLine($"The date you entered is: {theDateOnly}");

var timenow = TimeOnly.FromDateTime(DateTime.Now);
Console.WriteLine($"Time now: {timenow}");

//Date comparison

var date1 = new DateOnly(2023, 6, 15);
var date2 = new DateOnly(2023, 6, 20);

Console.WriteLine($"Is {nameof(date1)} equal? {date1==date2}");
Console.WriteLine($"Is {nameof(date1)} equal? {date1.Equals(date2)}");
Console.WriteLine($"Is {nameof(date1)} after? {date1>date2}");
