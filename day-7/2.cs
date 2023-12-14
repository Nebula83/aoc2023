using System.IO;
using System.Reflection.Metadata;

partial class Day2
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

        // Find Jokers
        var joker = pile.Where(c => c.Card == 'J').SingleOrDefault();
        var jokerCount = 0;
        if (joker != null)
        {
            jokerCount = joker.Count;
            pile.Remove(joker);
        }
        
        int firstCount = 0;
        int secondCount = 0;

        if (pile.Count > 0)
        {
            firstCount = pile[0].Count;
        }
        if (pile.Count > 1)
        {
            secondCount = pile[1].Count;
        }

        firstCount += jokerCount;
        
        var retval = HandType.HighCard;
        if (firstCount == 5)
        {
            retval = HandType.Five;
        }
        else if (firstCount == 4)
        {
            retval = HandType.Four;
        }
        else if (firstCount == 3 && secondCount == 2)
        {
            retval = HandType.FullHouse;
        }
        else if (firstCount == 3)
        {
            retval = HandType.Three;
        }
        else if (firstCount == 2 && secondCount == 2)
        {
            retval = HandType.TwoPair;
        }
        else if (firstCount == 2)
        {
            retval = HandType.OnePair;
        }

        return retval;
    }

    List<Hand> ParseHands(List<string> lines)
    {
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            var hand = new Hand(true)
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
        var day = new Day2();
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

        Console.WriteLine($"Result 2: {score}");
    }
}