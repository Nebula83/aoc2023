using System.IO;

class Day1
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
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        RollRocksNorth(lines);
        
        var tally = 0;
        for (int row = 0; row < lines.Count; row++)
        {
            var multiplier = lines.Count - row;
            tally += multiplier * lines[row].Where(c => c == 'O').Count();
        }

        Console.WriteLine($"Result 1: {tally}");
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
}