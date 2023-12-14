using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Schema;
class Day2
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
    static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }

    internal static void Run()
    {
        var day = new Day2();
        // var lines = day.ReadFile("test-3.txt");
        var lines = day.ReadFile("input.txt");
        var instructions = day.ParseInstructions(lines[0]);
        var nodes = day.ParseNodes(lines.Skip(2).ToList());

        var startingPoints = nodes.Keys.Where(n => n.EndsWith('A')).ToList();

        var currentNodes = new List<Node>();
        var periods = new List<int>();
        foreach (var startingPoint in startingPoints)
        {
            currentNodes.Add(nodes[startingPoint]);
            periods.Add(0);
        }

        var result = 0;
        while (periods.Where(p => p != 0).Count() != startingPoints.Count())
        {
            foreach (var step in instructions)
            {
                for (int nodeIndex = 0; nodeIndex < currentNodes.Count; nodeIndex++)
                {
                    currentNodes[nodeIndex] = step switch
                    {
                        'L' => nodes[currentNodes[nodeIndex].Left],
                        'R' => nodes[currentNodes[nodeIndex].Right],
                        _ => throw new InvalidOperationException("Wazda?"),
                    };
                    
                    if (currentNodes[nodeIndex].Name.EndsWith('Z'))
                    {
                        periods[nodeIndex] = result + 1;
                    }
                }
                result++;
            }
        }

        // Commence the LCM
        periods.Sort();
        long lhs = periods[0];
        for (int position = 1; position < periods.Count; position++)
        {
            long rhs = periods[position];
            lhs = lcm(lhs, rhs);
        }

        Console.WriteLine($"Result 2: {lhs}");
    }
}
