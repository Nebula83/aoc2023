using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

class Day1
{
    private Dictionary<string, IModule> _modules = new Dictionary<string, IModule>();
    private Conjunction? _rxModule = null;
    private List<int> _periods = new List<int>();
    private int previousPeriodCount = 0;

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

    private void ParseFile(string filename)
    {
        var lines = ReadFile(filename);
        var parseRegex = new Regex(@"(?:(broadcaster)|(?:([%&])([a-z]+))) -> ([a-z, ]+)");

        foreach (var line in lines)
        {
            var matches = parseRegex.Match(line);

            var broadcaster = !string.IsNullOrEmpty(matches.Groups[1].Value);
            var type = matches.Groups[2].Value;
            var name = matches.Groups[3].Value;
            var destinations = matches.Groups[4].Value.Split(',').Select(t => t.Trim()).ToList();

            if (broadcaster)
            {
                _modules.Add("broadcaster", new Broadcaster(_modules, destinations)
                {
                    Name = "broadcaster",
                });
            }
            else if (type == "%")
            {
                _modules.Add(name, new FlipFlop(_modules, destinations)
                {
                    Name = name
                });
            }
            else if (type == "&")
            {
                _modules.Add(name, new Conjunction(_modules, destinations)
                {
                    Name = name
                });
            }
        }

        foreach (var (key, module) in _modules)
        {
            foreach (var destination in module.Destinations)
            {
                if (_modules.ContainsKey(destination))
                    _modules[destination].AddInput(key);


                // Part 2, upcasting is awesome. Fight me.
                if (destination == "rx" && module is Conjunction)
                {
                    _rxModule = module as Conjunction;
                }
            }
        }

        if (_rxModule == null)
        {
            throw new Exception("No RX module found");
        }
    }

    private void PressButton()
    {
        var workQueue = new Queue<IModule>();
        _modules["broadcaster"].SendPulse(PulseType.Low, "button");
        _modules["broadcaster"].StorePulse(PulseType.Low);
        workQueue.Enqueue(_modules["broadcaster"]);
        // Console.WriteLine("button -low-> broadcaster");

        while (workQueue.Count > 0)
        {
            var currentModule = workQueue.Dequeue();
            var destinations = currentModule.ProcessPulse();

            // Nothing to do here
            if (destinations == null) continue;

            foreach (var destination in destinations)
            {
                if (_modules.ContainsKey(destination))
                {
                    workQueue.Enqueue(_modules[destination]);
                }
            }
        }
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

    private long UpdateRxPeriods(int buttonCount)
    {
        var result = 0L;

        if (_rxModule != null)
        {
            var periodCount = _rxModule.FoundPeriodCount();
            if (periodCount != previousPeriodCount)
            {
                _periods.Add(buttonCount);
                previousPeriodCount = periodCount;
            }

            if (_periods.Count == _rxModule.Periods.Count)
            {
                long lhs = _periods[0];
                for (int position = 1; position < _periods.Count(); position++)
                {
                    long rhs = _periods[position];
                    lhs = lcm(lhs, rhs);
                }
                result = lhs;
            }
        }

        return result;
    }

    internal static void Run()
    {
        var day = new Day1();
        // day.ParseFile("test-1.txt");
        // day.ParseFile("test-2.txt");
        day.ParseFile("input.txt");

        for (int i = 0; i < 1000; i++)
        {
            day.PressButton();
        }
        Console.WriteLine($"Result 1: {Module.PulseProduct()}");


        var result2 = 0L;
        for (int buttonCount = 1; result2 == 0; buttonCount++)
        {
            day.PressButton();
            // We already did 1000 above
            result2 = day.UpdateRxPeriods(buttonCount + 1000);
        }

        Console.WriteLine($"Result 2: {result2}");
    }
}
