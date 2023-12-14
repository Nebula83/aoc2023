using System.IO;

partial class Day1
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

    HandType GetHandType(List<char> cards)
    {
        var pile = cards
                    .GroupBy(c => c)
                    .Select(g => new {Count = g.Count(), Card = g.Key})
                    .OrderByDescending(x => x.Count)
                    .ToList();
                    
        if (pile[0].Count == 5)
        {
            return HandType.Five;
        }
        if (pile[0].Count == 4)
        {
            return HandType.Four;
        }
        if (pile[0].Count == 3 && pile[1].Count == 2)
        {
            return HandType.FullHouse;
        }
        if (pile[0].Count == 3)
        {
            return HandType.Three;
        }
        if (pile[0].Count == 2 && pile[1].Count == 2)
        {
            return HandType.TwoPair;
        }
        if (pile[0].Count == 2)
        {
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }

    List<Hand> ParseHands(List<string> lines)
    {
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            var hand = new Hand
            {
                Bid = int.Parse(line.Substring(6)),
            };
            for (int i = 0; i < 5; i++)
            {
                hand.Cards.Add(line[i]);
            }
            hand.Type = GetHandType(hand.Cards);
            hands.Add(hand);
        }

        return hands;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        var hands = day.ParseHands(lines);
        hands.Sort();

        var score = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            var s = hands[i].Bid * (i + 1);
            score += s;
        }

        Console.WriteLine($"Result 1: {score}");
    }
}