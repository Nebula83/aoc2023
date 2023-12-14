using System.IO;
using Microsoft.VisualBasic;

class Day2
{
    Race GetTestRace()
    {
        // Time:      7  15   30
        // Distance:  9  40  200
        return 
            new Race { DurationMs = 71530,  RecordMm = 940200 };
    }

    Race GetRealRace()
    {
        // Time:        41     96     88     94
        // Distance:   214   1789   1127   1055
        return 
            new Race { DurationMs = 41968894, RecordMm = 214178911271055 };
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
        var day = new Day2();
        // var race = day.GetTestRace();
        var race = day.GetRealRace();

        var (atLeast, atMost) = day.GetExtremes(race);
        int raceWins = atLeast - atMost + 1; // + 1 because zero counts as well)

        Console.WriteLine($"Result 1: {raceWins}");
    }
}