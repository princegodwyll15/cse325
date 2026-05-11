using System.IO;
//list all directories in the current directory
IEnumerable<string> dir = Directory.EnumerateDirectories("stores");

foreach (string d in dir)
{
    Console.WriteLine(d);
}

//list all files in the current directory
IEnumerable<string> files = Directory.EnumerateFiles("stores");

foreach (string f in files)
{
    Console.WriteLine(f);
}

//all content of the current directory and subdirectories
IEnumerable<string> all = Directory.EnumerateFiles("stores", "*.txt", SearchOption.AllDirectories);
foreach (string d in all)
{
    Console.WriteLine(d);
}