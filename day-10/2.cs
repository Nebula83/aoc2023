using System.IO;
using System.Net.Http.Headers;

class Day2 : Day1
{
    int RayTrace(List<List<Tile>> grid)
    {
        int filled = 0;
        for (var row = 0; row < grid.Count; row++)
        {
            for (var column = 0; column < grid[0].Count; column++)
            {
                var tile = grid[row][column];
                
                // not part of the tubes
                if (!tile.Visited)
                {
                    int intersections = 0;
                    for (var scan = column + 1; scan < grid[0].Count; scan++)
                    {      
                        var point = grid[row][scan];
                        
                        // Shoot a ray, and count the edges. 
                        if (
                            point.Visited 
                            && (point.Type == '|' || point.Type == 'F' || point.Type == '7'  || point.Type == 'S' )
                            )
                        {
                            intersections++;
                        }
                    }
                    
                    // If the number of edges is odd, the point is in the loop
                    //              F-------------------7
                    //              |        I----------|----> 1
                    //              |                   |
                    //              |                   |
                    //        O-----|-------------------|----> 2
                    //              |                   |
                    //              L-------------------J
                    if ((intersections % 2) != 0)
                    {
                        tile.Filled = true;
                        filled++;
                    }
                }
            }
        }

        return filled;
    }

    internal static new void Run()
    {
        var day = new Day2();
        
        var examples = new List<List<string>>
        {
            // day.ReadFile("test-3.txt"),
            // day.ReadFile("test-4.txt"),
            // day.ReadFile("test-5.txt"),
            day.ReadFile("input.txt"),
        };

        foreach (var lines in examples)
        {
            var (start, grid) = day.ParseGrid(lines);
            if (start == null)
            {
                throw new NullReferenceException("start");
            }

            // Find the tubes
            day.DFS(start, grid);

            // day.ShowGrid(grid);
            var filled = day.RayTrace(grid);
            // day.ShowGrid(grid);
            
        
            Console.WriteLine($"Result 2: {filled}");
        }
    }
}
