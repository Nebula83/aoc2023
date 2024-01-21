using System.IO;
using Microsoft.VisualBasic;

class Day1
{
    private List<List<bool>> _grid = new();
    private Point _start = new(-1, -1);
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

    private void ParseFile(string filename)
    {
        var lines = ReadFile(filename);
        for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
        {
            var line = lines[lineIndex];
            _grid.Add(new List<bool>());
            for (int columnIndex = 0; columnIndex < line.Length; columnIndex++)
            {
                var start = () => { _start = new(lineIndex, columnIndex); return true; };
                bool value = lines[lineIndex][columnIndex] switch
                {
                    '#' => false,
                    '.' => true,
                    'S' => start(),
                    _ => throw new NotSupportedException(),
                };
                _grid[lineIndex].Add(value);
            }
        }
    }

    private int FindPlotCount(int stepCount)
    {
        var visited = new List<Point>();
        var work = new Queue<Node>();
        var evenPlots = new List<Point>();
        var oddPlots = new List<Point>();

        work.Enqueue(new Node(_start, 0));
        while (work.Count > 0)
        {
            var node = work.Dequeue();

            if (node.Steps % 2 == 0)
            {
                evenPlots.Add(node.Point);
            }
            else
            {
                oddPlots.Add(node.Point);
            }

            if (node.Steps > stepCount)
            {
                continue;
            }

            foreach (var neighbor in Point.Neighbors)
            {
                var candidate = _grid.GetPoint(node.Point, neighbor);
                if (candidate != null)
                {
                    var newNode = new Node(candidate, node.Steps + 1);
                    if (!visited.Contains(candidate) && _grid.IsPlot(candidate))
                    {
                        visited.Add(candidate);
                        work.Enqueue(newNode);
                    }
                }
            }
        }


        // I battled for quite a bit trying to get my solve time below the
        // "days" count, so I budged and found this piece of advice:

        // > Initially I decided to store (location, steps taken) in my seen
        // > state, since there are multiple ways to arrive at the same
        // > location. But I wanted to allow for backtracking, because we're
        // > allowed to go back to locations we've been to before. However, I
        // > then realised that I can simply store every location previously
        // > visited. Why is this true? It is true because:
        // > - If we arrive at any location and we have an even number of steps
        // >   remaining, then we can always get back to this location on our
        // >   last step. So if this condition is met, this location is a valid
        // >   final location. 
        // > - If we arrive at any location and we have an odd number of steps
        // >   remaining, then there's no way to get back to this location on
        // >   our last step. That's because for every n steps we move away from
        // >   our current location, we need to move n steps back for this
        // >   location to one of solution locations. But 2n will always be an
        // >   even number. There's no way to travel 2n squares with an odd
        // >   number of steps remaining. 
        // > - So to conclude: any location we visit will either be a valid
        // >   solution location, or impossible to reach on our last step. In
        // >   either case, we can mark it as seen and never revisit. 
        
        // So in my own words: because we need to incorporate backtracking (a
        // position can be visited many times) in order to get to an answer, AND
        // we can't find that using brute force because we would just keep
        // jumping and forth indefinitely, we need a trick. The trick being that
        // if we walk forward n steps we need to walk back n steps, therefore
        // only even steps are relevant. The same holds for uneven steps, since
        // it is the same case with an offset of one. Maybe if the start
        // position was not zero steps, we'd need to offset for that.

        int result;
        if (stepCount % 2 == 0)
        {
            result = evenPlots.Count - 1;
        }
        else
        {
            result = oddPlots.Count;
        }
        return result;
    }

    internal static void Run()
    {
        int result;
        var day = new Day1();
        // day.ParseFile("test-1.txt");
        day.ParseFile("input.txt");

        result = day.FindPlotCount(64);
        Console.WriteLine($"Result 1: {result}");
    }
}

public record class Node
{
    public Point Point { get; private set; }
    public int Steps { get; set; }

    public Node(Point point, int steps)
    {
        Point = point;
        Steps = steps;
    }
}

public record class Point
{
    public static readonly Point[] Neighbors = {
        new (0, -1),
        new (0,  1),
        new (1,  0),
        new (-1, 0),
    };

    public Point(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; private set; }
    public int Column { get; private set; }
}

public static class GridExtensions
{
    public static bool IsPlot(this List<List<bool>> grid, Point pixel)
    {
        return grid[pixel.Row][pixel.Column];
    }

    public static Point? GetPoint(this List<List<bool>> grid, Point start, Point diff)
    {
        var row = start.Row + diff.Row;
        var column = start.Column + diff.Column;

        // If within the grid
        if (0 <= row && row < grid.Count
         && 0 <= column && column < grid[0].Count)
        {
            return new Point(row, column);
        }

        return null;
    }
}