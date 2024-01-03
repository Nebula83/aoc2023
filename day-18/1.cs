using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

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

    internal static void Run()
    {
        var day = new Day1();
        var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("input.txt");

        var start = new Hole
        {
            Coord = new Coord(0, 0),
            EdgeColor = "",
        };

        // Gather all the holes
        var current = start;
        var holes = new List<Hole>{start};
        var points = new List<Hole>{start};
        foreach (var line in lines)
        {
            var lineRegex = new Regex(@"([LURD]) (\d+) \((#[0-9a-f]{6})\)");
            var matches = lineRegex.Match(line);
            var direction = DirectionExtensions.Parse(matches.Groups[1].Value[0]);
            var count = int.Parse(matches.Groups[2].Value);
            var edgeColor = matches.Groups[3].Value;

            if (count == 0)
            {
                throw new IndexOutOfRangeException(nameof(count));
            }

            Hole? newHole = null;
            for (int step = 0; step < count; step++)
            {
                newHole = new Hole
                {
                    Coord = current.Coord.Move(direction),
                    EdgeColor = edgeColor,
                };
                holes.Add(newHole);
                current = newHole;
            }
        }

        // Find extremes so we can translate to a 0,0 origin
        var minX = holes.Select(h => h.Coord.X).Min();
        var maxX = holes.Select(h => h.Coord.X).Max();
        
        var minY = holes.Select(h => h.Coord.Y).Min();
        var maxY = holes.Select(h => h.Coord.Y).Max();
        
        // Find the overlap
        var grid = new bool[maxX - minX + 1, maxY - minY + 1];
        foreach (var hole in holes)
        {
            var x = hole.Coord.X - minX;
            var y = hole.Coord.Y - minY;

            grid[x, y] =  true;;
        }

        Coord overlap = new Coord(-minX, -minY);

        // Flood fill
        var queue = new Queue<Coord>();
        queue.Enqueue(new Coord(overlap.X + 1, overlap.Y + 1));
        var neighborList = new List<(int, int)>
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0),
        };
        while (queue.Count > 0)
        {
            var coord = queue.Dequeue();
            // Get neighbors
            foreach (var (dx, dy) in neighborList)
            {
                var newCoord = new Coord(coord.X + dx, coord.Y + dy) ;
                if (0 <= newCoord.X && newCoord.X < grid.GetLength(0)
                    && 0 <= newCoord.Y && newCoord.Y < grid.GetLength(1)
                    && !grid[newCoord.X, newCoord.Y])
                {
                    grid[newCoord.X, newCoord.Y] = true;
                    queue.Enqueue(newCoord);
                }
            }
        }

        var sum = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                // Console.Write(grid[x,y] ? '#' :'.');
                if (grid[x,y])
                {
                    sum++;
                }
            }
            // Console.WriteLine();
        }
        // Console.WriteLine();
       
        // 54552 too low
        Console.WriteLine($"Result 1: {sum}");
    }
}
