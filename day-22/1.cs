using System.IO;

class Day1
{
    protected List<Brick> _bricks = new();

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
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }

        return lines;
    }

    internal static void Run()
    {
        var day = new Day1();
        // day.ParseFile("test-1.txt");
        // day.ParseFile("test-2.txt");
        day.ParseFile("input.txt");

        day.DropItLikeItsHot();

        var result = day._bricks.Sum(b => {
            // Check if this brick supports other bricks
            foreach (var above in b.Supports)
            {
                // If this brick supports another brick AND
                // that other brick has nothing else supporting
                // it, skip this brick. It cannot be removed.
                if (above.SupportedBy.Count == 1)
                {
                    return 0;
                }
            }
            return 1;
        });

        Console.WriteLine($"Result 1: {result}");
    }

    protected void DropItLikeItsHot()
    {
        var height = _bricks.Max(b => b.HighestZ);
        var xWidth = _bricks.Max(b => b.HighestX);
        var yWidth = _bricks.Max(b => b.HighestY);

        foreach (var brick in _bricks)
        {
            var newZ = 1;
            var lower = GetLowerBricks(brick);

            foreach (var target in lower)
            {
                var candidateZ = target.HighestZ + 1;
                if (brick.WouldCollide(target))
                {
                    // Update all bricks in the colliding layer, but stop if we move past it.
                    if (newZ > candidateZ)
                    {
                        break;
                    }
                    // Update new position
                    newZ = candidateZ;
                    // Update supporting bricks
                    target.Supports.Add(brick);
                    brick.SupportedBy.Add(target);
                }
            }

            brick.Drop(newZ);
        }
    }
    private Brick[] GetLowerBricks(Brick brick)
    {
        return _bricks
        .Where(b => (b.LowestZ <= brick.HighestZ) && (brick != b))
        .OrderByDescending(b => b.HighestZ)
        .ToArray();
    }

    protected void ParseFile(string filename)
    {
        var lines = ReadFile(filename);
        int index = 0;
        foreach (var line in lines)
        {
            var coords = line.Split('~');
            var toCoord = (string s) =>
            {
                var coord = s.Split(',');
                return new Coord
                (
                    int.Parse(coord[0]),
                    int.Parse(coord[1]),
                    int.Parse(coord[2])
                );
            };
            _bricks.Add(new Brick(toCoord(coords[0]), toCoord(coords[1]))
            {
                Index = index++,
            });
        }

        _bricks = _bricks.OrderBy(b => b.LowestZ).ToList();
    }
}
