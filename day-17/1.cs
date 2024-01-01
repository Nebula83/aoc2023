using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection.Metadata;

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
        // var lines = day.ReadFile("test-2.txt");
        // var lines = day.ReadFile("input.txt");

        var nodes = new Node[lines.Count, lines[0].Length];
        for (int row = 0; row < lines.Count; row++)
        {
            for (int column = 0; column < lines[0].Length; column++)
            {
                nodes[row, column] = new Node
                {
                    HeatLoss = int.Parse($"{lines[row][column]}"),
                    Row = row,
                    Column = column,
                };
            }
        }

        // Start point
        var start = nodes[0, 0];
        start.Distance = 0;
        start.Direction = Direction.Xnknown;

        var nodeQueue = new PriorityQueue<Node, int>();
        nodeQueue.Enqueue(start, start.Distance);
        
        while (nodeQueue.Count > 0)
        {
            var currentNode = nodeQueue.Dequeue();

            var candidates = GetNeighbors(nodes, currentNode);
            foreach (var (direction, candidate) in candidates)
            {
                if (currentNode.Steps == 3 && currentNode.Direction == direction)
                    continue;

                // Andere boeg, zelfde resultaat.
                // if (candidate.IsWithinUnidirectionalDistance(currentNode))
                {
                    var newDistance = currentNode.Distance + candidate.HeatLoss;
                    if (newDistance < candidate.Distance)
                    {
                        candidate.Distance = newDistance;
                        candidate.Parent = currentNode;
                        candidate.Direction = direction;
                       
                        // If the direction of travel is the same: raise the step count. Otherwise reset.
                        if (candidate.Direction == currentNode.Direction)
                        {
                            candidate.Steps = currentNode.Steps + 1;
                        }
                        else 
                        {
                            candidate.Steps = 1;
                        }
                        
                        nodeQueue.Enqueue(candidate, candidate.Distance);
                    }
                }
            }
            // currentNode.Visited = true;
        }

        // Fetch the result
        var destination = nodes[nodes.GetLength(0) - 1, nodes.GetLength(1) - 1];

        // Mark the road traveled (debugging)
        var grid = new bool[nodes.GetLength(0), nodes.GetLength(1)];
        var current = destination;
        grid[current.Row, current.Column] = true;
        while (current.Row != 0 || current.Column != 0)
        {
            current = current.Parent;
            grid[current.Row, current.Column] = true;
        } 

        // Plot the grid (debugging)
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                // if (grid[row, column]) Console.Write($"{nodes[row, column].Steps:000}");
                // if (grid[row, column]) Console.Write(nodes[row, column].Direction.ToString()[0]);
                // if (grid[row, column]) Console.Write($"{nodes[row, column].Distance:000}");
                // Console.Write($"{nodes[row, column].Steps} ");
                // Console.Write($"{nodes[row, column].Distance:000} ");
                if (grid[row, column]) Console.Write($"{nodes[row, column].UnidirectionalDistance():000}");
                // if (grid[row, column]) Console.Write($"{nodes[row, column].UnidirectionalDistance():00} ");
                // if (grid[row, column]) Console.Write("#");
                // else Console.Write(lines[row][column]);
                else Console.Write(" . ");
                // else Console.Write(".");
                // else Console.Write("   ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.WriteLine($"Result 1: {destination.Distance}");
    }

    private static List<(Direction, Node)> GetNeighbors(Node[,] nodes, Node currentNode)
    {
        var candidates = new List<(Direction, Node)>();
        if (currentNode.Row >= 1)
        {
            var node = nodes[currentNode.Row - 1, currentNode.Column]; 
            if (!node.Visited) candidates.Add((Direction.Up, node));
        }
        if (currentNode.Row < nodes.GetLength(0) - 1)
        {
            var node = nodes[currentNode.Row + 1, currentNode.Column];
            if (!node.Visited) candidates.Add((Direction.Down, node));
        }
        if (currentNode.Column >= 1)
        {
            var node = nodes[currentNode.Row, currentNode.Column - 1];
            if (!node.Visited) candidates.Add((Direction.Left, node));
        }
        if (currentNode.Column < nodes.GetLength(1) - 1)
        {
            var node = nodes[currentNode.Row, currentNode.Column + 1];
            if (!node.Visited) candidates.Add((Direction.Right, node));
        }

        return candidates;
    }


    class Node
    {
        const int MAX_SINGLE_DIRECTION = 3;

        public bool Visited { get; set; } = false;
        public Node? Parent { get; set; } =  null;
        public int Distance { get; set; } = int.MaxValue;
        public int Steps { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Xnknown;
        public int HeatLoss { get; set; } = 0;
        public int Row { get; set; }
        public int Column { get; set; }

        public bool IsOnLocation(int row, int column)
        {
            return Row == row && Column == column;
        }

        internal bool IsWithinUnidirectionalDistance(Node current)
        {
            var unidirectionalDistance = 0;
            if (current.Column == Column)
            {
                var parent = current.Parent;
                unidirectionalDistance++;
                while (Column == parent?.Column)
                {
                    unidirectionalDistance++;
                    parent = parent.Parent;
                }
            }

            else if (current.Row == Row)
            {
                var parent = current.Parent;
                unidirectionalDistance++;
                while (Row == parent?.Row)
                {
                    unidirectionalDistance++;
                    parent = parent.Parent;
                }
            }

            return unidirectionalDistance <= MAX_SINGLE_DIRECTION;
            // return true;
        }
        
        internal int UnidirectionalDistance()
        {
            var unidirectionalDistance = 0;
            if (Parent != null && Parent.Column == Column)
            {
                var parent = Parent;
                while (Column == parent?.Column)
                {
                    unidirectionalDistance++;
                    parent = parent.Parent;
                }
            }

            else if (Parent != null && Parent.Row == Row)
            {
                var parent = Parent;
                while (Row == parent?.Row)
                {
                    unidirectionalDistance++;
                    parent = parent.Parent;
                }
            }

            return unidirectionalDistance;
        }
    }
}