using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;


var currentDirectory = Directory.GetCurrentDirectory();
var storeDirectory = Path.Combine(currentDirectory, "stores");
var salesDirectory = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesDirectory);

var salesFiles = FindFiles(storeDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);

//append sales total to totals.txt file in salesTotalDir
File.AppendAllText($"{salesDirectory}{Path.DirectorySeparatorChar}totals.txt", $"{salesTotal}{Environment.NewLine}");

//create sales summary file in salesTotalDir
var salesSummaryData = $"Sales Summary" + Environment.NewLine
+ "----------------------------------" + Environment.NewLine
+ "Total Sales: " + salesTotal + Environment.NewLine
+ "Details:" + Environment.NewLine + string.Join(Environment.NewLine, salesFiles);


File.AppendAllText($"{salesDirectory}{Path.DirectorySeparatorChar}sales_summary.txt", $"{salesSummaryData}");
Console.WriteLine($"Total sales: {salesTotal}");

//function to find sales.json file in directory
IEnumerable<string> FindFiles(string folderName)
{
    List<string> files = new List<string>();
    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
    foreach (var file in foundFiles)
    {
        var extention = Path.GetExtension(file);
        if (extention == ".json")
        {
            files.Add(file);
        }
    }
    return files;
}

//function to calculate sales total from sales.json files
double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        salesTotal += data?.Total ?? 0;
    }
    return salesTotal;
}

record SalesData(double Total);





//using string builder to create sales summary report
// var report = new StringBuilder();
// report.AppendLine("Sales Summary");
// report.AppendLine("----------------------------------");
// report.AppendLine($"Total Sales: {salesTotal}");
// report.AppendLine("Details:");
// report.AppendLine(string.Join(Environment.NewLine, salesFiles));