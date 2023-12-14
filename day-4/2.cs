using System.IO;
using System.Text.RegularExpressions;

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

    internal Card ParseLine(string line)
    {
        int pastNumberPosition = line.IndexOf(':');
        int separatorPosition = line.IndexOf('|');
    
        var winning = line.Substring(pastNumberPosition + 1, separatorPosition - (pastNumberPosition + 1))
                        .Trim();
        var given = line.Substring(separatorPosition + 1)
                        .Trim();
        
        var digits = new Regex(@"(\d+)");
        var card = new Card{
            Number = int.Parse(line.Substring(4, pastNumberPosition - 4)),
            Winning = digits.Matches(winning).Select(m => int.Parse(m.Groups[0].Value)).ToList(),
            Given = digits.Matches(given).Select(m => int.Parse(m.Groups[0].Value)).ToList()
        };
        card.Winnings = Enumerable.Range(start: card.Number + 1, card.Given.Intersect(card.Winning).Count()).ToList();

        return card;
    }

    private int GetCardCount(Card card, List<Card> cards)
    {
        int count = 1;
        foreach (var win in card.Winnings)
        {
            // Convert the game number into an index into the table
            count += GetCardCount(cards[win - 1], cards);
        } 

        return count;
    }

    internal static void Run()
    {
        var day = new Day2();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        var cards = lines.Select(l => day.ParseLine(l)).ToList();

        var result = 0;
        foreach (var card in cards)
        {
            result += day.GetCardCount(card, cards);
        }

        Console.WriteLine($"Result 2: {result}");
    }
}