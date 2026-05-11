using Humanizer;

DateTime now = DateTime.Now;
Console.WriteLine("Hello, World!");
Console.WriteLine("The current time is: " + now);

//get days until christmas
DateTime christmasDate = new DateTime(now.Year, 12, 25);
//if christmas is already passed, set it to next year
if (now > christmasDate)
{
    christmasDate = christmasDate.AddYears(1);
}
//calculate days until christmas
int daysUntilChristmas = (christmasDate - now).Days;
Console.WriteLine("There are " + daysUntilChristmas + " Days until Christmas.");

Console.WriteLine("Case".ToQuantity(1)); // Case
Console.WriteLine("Case".ToQuantity(2)); // Cases
Console.WriteLine("Case".ToQuantity(5)); // Cases