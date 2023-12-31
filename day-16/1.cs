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
    private Beam? UpdateBeam(Beam beam, char cell)
    {
        Beam? newBeam = null;
        switch (cell)
        {
            case '.':
                switch (beam.CurrentDirection)
                {
                    case Direction.ToTheLeft:
                        beam.Column--;
                        break;
                    case Direction.ToTheRight:
                        beam.Column++;
                        break;
                    case Direction.Down:
                        beam.Row++;
                        break;
                    case Direction.Up:
                        beam.Row--;
                        break;
                }
                break;
            case '/': 
                switch (beam.CurrentDirection)
                {
                    case Direction.ToTheLeft:
                        beam.CurrentDirection = Direction.Down;
                        beam.Row++;
                        break;
                    case Direction.ToTheRight:
                        beam.CurrentDirection = Direction.Up;
                        beam.Row--;
                        break;
                    case Direction.Down:
                        beam.CurrentDirection = Direction.ToTheLeft;
                        beam.Column--;
                        break;
                    case Direction.Up:
                        beam.CurrentDirection = Direction.ToTheRight;
                        beam.Column++;
                        break;
                }
                break;
            case '\\': 
                switch (beam.CurrentDirection)
                {
                    case Direction.ToTheLeft:
                        beam.CurrentDirection = Direction.Up;
                        beam.Row--;
                        break;
                    case Direction.ToTheRight:
                        beam.CurrentDirection = Direction.Down;
                        beam.Row++;
                        break;
                    case Direction.Down:
                        beam.CurrentDirection = Direction.ToTheRight;
                        beam.Column++;
                        break;
                    case Direction.Up:
                        beam.CurrentDirection = Direction.ToTheLeft;
                        beam.Column--;
                        break;
                }
                break;
            case '|': 
                switch (beam.CurrentDirection)
                {
                    case Direction.ToTheLeft:
                        newBeam = new Beam
                        {
                            CurrentDirection = Direction.Up,
                            Row = beam.Row - 1,
                            Column = beam.Column,
                        };
                        beam.CurrentDirection = Direction.Down;
                        beam.Row++;
                        break;
                    case Direction.ToTheRight:
                        newBeam = new Beam
                        {
                            CurrentDirection = Direction.Up,
                            Row = beam.Row - 1,
                            Column = beam.Column,
                        };
                        beam.CurrentDirection = Direction.Down;
                        beam.Row++;
                        break;
                    case Direction.Down:
                        beam.Row++;
                        break;
                    case Direction.Up:
                        beam.Row--;
                        break;
                }
                break;
            case '-': 
                switch (beam.CurrentDirection)
                {
                    case Direction.ToTheLeft:
                        beam.Column--;
                        break;
                    case Direction.ToTheRight:
                        beam.Column++;
                        break;
                    case Direction.Down:
                        newBeam = new Beam
                        {
                            CurrentDirection = Direction.ToTheLeft,
                            Row = beam.Row,
                            Column = beam.Column - 1,
                        };
                        beam.CurrentDirection = Direction.ToTheRight;
                        beam.Column++;
                        break;
                    case Direction.Up:
                        newBeam = new Beam
                        {
                            CurrentDirection = Direction.ToTheLeft,
                            Row = beam.Row,
                            Column = beam.Column - 1,
                        };
                        beam.CurrentDirection = Direction.ToTheRight;
                        beam.Column++;
                        break;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException($"{cell}");
        }

        return newBeam;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var grid = day.ReadFile("test-1.txt");
        var grid = day.ReadFile("input.txt");
        var visited = new bool[grid.Count, grid[0].Length];
        var beams = new List<Beam>();

        // Initial beam
        beams.Add(new Beam
        {
            CurrentDirection = Direction.ToTheRight,
            Row = 0,
            Column = 0,
        });

        var prevResult = 0;
        var stabilized = 0;
        while (beams.Count > 0)
        {
            var deadBeams = new Queue<Beam>();
            var newBeams = new Queue<Beam>();
            foreach (var beam in beams)
            {
                try
                {
                    visited[beam.Row, beam.Column] = true;
                    var newBeam = day.UpdateBeam(beam, grid[beam.Row][beam.Column]);
                    if (newBeam != null)
                    {
                        newBeams.Enqueue(newBeam);
                    }
                }
                // Beam walked off the screen, remove it
                catch (IndexOutOfRangeException)
                {
                    deadBeams.Enqueue(beam);
                }
            }

            while (newBeams.Count > 0)
            {
                beams.Add(newBeams.Dequeue());
            }

            while (deadBeams.Count > 0)
            {
                beams.Remove(deadBeams.Dequeue());
            }

            var result = 0;
            foreach (var cell in visited)
            {
                if (cell)
                {
                    result++;
                }
            }

            if (prevResult != result)
            {
                prevResult = result;
                stabilized = 0;
            }
            else
            {
                stabilized++;
            }

            if (stabilized > 10)
            {
                break;
            }
        }

        Console.WriteLine($"Result 1: {prevResult}");
    }
}