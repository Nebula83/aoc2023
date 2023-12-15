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
        }

        return lines;
    }

    private List<List<long>> ParseSequences(List<string> lines)
    {
        var sequences = new List<List<long>>();

        foreach (var line in lines)
        {
            sequences.Add(line.Split(' ').Select(c => long.Parse(c)).ToList());
        }

        return sequences;
    }

    private List<long> GetDiffList(List<long> sequence)
    {
        var result = new List<long>();
        var previous = sequence[0];

        for (int index = 1; index < sequence.Count; index++)
        {
            result.Add(sequence[index] - previous);
            previous = sequence[index];
        }

        return result;
    }

    private long Extrapolate(List<long> sequence)
    {
        var diffs = new List<List<long>>();
        var diffList = sequence;
        while(true)
        {
            diffs.Add(diffList);
            diffList = GetDiffList(diffList);
            // BUG! HAHAHA Sneaky bastards :-D
            // if (diffList.Sum() == 0)
            if (diffList.All(i => i == 0))
            {
                break;
            }
        }
        
        // Extrapolate value
        long increment = 0;
        for (var diffIndex = diffs.Count - 1 ; diffIndex >= 0; diffIndex--)
        {
            var diff = diffs[diffIndex];
            increment += diff[diff.Count - 1];
        }

        return increment;
    }
    
    private long ExtrapolateBack(List<long> sequence)
    {
        var diffs = new List<List<long>>();
        var diffList = sequence;
        while(true)
        {
            diffs.Add(diffList);
            diffList = GetDiffList(diffList);
            if (diffList.All(i => i == 0))
            {
                break;
            }
        }
        
        // Extrapolate value
        long increment = 0;
        for (var diffIndex = diffs.Count - 1; diffIndex >= 0; diffIndex--)
        {
            var diff = diffs[diffIndex];
            increment = diff[0] - increment;
        }

        return increment;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        var sequences = day.ParseSequences(lines);

        var result = 0L;
        var resultBack = 0L;
        foreach (var sequence in sequences)
        {
            var extrapolated = day.Extrapolate(sequence);
            var extrapolatedBack = day.ExtrapolateBack(sequence);
            result += extrapolated;
            resultBack += extrapolatedBack;
        }

        Console.WriteLine($"Result 1: {result}");
        Console.WriteLine($"Result 2: {resultBack}");
    }
}