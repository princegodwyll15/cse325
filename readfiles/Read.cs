File.ReadAllText($"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales.json");

// check if file exists returns true or false
bool filesExists = File.Exists("filepath");

// create a file and write text to it
var newFile = File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "newfile.txt"), "This is a new file created by the program");


//list all sales files in the store directory
foreach (var filename in salesFiles)
{
    Console.WriteLine(filename);
}
//prepare total sales data to insert in itd file
var salesJson = File.ReadAllText($"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales.json");

//convert json to object
var data = JsonConvert.DeserializeObject<SalesTotal>(salesJson);

//write the json data to file
File.WriteAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", data.Total.ToString());

//add new lines to the totals.txt file in salesTotalDir
File.AppendAllText($"{salesDirectory}{Path.DirectorySeparatorChar}totals.txt", $"{data.Total}{Environment.NewLine}");

//create a new file in the sales directory
File.WriteAllText(Path.Combine(salesDirectory, "total.txt"), String.Empty);

//create a sales dto 
class SalesTotal
{
    public double Total { get; set; }
}
