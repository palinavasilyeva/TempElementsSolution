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

        Console.WriteLine("\n== TempTxtFile demo ==");

        using (var tempTxt = new TempTxtFile())
        {
            tempTxt.WriteLine("Test string");
            Console.WriteLine("Read: " + tempTxt.ReadLine());
        }

        Console.WriteLine("\n== TempDir demo ==");
        using (var tempDir = new TempDir())
        {
            Console.WriteLine($"Temporary directory created at: {tempDir.DirectoryInfo.FullName}");

            string newFilePath = System.IO.Path.Combine(tempDir.DirectoryInfo.FullName, "testfile.txt");
            System.IO.File.WriteAllText(newFilePath, "Hello from temp dir!");

            Console.WriteLine($"File created inside temp dir: {newFilePath}");
            Console.WriteLine("File content:");
            Console.WriteLine(System.IO.File.ReadAllText(newFilePath));
        }
        Console.WriteLine("Temporary directory was deleted after using.");

        Console.WriteLine("\n== TempElementsList demo ==");

        using (var tempList = new TempElementsList())
        {
            tempList.AddElement<TempFile>();
            tempList.AddElement<TempDir>();

            foreach (var element in tempList.Elements)
            {
                if (element is TempFile tf)
                    Console.WriteLine($"TempFile in list: {tf.FileInfo.FullName}");
                else if (element is TempDir td)
                    Console.WriteLine($"TempDir in list: {td.DirectoryInfo.FullName}");
            }

            tempList.Dispose();
        }

        Console.WriteLine("TempElementsList disposed, all elements cleaned up.");


        Console.ReadLine();
    }
}
