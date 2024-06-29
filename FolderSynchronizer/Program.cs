using System;
using CommandLine;
using CommandLine.Text;

class Options
{
    [Option('s', "source", Required = true, HelpText = "The path to the source folder.")]
    public string SourceFolder { get; set; }

    [Option('r', "replica", Required = true, HelpText = "The path to the replica folder.")]
    public string ReplicaFolder { get; set; }

    [Option('l', "log", Required = true, HelpText = "The path to the log file.")]
    public string LogFile { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = Console.Error);
        var parserResult = parser.ParseArguments<Options>(args);

        parserResult
            .WithParsed(options =>
            {
                // Validate and create directories/log file
                CreateIfNotExist(options.SourceFolder, "Source");
                CreateIfNotExist(options.ReplicaFolder, "Replica");

                // Ensure the log file's directory exists
                var logDirectory = Path.GetDirectoryName(options.LogFile);
                CreateIfNotExist(logDirectory, "Log");

            })
            .WithNotParsed(errors =>
            {
                // Parsing errors
            });
    }

     static void CreateIfNotExist(string path, string folderType)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"{folderType} folder did not exist and was created at: {path}");
        }
        else
        {
            Console.WriteLine($"{folderType} folder already exists at: {path}");
        }
    }
}
