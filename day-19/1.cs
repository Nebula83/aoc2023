using System.IO;
using System.Text.RegularExpressions;

class Day1
{
    private Dictionary<string, Worklow> workflows = new Dictionary<string, Worklow>();
    private List<Dictionary<char, int>> parts = new List<Dictionary<char, int>>();

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
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }

        return lines;
    }

    private void ParseFile(string fileName)
    {
        var lines = ReadFile(fileName);
        var index = lines.FindIndex(l => string.IsNullOrEmpty(l));
        ParseWorkflows(lines.Take(index).ToList());
        ParseParts(lines.Skip(index + 1).ToList());
    }

    private void ParseWorkflows(List<string> workflowLines)
    {
        var containerRegex = new Regex(@"^([a-z]+){(.+),([a-zAR]+)}");
        var rulesRegex = new Regex(@"([xmas])([><])(\d+):([a-zAR]+),?");

        workflows = new Dictionary<string, Worklow>();
        foreach (var line in workflowLines)
        {
            var container = containerRegex.Match(line);
            var rules = rulesRegex.Matches(container.Groups[2].Value);
            var ruleList = new List<Rule>();

            foreach (Match rule in rules)
            {
                ruleList.Add(new Rule
                {
                    Type = rule.Groups[1].Value[0],
                    Compare = rule.Groups[2].Value[0] == '<'
                        ? (i, v) => { return i < v; }
                    : (i, v) => { return i > v; },
                    Value = int.Parse(rule.Groups[3].Value),
                    Refer = rule.Groups[4].Value,
                });
            }

            var workflow = new Worklow
            {
                Name = container.Groups[1].Value,
                LastRule = container.Groups[3].Value,
                Rules = ruleList,
            };
            workflows.Add(workflow.Name, workflow);
        }
    }

    private void ParseParts(List<string> partsLines)
    {
        parts = new List<Dictionary<char, int>>();
        var partRegex = new Regex(@"^{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
        foreach (var line in partsLines)
        {
            var part = partRegex.Match(line);
            parts.Add(new Dictionary<char, int>{
                {'x', int.Parse(part.Groups[1].Value)},
                {'m', int.Parse(part.Groups[2].Value)},
                {'a', int.Parse(part.Groups[3].Value)},
                {'s', int.Parse(part.Groups[4].Value)},
            });
        }
    }

    private int GetTotalRatings()
    {
        var sum = 0;
        foreach (var part in parts)
        {
            var currentWorkflow = workflows["in"];
            while (true)
            {
                var refer = currentWorkflow.Process(part);
                if (refer == "A")
                {
                    sum += part['x'] + part['m'] + part['a'] + part['s'];
                    break;
                }
                else if (refer == "R")
                {
                    break;
                }
                currentWorkflow = workflows[refer];
            }
        }
        return sum;
    }

    internal static void Run()
    {
        var day = new Day1();
        // day.ParseFile("test-1.txt");
        day.ParseFile("input.txt");

        var result = day.GetTotalRatings();

        Console.WriteLine($"Result 1: {result}");
    }

}