using System.IO;

class Day1
{
    private List<string> ReadFile(string name)
    {
        List<string> lines = new List<string>();
        string? line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(name);
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                //write the line to console window
                lines.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }

        return lines;
    }

    internal static void Run()
    {
        var day = new Day1();
        var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("input.txt");

        // Console.WriteLine($"Result 1: {result}");
    }
}