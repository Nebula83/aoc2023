using System.IO;

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
            throw;
        }

        return lines;
    }

    private List<(long, int, long)> ParseLines(List<string> lines, long expansion)
    {
        // Vertical expansion. Store the expansions in a temporary to prevent a
        // shift from being added to a shifted location.
        var horizontalUniverseExpansion = 0L;
        var galaxies = new List<(long, int, long)>();
        var columns = new HashSet<int>();

        for (var row = 0; row < lines.Count; row++)
        {
            var foundGalaxy = false;
            for (var column = 0; column < lines[0].Length; column++)
            {
                var spacePoint =  lines[row][column];
                if (spacePoint == '#')
                {
                    foundGalaxy = true;
                    columns.Add(column);
                    galaxies.Add((row + horizontalUniverseExpansion, column, 0L));
                }
            }
            
            if (!foundGalaxy)
            {
                horizontalUniverseExpansion += (expansion - 1);
            }
        }

        for (var column = 0; column < lines[0].Length; column++)
        {
            if (!columns.Contains(column))
            {
              for (int galaxy = 0; galaxy < galaxies.Count; galaxy++)
              {
                var (r, c, v) = galaxies[galaxy];
                if (c > column)
                {
                    galaxies[galaxy] = (r, c, v + (expansion - 1));
                }
              }
            }
        }

        return galaxies;
    }

    public static long absDiff(long lhs, long rhs)
    {
        if (lhs > rhs) return lhs - rhs;
        return rhs - lhs;
    }

    private long GetDistances(List<(long, int, long)> galaxies)
    {
        // Loop over all combinations
        var result = 0L;
        for (int lhs = 0; lhs < galaxies.Count; lhs++)
        {
            for (int rhs = lhs; rhs < galaxies.Count; rhs++)
            {
                if (lhs != rhs)
                {
                    var a = galaxies[lhs];
                    var b = galaxies[rhs];

                    var dx = absDiff((b.Item2 + b.Item3), (a.Item2 + a.Item3));
                    var dy = absDiff(b.Item1, a.Item1);

                    result += (dx + dy);
                }
            }
        }

        return result;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        
        var galaxies1 = day.ParseLines(lines, 2);
        var result1 = day.GetDistances(galaxies1);

        var galaxies2 = day.ParseLines(lines, 1000000);
        var result2 = day.GetDistances(galaxies2);

        Console.WriteLine($"Result 1: {result1}");
        Console.WriteLine($"Result 2: {result2}"); 
    }
}