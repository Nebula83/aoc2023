using System.IO;

class Day2
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

    private Dictionary<int, Tuple<string, string>> GetIndexes(string line, bool reversed = false)
    {
        var retval = new Dictionary<int, Tuple<string, string>>();
        var conversion = new Dictionary <string, string> {
            {"one",   "1"},
            {"two",   "2"},
            {"three", "3"},
            {"four",  "4"},
            {"five",  "5"},
            {"six",   "6"},
            {"seven", "7"},
            {"eight", "8"},
            {"nine",  "9"},
        };

        foreach (var (text, number) in conversion)
        {
            // TODO find the lowest and highest index where a number matches, replace the lowest and highest index
            // translated = translated.Replace(text, number);
            var index = reversed ? line.LastIndexOf(text) : line.IndexOf(text);
            if (index != -1)
            {
                retval.Add(index, new Tuple<string, string>(text, number));
            }
        }

        return retval;
    } 

    private List<string> ReplaceNumbers(List<string> raw)
    {
        var lines = new List<string>();

        foreach (var line in raw)
        {
            var translated = line;

            // Ask for forgiveness, not permission like it's python.
            try
            {
                // Replace the first text number
                var (lowIndex, (lowText, lowNumber)) = GetIndexes(translated).OrderBy(item => item.Key).FirstOrDefault();
                // Insert the number before, but leave the text
                translated = translated.Insert(lowIndex, lowNumber);


                // Replace the last text number
                var (highIndex, (highText, highNumber)) = GetIndexes(translated, true).OrderByDescending(item => item.Key).FirstOrDefault();
                // Insert the number after, but leave the text
                translated = translated.Insert(highIndex + highText.Length, highNumber);
            }
            catch(System.NullReferenceException)
            {}

            lines.Add(translated);
        }
        
        return lines;
    }

    internal static void Run()
    {
        var day = new Day2();
        // var raw = day.readFile("test-2.txt");
        var raw = day.readFile("input.txt");

        var lines = day.ReplaceNumbers(raw);

        int result = 0;
        foreach (var line in lines)
        {
            var first = line.SkipWhile(c=>!char.IsDigit(c)).Take(1).ToList()[0];
            var last = line.Reverse().SkipWhile(c=>!char.IsDigit(c)).Take(1).ToList()[0];
            int value = 10 * (int)(char.GetNumericValue(first)) + (int)(char.GetNumericValue(last));
            
            result += value;
        }
        Console.WriteLine($"Result 1.2: {result}");

    }
}