using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection.Metadata;

partial class Day1
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
    private static List<Node> GetCandidates(Node node, int[,] blocks)
    {
        var nodes = new List<Node>();
        foreach (var (direction, position) in node.Position.GetNeighbors(blocks.GetLength(0), blocks.GetLength(1)))
        {
            nodes.Add(new Node{
                Position = position,
            });

        }
        return nodes;
    }

    internal static void Run()
    {
        var day = new Day1();
        var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("test-2.txt");
        // var lines = day.ReadFile("input.txt");

        // Convert the input strings into integers
        var blocks = new int[lines.Count, lines[0].Length];
        for (int row = 0; row < lines.Count; row++)
        {
            for (int column = 0; column < lines[0].Length; column++)
            {
                blocks[row, column] = int.Parse($"{lines[row][column]}");
            }
        }

        var result = Dijkstra(blocks);

        Console.WriteLine($"Result 1: {result}");
    }

    private static int Dijkstra(int[,] blocks)
    {
        // Start at top left corner
        var startPoint = new Node
        {
            Position = new Position(0, 0)
        };

        // Stop at bottom right corner
        Position destination = new Position(blocks.GetLength(0) - 1, blocks.GetLength(1) - 1);


        // lol. Convert node to heat loss
        var heatMap = new Dictionary<Node, int>
        {
            { startPoint, 0 },
        };

        // Node with heat loss ordered by heat loss
        var queue = new PriorityQueue<(Node, int), int>();
        foreach (var node in heatMap.Keys)
        {
            queue.Enqueue((node, 0), 0);
        }

        while (queue.Count > 0)
        {
            var (node, heatLoss) = queue.Dequeue();
            if (node.Position == destination)
            {
                return heatMap[node];
            }

            var candidates = GetCandidates(node, blocks);
            foreach (var candidate in candidates)
            {
                var newHeatLoss = heatLoss + blocks[candidate.Position.Row, candidate.Position.Column];
                var currentHeatLoss = heatMap.GetValueOrDefault(candidate, int.MaxValue);
                if (newHeatLoss < currentHeatLoss)
                {
                    heatMap[candidate] = newHeatLoss;
                    queue.Enqueue((candidate, newHeatLoss), newHeatLoss);
                }
            }
        }

        throw new Exception();
    }
}
