using System.IO;
using Microsoft.VisualBasic;

class Day1
{
    List<Race> GetTestRaces()
    {
        // Time:      7  15   30
        // Distance:  9  40  200
        return new List<Race>
        {
            new Race { DurationMs = 7,  RecordMm = 9,   },
            new Race { DurationMs = 15, RecordMm = 40,  },
            new Race { DurationMs = 30, RecordMm = 200, },
        };
    }

    List<Race> GetRealRaces()
    {
        // Time:        41     96     88     94
        // Distance:   214   1789   1127   1055
        return new List<Race>
        {
            new Race { DurationMs = 41, RecordMm = 214,  },
            new Race { DurationMs = 96, RecordMm = 1789, },
            new Race { DurationMs = 88, RecordMm = 1127, },
            new Race { DurationMs = 94, RecordMm = 1055, },
        };
    }

    // distance = holdTime * (duration - holdTime))
    // h = abc formula = (-d +/- sqrt(d^2-4r)/(2))
    (int, int) GetExtremes(Race race)
    {
        long duration = race.DurationMs;
        long record =  race.RecordMm + 1; // + 1 since it needs to be bigger

        if (((duration * duration) - 4 * record) <= 0)
        {
            throw new ArgumentOutOfRangeException("There is no real solution to the given parameters");
        }

        double positive = (duration + Math.Sqrt((duration * duration)-4 * record))/2;
        double negative = (duration - Math.Sqrt((duration * duration)-4 * record))/2;

        var results = new List<double>{positive, negative};
        int atLeast = (int)Math.Ceiling(results.Min());
        int atMost = (int)Math.Floor(results.Max());

        return (atMost, atLeast);
    }

    internal static void Run()
    {
        var day = new Day1();
        // var races = day.GetTestRaces();
        var races = day.GetRealRaces();

        var totalWins = new List<int> ();
        var margin = 1;
        foreach (var race in races)
        {
            var (atLeast, atMost) = day.GetExtremes(race);
            int raceWins = atLeast - atMost + 1; // + 1 because zero counts as well)
            totalWins.Add(raceWins);
            margin *= raceWins; 
        }

        Console.WriteLine($"Result 2: {margin}");
    }
}