using System.Text.RegularExpressions;

class Day2
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
                    Type = rule.Groups[1].Value[0], // xmas
                    Operator = rule.Groups[2].Value[0], 
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

    private long Combinations(Dictionary<char, XmasRange> xmas, string workflowName)
    {
        if (workflowName == "R") return 0;
        else if (workflowName == "A") 
        {
            var combinations  = 1L;
            foreach (var range in xmas.Values)
            {
                combinations *= range.GetRange();
            }
            return combinations;
        }

        var sum = 0L;
        var workflow = workflows[workflowName];
        foreach (var rule in workflow.Rules)
        {
            var range = xmas[rule.Type];
            XmasRange accept;
            XmasRange reject;
            if (rule.Operator == '<')
            {
                accept = new XmasRange(range.Left, rule.Value - 1);
                reject = new XmasRange(rule.Value,  range.Right);
            }
            else if (rule.Operator == '>')
            {
                accept = new XmasRange(rule.Value + 1, range.Right);
                reject = new XmasRange(range.Left, rule.Value);
            }
            else
            {
                throw new InvalidOperationException($"{rule.Operator}");
            }

            // Check if the ranges are valid
            if (accept.IsValid())
            {
                var result = new Dictionary<char, XmasRange>(xmas);
                result[rule.Type] = accept;
                // The rule accepted this range, so venture further
                sum += Combinations(result, rule.Refer);
            }
            if (reject.IsValid())
            {
                xmas = new Dictionary<char, XmasRange>(xmas);
                xmas[rule.Type] = reject;
            }
        }

        sum += Combinations(xmas, workflow.LastRule);

        return sum;
    }

    private long GetFullRange()
    {
        var sum = 0L;

        var ranges = new Dictionary<char, XmasRange>
        {
            {'x', new XmasRange(1, 4000)},
            {'m', new XmasRange(1, 4000)},
            {'a', new XmasRange(1, 4000)},
            {'s', new XmasRange(1, 4000)},
        };
        sum += Combinations(ranges, "in");

        return sum;
    }

    internal static void Run()
    {
        var day = new Day2();
        // day.ParseFile("test-1.txt");
        day.ParseFile("input.txt");

        var result = day.GetFullRange();

        Console.WriteLine($"Result 1: {result}");
    }

}

public class XmasRange
{
    public int Left { get; set; }
    public int Right { get; set; }

    public XmasRange(int left, int right)
    {
        Left = left;
        Right = right;
    }

    public XmasRange()
    {
        Left = 0;
        Right = 0;
    }

    public long GetRange()
    {
        // + 1 because both ends are inclusive (full range is 4000) 
        return Right - Left + 1;
    }

    public bool IsValid()
    {
        return Left <= Right;
    }
}