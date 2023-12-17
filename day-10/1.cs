using System.IO;
using System.Net.Http.Headers;

class Day1
{
    protected List<string> ReadFile(string name)
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

    protected (Tile?, List<List<Tile>>) ParseGrid(List<string> lines)
    {
        Tile? start = null;
        var grid = new List<List<Tile>>();

        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            var gridRow = new List<Tile>();
            for (var column = 0; column < line.Length; column++)
            {
                var tile = new Tile(row, column, line[column]);
                if ( line[column] == 'S')
                {
                    start = tile;
                }
                gridRow.Add(tile);
            }
            grid.Add(gridRow);
        }

        return (start, grid);
    }

    public void ShowGrid(List<List<Tile>> grid)
    {
        Console.WriteLine("");
        for (var row = 0; row < grid.Count; row++)
        {
            for (var column = 0; column < grid[0].Count; column++)
            {
                var tile = grid[row][column];

                if (tile.Filled)
                {
                    Console.Write('I');
                }
                else if (tile.Visited)
                {
                    Console.Write(tile.Type);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine("");
        }
    }

    private List<Tile> GetEdges(Tile tile, List<List<Tile>> grid)
    {
        var edges = new List<Tile> ();
        
        if ((tile.Row - 1) >= 0) 
        {
            var edge = grid[tile.Row - 1][tile.Column];
            if (tile.NorthOpen && edge.SouthOpen)
            {
                edges.Add(edge);
            }
        } 
        if ((tile.Column + 1) < grid[0].Count)
        {
            var edge = grid[tile.Row][tile.Column + 1];
            if (tile.EastOpen && edge.WestOpen)
            {
                edges.Add(edge);
            }

        }
        if ((tile.Row + 1) < grid.Count)
        {
            var edge = grid[tile.Row + 1][tile.Column];
            if (tile.SouthOpen && edge.NorthOpen)
            {
                edges.Add(edge);
            }
        }
        if ((tile.Column - 1) >= 0) 
        {
            var edge = grid[tile.Row][tile.Column - 1];
            if (tile.WestOpen && edge.EastOpen)
            {
                edges.Add(edge);
            }
        } 

        return edges;
    }

    protected int DFS(Tile start, List<List<Tile>> grid)
    {
        start.Visited = true;

        var edges = GetEdges(start, grid);
        var unvisitedCount = edges.Where(t => !t.Visited).Count();

        int distance = 0;
        foreach (var edge in edges)
        {
            if (!edge.Visited)
            {
                edge.Parent = start;
                edge.Distance = start.Distance + 1;
                distance = DFS(edge, grid);   
            }
            else
            {
                // Detect the end of the loop
                if (start.Parent != edge)
                {
                    return edge.Distance + 1;
                }
            }
        }

        return distance;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("test-2.txt");
        var lines = day.ReadFile("input.txt");
        var (start, grid) = day.ParseGrid(lines);
        if (start == null)
        {
            throw new NullReferenceException("start");
        }

        var distance = day.DFS(start, grid);


        Console.WriteLine($"Result 1: {distance / 2}");
    }

    /*
        | is a vertical pipe connecting north and south.
        - is a horizontal pipe connecting east and west.
        L is a 90-degree bend connecting north and east.
        J is a 90-degree bend connecting north and west.
        7 is a 90-degree bend connecting south and west.
        F is a 90-degree bend connecting south and east.
        . is ground; there is no pipe in this tile.
        S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
    */
}
