using System.IO;
using System.Text.RegularExpressions;
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

    List<char> ParseInstructions(string line)
    {
        return line.ToList();
    }
    
    Dictionary<string, Node> ParseNodes(List<string> lines)
    {
        var result = new  Dictionary<string, Node> ();
        var nodesRegex = new Regex(@"^(.{3}) = \((.{3}), (.{3})\)");
        foreach (var line in lines)
        {
            var matches = nodesRegex.Matches(line); 
            var name = matches[0].Groups[1].Value;
            var node = new Node { 
                        Name = name,
                        Left = matches[0].Groups[2].Value,
                        Right = matches[0].Groups[3].Value,
                    };
            result.Add(name, node);
        }

        return result;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("test-2.txt");
        var lines = day.ReadFile("input.txt");
        var instructions = day.ParseInstructions(lines[0]);
        var nodes = day.ParseNodes(lines.Skip(2).ToList());

        var currentNode = nodes["AAA"];
        var result = 0;
        while (currentNode.Name != "ZZZ")
        {
            foreach (var step in instructions)
            {
                switch (step)
                {
                    case 'L': currentNode = nodes[currentNode.Left]; break;
                    case 'R': currentNode = nodes[currentNode.Right]; break;
                    default: throw new InvalidOperationException("Wazda?");
                }
                result++;
                // Console.WriteLine($"CurrentNode {currentNode.Name}");
            }
        }

        Console.WriteLine($"Result 1: {result}");
    }
}
