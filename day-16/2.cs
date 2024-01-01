using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;

class Day2
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
        var day = new Day2();
        // var grid = day.ReadFile("test-1.txt");
        var grid = day.ReadFile("input.txt");

        var results = new List<int>();
        var beamQueue = new Queue<Beam>();
        
        for (int row = 0; row < grid.Count; row++)
        {
            beamQueue.Enqueue(new Beam{
                CurrentDirection = Direction.ToTheRight,
                Row = row,
                Column = 0,
            });
            beamQueue.Enqueue(new Beam{
                CurrentDirection = Direction.ToTheLeft,
                Row = row,
                Column = grid.Count -1,
            });
        }

        for (int column = 0; column < grid[0].Length; column++)
        {
            beamQueue.Enqueue(new Beam{
                CurrentDirection = Direction.Down,
                Row = 0,
                Column = column,
            });
            beamQueue.Enqueue(new Beam{
                CurrentDirection = Direction.Up,
                Row = grid[0].Length - 1,
                Column = column,
            });
        }
        
        while (beamQueue.Count > 0)
        {
            var visited = new bool[grid.Count, grid[0].Length];
            var beams = new List<Beam>
            {
                // Initial beam
                beamQueue.Dequeue()
            };

            var prevResult = 0;
            var stabilized = 0;

            while (beams.Count > 0)
            {
                var deadBeams = new Queue<Beam>();
                var newBeams = new Queue<Beam>();
                foreach (var beam in beams)
                {
                    if ((0 <= beam.Row && beam.Row < grid.Count) 
                        && (0 <= beam.Column && beam.Column < grid[0].Length))
                    {
                        visited[beam.Row, beam.Column] = true;
                        var newBeam = day.UpdateBeam(beam, grid[beam.Row][beam.Column]);
                        if (newBeam != null)
                        {
                            newBeams.Enqueue(newBeam);
                        }
                    }
                    else
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

                // What a horrible construction, but somehow it doesn't end otherwise
                if (prevResult != result)
                {
                    prevResult = result;
                    stabilized = 0;
                }
                else
                {
                    stabilized++;
                }

                if (stabilized > 8)
                {
                    break;
                }
            }

            results.Add(prevResult);
        }

        Console.WriteLine($"Result 2: {results.Max()} ");
    }
}