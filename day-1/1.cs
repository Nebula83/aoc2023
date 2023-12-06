using System.IO;

class Day1
{
    private List<string> readFile(string name)
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
        }

        return lines;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day1.readFile("test-1.txt");
        var lines = day.readFile("input.txt");

        int result = 0;
        foreach (var line in lines)
        {
            var first = line.SkipWhile(c=>!char.IsDigit(c)).Take(1).ToList()[0];
            var last = line.Reverse().SkipWhile(c=>!char.IsDigit(c)).Take(1).ToList()[0];

            int value = 10 * (int)(char.GetNumericValue(first)) + (int)(char.GetNumericValue(last));
            
            result += value;
        }
        Console.WriteLine($"Result 1.1: {result}");
    }
}