using System.IO;
using System.Security.Principal;

class Day1_1
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
        var day = new Day1_1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        var images = day.ParseLines(lines);

        var result = 0;
        foreach (var image in images)
        {
            result += 100 * day.FindMirrorLine(image);
            
            // Transpose
            var columns = new List<string>();
            for (int column = 0; column < image[0].Length; column++)
            {
                var columnString = string.Empty;
                for (int row = 0; row < image.Count; row++)
                {
                    columnString += image[row][column];
                }
                columns.Add(columnString);
            }

            result +=  day.FindMirrorLine(columns);
        }

        Console.WriteLine($"Result 1.1: {result}");
    }

    private int FindMirrorLine(List<string> image)
    {
        for (int lineIndex = 1; lineIndex < image.Count; lineIndex++)
        {
            var top = image.Take(lineIndex).Reverse();
            var bottom = image.Skip(lineIndex);

            var imgDiff = 0;
            foreach (var (lhs, rhs) in top.Zip(bottom))
            {
                imgDiff += Diff(lhs, rhs);
            }

            if (imgDiff == 0)
            {
                // Return the line that produced, off by one
                return lineIndex;
            }
        }

        return 0;
    }

    private int Diff(string lhs, string rhs)
    {
        var diffCount = 0;
        for (var charIndex = 0; charIndex < lhs.Length; charIndex++)
        {
            if (lhs[charIndex] != rhs[charIndex])
            {
                diffCount++;
            }

        }
        return diffCount;
    }

    private List<List<string>> ParseLines(List<string> lines)
    {
        var images = new List<List<string>>();

        var image = new List<string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                images.Add(image);
                image = new List<string>();
            }
            else
            {
                image.Add(line);
            }
        }
        images.Add(image);
        
        return images;
    }
}