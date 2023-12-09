using System.IO;
using System.Text.RegularExpressions;


class Card
{
    public int Number { get; set; }
    public List<int> Winning { get; set; } = new List<int>();
    public List<int> Given { get; set; } = new List<int>();

}
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

    internal Card ParseLine(string line)
    {
        int pastNumberPosition = line.IndexOf(':');
        int separatorPosition = line.IndexOf('|');
    
        var winning = line.Substring(pastNumberPosition + 1, separatorPosition - (pastNumberPosition + 1))
                        .Trim();
        var given = line.Substring(separatorPosition + 1)
                        .Trim();
        
        var digits = new Regex(@"(\d+)");
        return new Card{
            Number = int.Parse(line.Substring(4, pastNumberPosition - 4)),
            Winning = digits.Matches(winning).Select(m => int.Parse(m.Groups[0].Value)).ToList(),
            Given = digits.Matches(given).Select(m => int.Parse(m.Groups[0].Value)).ToList(),
        };
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        var cards = lines.Select(l => day.ParseLine(l)).ToList();

        var result = cards.Select(c =>  (int)Math.Pow(2, c.Given.Intersect(c.Winning).Count() - 1)).Sum();

        Console.WriteLine($"Result 1: {result}");
    }
}