using System.IO;

class Day2
{
    private List<char[]> ReadFile(string name)
    {
        List<char[]> lines = new List<char[]>();
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
                lines.Add(line.ToCharArray());
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
    
    internal static void Run()
    {
        var day = new Day2();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        const long CYCLES = 1000000000; 
        var tallies = new List<int>();
        var pattern = new List<int>();
        var patternStart = -1L;

        // Establish pattern
        for (var count = 0L; true; count++)
        {
            RollRocksNorth(lines);
            RollRocksWest(lines);
            RollRocksSouth(lines);
            RollRocksEast(lines);

            var tally = 0;
            for (int row = 0; row < lines.Count; row++)
            {
                var multiplier = lines.Count - row;
                tally += multiplier * lines[row].Where(c => c == 'O').Count();
            }

            var last = tallies.LastIndexOf(tally);
            tallies.Add(tally);
            if (last != -1)
            {
                var repeat = (int)(count - last);
                pattern.Add(tally);
                patternStart = count;

                if (pattern.Count > 2 
                    // Wait until the pattern repeats itself
                    && pattern.Take(pattern.Count / 2).SequenceEqual(pattern.Skip(pattern.Count/2)))
                {
                    // Found the pattern, stop the search
                    break;
                }
            }
            else
            {
                pattern.Clear();
                patternStart = -1;
            }
        }

        var index = (int)((CYCLES - 1 - patternStart - 1) % pattern.Count);
        Console.WriteLine($"Result 2: {pattern[index]}");
    }

    private static void RollRocksNorth(List<char[]> lines)
    {
        bool rocksMoved;
        do
        {
            rocksMoved = false;
            for (int row = 1; row < lines.Count; row++)
            {
                for (int rockIndex = 0; rockIndex < lines[0].Length; rockIndex++)
                {
                    if (lines[row][rockIndex] == 'O')
                    {
                        if (lines[row - 1][rockIndex] == '.')
                        {
                            rocksMoved = true;
                            lines[row - 1][rockIndex] = 'O';
                            lines[row][rockIndex] = '.';
                        }
                    }
                }
            }

            // ShowRocks(lines);
        } while (rocksMoved);
    }

    private static void RollRocksSouth(List<char[]> lines)
    {
        bool rocksMoved;
        do
        {
            rocksMoved = false;
            for (int row = lines.Count - 2; row >= 0; row--)
            {
                for (int rockIndex = 0; rockIndex < lines[0].Length; rockIndex++)
                {
                    if (lines[row][rockIndex] == 'O')
                    {
                        if (lines[row + 1][rockIndex] == '.')
                        {
                            rocksMoved = true;
                            lines[row + 1][rockIndex] = 'O';
                            lines[row][rockIndex] = '.';
                        }
                    }
                }
            }

            // ShowRocks(lines);
        } while (rocksMoved);
    }
    
    private static void RollRocksEast(List<char[]> lines)
    {
        bool rocksMoved;
        do
        {
            rocksMoved = false;
            for (int rockIndex = lines[0].Length - 2; rockIndex >= 0; rockIndex--)
            {
                for (int row = 0; row < lines.Count; row++)
                {
                    if (lines[row][rockIndex] == 'O')
                    {
                        if (lines[row][rockIndex + 1] == '.')
                        {
                            rocksMoved = true;
                            lines[row][rockIndex + 1] = 'O';
                            lines[row][rockIndex] = '.';
                        }
                    }
                }
            }

            // ShowRocks(lines);
        } while (rocksMoved);
    }
    
    private static void RollRocksWest(List<char[]> lines)
    {
        bool rocksMoved;
        do
        {
            rocksMoved = false;
            for (int rockIndex = 1; rockIndex < lines[0].Length; rockIndex++)
            {
                for (int row = 0; row < lines.Count; row++)
                {
                    if (lines[row][rockIndex] == 'O')
                    {
                        if (lines[row][rockIndex - 1] == '.')
                        {
                            rocksMoved = true;
                            lines[row][rockIndex - 1] = 'O';
                            lines[row][rockIndex] = '.';
                        }
                    }
                }
            }

            // ShowRocks(lines);
        } while (rocksMoved);
    }
}