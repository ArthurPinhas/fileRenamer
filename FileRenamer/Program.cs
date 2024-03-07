using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a folder path as an argument.");
            return;
        }

        string folderPath = args[0];
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Folder not found.");
            return;
        }

        try
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                DateTime creationDate = fileInfo.CreationTime;
                DateTime lastModifiedDate = fileInfo.LastWriteTime;

                DateTime renameDate = (creationDate < lastModifiedDate) ? creationDate : lastModifiedDate;

                string fileType = Path.GetExtension(filePath);
                string newFileName = $"{renameDate:yyyy-MM-dd_HH-mm-ss}{fileType}";

                string directory = fileInfo.DirectoryName;
                string newFilePath = Path.Combine(directory, newFileName);

                // If the file already exists, create a copy with a unique name
                if (File.Exists(newFilePath))
                {
                    int copyNumber = 1;
                    string copyFileName = $"copy_{copyNumber}_{renameDate:yyyy-MM-dd_HH-mm-ss}{fileType}";
                    string copyFilePath = Path.Combine(directory, copyFileName);

                    // Find a unique copy name
                    while (File.Exists(copyFilePath))
                    {
                        copyNumber++;
                        copyFileName = $"copy_{copyNumber}_{renameDate:yyyy-MM-dd_HH-mm-ss}{fileType}";
                        copyFilePath = Path.Combine(directory, copyFileName);
                    }

                    // Rename the file to the unique copy name
                    File.Move(filePath, copyFilePath);
                    Console.WriteLine($"File renamed successfully: {copyFileName}");
                }
                else
                {
                    // Move the file to the new path
                    File.Move(filePath, newFilePath);
                    Console.WriteLine($"File renamed successfully: {newFileName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}
