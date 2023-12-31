using System.IO;
using System.Text;

class Day1
{
    private string ReadFile(string name)
    {
        string line = "";
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(name);
            //Read the first line of text
            line = sr.ReadLine() ?? "";
                //Continue to read until you reach end of file
            //close the file
            sr.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }

        return line;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var line = day.ReadFile("test-1.txt");
        var line = day.ReadFile("input.txt");
        var instructions = line.Split(',');

        var result = 0;
        foreach (var instruction in instructions)
        {
            var hash = GetHash(instruction);
            result += hash;
        }

        Console.WriteLine($"Result 1: {result}");
    }

    private static int GetHash(string instruction)
    {
        var result = 0;

        // Determine the ASCII code for the current character of the string.
        foreach (var c in Encoding.ASCII.GetBytes(instruction))
        {
            // Increase the current value by the ASCII code you just determined.
            result += c;
            // Set the current value to itself multiplied by 17.
            result *= 17;
            // Set the current value to the remainder of dividing itself by 256.
            result %= 256;
        }

        return result;
    }
}