using System;
using TempElementsLib;

class Program
{
    static void Main()
    {
        using (var tempFile = new TempFile())
        {
            tempFile.AddText("Hello, temporary file!"); 
            Console.WriteLine($"File is created: {tempFile.FileInfo.FullName}");
        }
        Console.WriteLine("File was deleted after using.");

        var tempFile2 = new TempFile();
        tempFile2.AddText("Test Dispose."); 
        tempFile2.Dispose();
        try
        {
            tempFile2.AddText("Should throw an exception."); 
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }

        var tempFile3 = new TempFile();
        try
        {
            tempFile3.AddText("Test without using.");
        }
        finally
        {
            tempFile3.Dispose();
        }

        Console.ReadLine();
    }
}