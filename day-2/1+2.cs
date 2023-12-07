using System.IO;

class Day1
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

    public (int, List<(int, int, int)>) ParseLine(string line)
    {
        int game = 0;
        var scores = new List<(int, int, int)>();
        
        int gameSeparator = line.IndexOf(':'); 
        game = int.Parse(line.Substring(5, gameSeparator - 5));
        
        var sets = line.Substring(gameSeparator + 2).Split(';');
        foreach(var set in sets)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            var cubes = set.Trim().Split(',');
            foreach( var cube in cubes)
            {
                int colorSeparator = cube.Trim().IndexOf(' '); 
                var count = int.Parse(cube.Trim().Substring(0, colorSeparator));
                if (cube.Contains("red"))
                {
                    red = count;
                }
                else if (cube.Contains("green"))
                {
                    green = count;
                }
                else if (cube.Contains("blue"))
                {
                    blue = count;
                }
            }
            scores.Add((red, green, blue));
        }
        
        return (game, scores);
    }

    public (int, int, int, int) GetHighScores(string line)
    {
        var (game, scores) = ParseLine(line);
        int red = 0, green = 0, blue = 0;
        foreach(var score in scores)
        {
            var (r, g, b) = score;
            red = Math.Max(red, r);
            green = Math.Max(green, g);
            blue = Math.Max(blue, b);
        }

        return (game, red, green, blue);
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.readFile("test.txt");
        var lines = day.readFile("input.txt");

        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;

        int result1 = 0;
        foreach (var line in lines)
        {
            // Console.WriteLine(line);
            var (game, red, green, blue) = day.GetHighScores(line);
            bool possible = (red <= maxRed) && (green <= maxGreen) && (blue <= maxBlue);
            if (possible)
            {
                result1 += game;
            }
            // Console.WriteLine($"game{game} r{red}, g{green}, b{blue}");
        }

        Console.WriteLine($"Result 1: {result1}");

        int result2 = 0;
        foreach (var line in lines)
        {
            // Console.WriteLine(line);
            var (game, red, green, blue) = day.GetHighScores(line);
            result2 += (red *  green* blue);
        }
        Console.WriteLine($"Result 2: {result2}");
    }
}