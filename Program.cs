using System;
using System.IO;

class Program
{
    static void Main()
    {
        string sourcePath = "C:\\SourceFolder";
        string replicaPath = "C:\\ReplicaFolder";

        // Create folders if they don't exist
        if (!Directory.Exists(sourcePath) || !Directory.Exists(replicaPath))
        {
            Directory.CreateDirectory(sourcePath);
            Directory.CreateDirectory(replicaPath);
            Console.WriteLine("\nSourceFolder and ReplicaFolder created at " + sourcePath + " and " + replicaPath + " respectively.");
        }
        else
        {
            Console.WriteLine("\nSourceFolder and ReplicaFolder already exist at " + sourcePath + " and " + replicaPath + " respectively.");
        }

        // Set up FileSystemWatchers
        FileSystemWatcher sourceWatcher = new FileSystemWatcher(sourcePath);
        sourceWatcher.IncludeSubdirectories = true; // Watch subfolders as well
        sourceWatcher.NotifyFilter =
            NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
        sourceWatcher.Created += OnChanged;
        sourceWatcher.Deleted += OnChanged;
        sourceWatcher.Renamed += OnRenamed;
        sourceWatcher.Changed += OnChanged;
        sourceWatcher.EnableRaisingEvents = true; // Start monitoring
        

        FileSystemWatcher replicaWatcher = new FileSystemWatcher(replicaPath);
        replicaWatcher.IncludeSubdirectories = true;
        replicaWatcher.NotifyFilter =
            NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
        replicaWatcher.Created += OnChanged;
        replicaWatcher.Deleted += OnChanged;
        replicaWatcher.Renamed += OnRenamed;
        replicaWatcher.Changed += OnChanged;
        replicaWatcher.EnableRaisingEvents = true; // Start monitoring

        while (true) // Main loop for the menu
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Create a file in SourceFolder");
            Console.WriteLine("2. Delete a file in SourceFolder");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue; // Go back to the beginning of the loop
            }

            switch (choice)
            {
                case 1:
                    CreateFile(sourcePath); 
                    break;
                case 2:
                    DeleteFile(sourcePath);
                    break;
                case 3:
                    return; // Exit the program
                default:
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    break;
            }
        }
    }

    static void CreateFile(string folderPath)
    {
        Console.Write("Enter file name (with extension): ");
        string fileName = Console.ReadLine();
        string filePath = Path.Combine(folderPath, fileName);

        try
        {
            using (File.Create(filePath)) { } // Create an empty file
            Console.WriteLine($"File '{fileName}' created successfully in SourceFolder.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error creating file: {e.Message}");
        }
    }

    static void DeleteFile(string folderPath)
    {
        Console.Write("Enter file name (with extension): ");
        string fileName = Console.ReadLine();
        string filePath = Path.Combine(folderPath, fileName);

        try
        {
            File.Delete(filePath);
            Console.WriteLine($"File '{fileName}' deleted successfully from SourceFolder.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error deleting file: {e.Message}");
        }
    }


    static void OnChanged(object sender, FileSystemEventArgs e)
    {
        string eventType = e.ChangeType.ToString(); // Get event type (Created, Deleted, Changed)
        Console.WriteLine($"{eventType}: {e.FullPath}");
    }

    static void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"Renamed: {e.OldFullPath} to {e.FullPath}");
    }
}
