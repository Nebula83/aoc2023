using System.IO;
using System.Runtime.InteropServices;

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

    private List<(string, List<int>)> ParseLines(List<string> lines)
    {
        var data = new List<(string, List<int>)>();
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var broken = parts[1].Split(',').Select(s => int.Parse(s)).ToList();

            data.Add((parts[0], broken));
        }

        return data;
    }

    internal bool IsOption(string input, IList<int> rhs)
    {
        var lhs = input
                    .Split('.')
                    .Where(r => !string.IsNullOrEmpty(r))
                    .Select(c => c.Length).ToArray();

        return IsEqual(lhs, rhs);
    }

    internal bool IsEqual(IList<int> lhs, IList<int> rhs)
    {
        var match = false;
        if (lhs.Count() == rhs.Count())
        {
            match = true;
            for (int index = 0; index < lhs.Count(); index++)
            {
                match &= lhs[index] == rhs[index];
                if (!match)
                {
                    break;
                }
            }
        }
        return match;
    }

    private static IEnumerable<string> GetAllOptions(string conditions)
    {
        // Find all indexes of the unknowns
        var foundIndexes = new List<int>();
        for (int index = conditions.IndexOf('?'); index > -1; index = conditions.IndexOf('?', index + 1))
        {
            foundIndexes.Add(index);
        }

        // Loop through all the options
        var options = Math.Pow(2, foundIndexes.Count);
        for (int option = 0; option < options; option++)
        {
            var tryme = conditions.ToCharArray();
            for (int index = 0; index < foundIndexes.Count; index++)
            {
                tryme[foundIndexes[index]] = (option & (1 << index)) > 0 ? '.' : '#';
            }

            yield return new string(tryme);
        }
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        var options = day.ParseLines(lines);


        var result  = 0;
        foreach (var (record, broken) in options)
        {
            // Console.WriteLine($"{record}");
            foreach (var variation in GetAllOptions(record))
            {
                // Console.WriteLine(variation);
                if (day.IsOption(variation, broken))
                {
                    result++;
                }
            }
        }

        Console.WriteLine($"Result 1: {result}");
    }
}
