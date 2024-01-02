partial class Day1
{
    class Node : IEquatable<Node>
    {
        public Position Position { get; set; }
        public int Steps { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Down;

        // To use the object as an efficient key into the heat map dictionary.
        public bool Equals(Node? other)
        {
            return Position == other?.Position;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Position.GetHashCode() * 17;
            }
        }
    }
}

//     internal static void Run()
//     {
//         var day = new Day1();
//         var lines = day.ReadFile("test-1.txt");
//         // var lines = day.ReadFile("test-2.txt");
//         // var lines = day.ReadFile("input.txt");

//         var nodes = new Node[lines.Count, lines[0].Length];
//         for (int row = 0; row < lines.Count; row++)
//         {
//             for (int column = 0; column < lines[0].Length; column++)
//             {
//                 nodes[row, column] = new Node
//                 {
//                     HeatLoss = int.Parse($"{lines[row][column]}"),
//                     Row = row,
//                     Column = column,
//                 };
//             }
//         }

//         // Start point
//         var start = nodes[0, 0];
//         start.Distance = 0;
//         start.Direction = Direction.Xnknown;

//         var nodeQueue = new PriorityQueue<Node, int>();
//         nodeQueue.Enqueue(start, start.Distance);

//         while (nodeQueue.Count > 0)
//         {
//             var currentNode = nodeQueue.Dequeue();

//             var candidates = GetNeighbors(nodes, currentNode);
//             foreach (var (direction, candidate) in candidates)
//             {
//                 if (currentNode.Steps == 3 && currentNode.Direction == direction)
//                     continue;

//                 // Andere boeg, zelfde resultaat.
//                 // if (candidate.IsWithinUnidirectionalDistance(currentNode))
//                 {
//                     var newDistance = currentNode.Distance + candidate.HeatLoss;
//                     if (newDistance < candidate.Distance)
//                     {
//                         candidate.Distance = newDistance;
//                         candidate.Parent = currentNode;
//                         candidate.Direction = direction;

//                         // If the direction of travel is the same: raise the step count. Otherwise reset.
//                         if (candidate.Direction == currentNode.Direction)
//                         {
//                             candidate.Steps = currentNode.Steps + 1;
//                         }
//                         else 
//                         {
//                             candidate.Steps = 1;
//                         }

//                         nodeQueue.Enqueue(candidate, candidate.Distance);
//                     }
//                 }
//             }
//             // currentNode.Visited = true;
//         }

//         // Fetch the result
//         var destination = nodes[nodes.GetLength(0) - 1, nodes.GetLength(1) - 1];

//         // Mark the road traveled (debugging)
//         var grid = new bool[nodes.GetLength(0), nodes.GetLength(1)];
//         var current = destination;
//         grid[current.Row, current.Column] = true;
//         while (current.Row != 0 || current.Column != 0)
//         {
//             current = current.Parent;
//             grid[current.Row, current.Column] = true;
//         } 

//         // Plot the grid (debugging)
//         for (int row = 0; row < grid.GetLength(0); row++)
//         {
//             for (int column = 0; column < grid.GetLength(1); column++)
//             {
//                 // if (grid[row, column]) Console.Write($"{nodes[row, column].Steps:000}");
//                 // if (grid[row, column]) Console.Write(nodes[row, column].Direction.ToString()[0]);
//                 // if (grid[row, column]) Console.Write($"{nodes[row, column].Distance:000}");
//                 // Console.Write($"{nodes[row, column].Steps} ");
//                 // Console.Write($"{nodes[row, column].Distance:000} ");
//                 if (grid[row, column]) Console.Write($"{nodes[row, column].UnidirectionalDistance():000}");
//                 // if (grid[row, column]) Console.Write($"{nodes[row, column].UnidirectionalDistance():00} ");
//                 // if (grid[row, column]) Console.Write("#");
//                 // else Console.Write(lines[row][column]);
//                 else Console.Write(" . ");
//                 // else Console.Write(".");
//                 // else Console.Write("   ");
//             }
//             Console.WriteLine();
//         }
//         Console.WriteLine();

//         Console.WriteLine($"Result 1: {destination.Distance}");
//     }

//     private static List<(Direction, Node)> GetNeighbors(Node[,] nodes, Node currentNode)
//     {
//         var candidates = new List<(Direction, Node)>();
//         if (currentNode.Row >= 1)
//         {
//             var node = nodes[currentNode.Row - 1, currentNode.Column]; 
//             if (!node.Visited) candidates.Add((Direction.Up, node));
//         }
//         if (currentNode.Row < nodes.GetLength(0) - 1)
//         {
//             var node = nodes[currentNode.Row + 1, currentNode.Column];
//             if (!node.Visited) candidates.Add((Direction.Down, node));
//         }
//         if (currentNode.Column >= 1)
//         {
//             var node = nodes[currentNode.Row, currentNode.Column - 1];
//             if (!node.Visited) candidates.Add((Direction.Left, node));
//         }
//         if (currentNode.Column < nodes.GetLength(1) - 1)
//         {
//             var node = nodes[currentNode.Row, currentNode.Column + 1];
//             if (!node.Visited) candidates.Add((Direction.Right, node));
//         }

//         return candidates;
//     }