using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Day2_Edges
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
        var day = new Day2_Edges();
        var lines = day.ReadFile("test-1.txt");
        // var lines = day.ReadFile("input.txt");

        var start = new Coord(0, 0);

        // Gather all the holes
        var current = start;
        var points = new List<Coord>{start};
        // var xToYs = new Dictionary<int, List<(int, int)>>();
        var edges = new Dictionary<long, List<Edge>>();

        // var part1 = false;
        var part1 = true;
        // Console.Write("Reading data..");
        Console.Out.Flush();
        foreach (var line in lines)
        {
            var lineRegex = new Regex(@"([LURD]) (\d+) \((#[0-9a-f]{6})\)");
            var matches = lineRegex.Match(line);
            
            Direction direction;
            int count;
            if (part1)
            {
                direction = DirectionExtensions.Parse(matches.Groups[1].Value[0]);
                count = int.Parse(matches.Groups[2].Value);
            }
            else
            {
                var edgeColor = matches.Groups[3].Value;
                count = Convert.ToInt32(edgeColor.Substring(1, 5), 16);
                direction = DirectionExtensions.Parse(int.Parse(edgeColor.Substring(6)));
            }

            if (count == 0)
            {
                throw new IndexOutOfRangeException(nameof(count));
            }

            var newPoint = current.Move(direction, count);
            var corner =  new Edge
            {
                Type = Edge.DirectionsToCornerType(current.Direction, newPoint.Direction),
                Y = current.Y,
            };

            if (!edges.ContainsKey(current.X))
            {
                edges[current.X] = new List<Edge>();
            }
            edges[current.X].Add(corner);
            
            if (current.Y == newPoint.Y)
            {
                var startX = Math.Min(newPoint.X, current.X) + 1;
                var endX = Math.Max(newPoint.X, current.X);

                for (long x = startX ; x < endX; x++)
                {
                    if (!edges.ContainsKey(x))
                    {
                        edges[x] = new List<Edge>();
                    }

                    var edge =  new Edge
                    {
                        Type = EdgeType.Vertical,
                        Y = current.Y,
                    };
                    edges[x].Add(edge);
                }
            }

            points.Add(newPoint);
            current = newPoint;
        }
        // Console.WriteLine(" done");

        // Find extremes so we can  loop over it
        var minX = points.Select(h => h.X).Min();
        var maxX = points.Select(h => h.X).Max();

        // foreach (var key in edges.Keys.Order())
        // {
        //     Console.Write($"{key}: ");
        //     foreach (var edge in edges[key].OrderBy(e => e.Y))
        //     {
        //         Console.Write($"{edge.Y}: {edge.Type} ");
        //     }
        //     Console.WriteLine();
        // }

        // Console.Write("Calculating..");
        Console.Out.Flush();
        var result = 0L;
        // var debugLine = -81;
        // minX = -81;
        // minX = 5;
        for (long line = minX; line <= maxX; line++)
        {
            Console.Write($"{line}: ");


            var orderedEdges = edges[line].OrderBy(e => e.Y).ToList();
            var startCorner = EdgeType.Unknown;
            var skipCorners = 0L;
            // var edgeCount = 0;
            var startY = -1L;
            var sum = 0L;
            // var onEdge = false;
            var inSurface = false;
            // // var onCornerBorder = false;
            foreach (var edge in orderedEdges)
            {
                var edgeType = edge.Type;

                if (edgeType != EdgeType.Vertical)
                {
                    if (startCorner == EdgeType.Unknown)
                    {
                        // Origin edge
                        if (edgeType == EdgeType.Unknown)
                        {
                            edgeType = EdgeType.CornerDown;
                        }
                        if (!inSurface)
                        {
                            inSurface = true;
                            startY = edge.Y;
                        }
                        else
                        {
                            skipCorners--;
                            if (skipCorners == 0)
                            {
                                continue;
                            }
                        }
                        startCorner = edgeType;
                    }
                    // else if ((startCorner != edgeType) == inSurface)
                    else if (startCorner == edgeType)
                    {
                        inSurface = false;
                        // Include endpoint
                        sum += edge.Y - startY + 1;
                    }
                    else
                    {
                        skipCorners++;
                        startCorner = EdgeType.Unknown;
                    }
                }
                else
                {
                    if (!inSurface)
                    {
                        inSurface = true;
                        startY = edge.Y;
                    }
                    else
                    {
                        inSurface = false;
                        // Include endpoint
                        sum += edge.Y - startY + 1;
                    }
                }






                
            //     var edgeType = edge.Type;
            //     if (edgeType != EdgeType.Vertical)
            //     {
            //         if (startCorner == EdgeType.Unknown)
            //         {
            //             if (edgeType == EdgeType.Unknown)
            //             {
            //                 edgeType = EdgeType.CornerDown;
            //             }
            //             startCorner = edgeType;
            //             onEdge = true;
            //         }
            //         // Edge flip
            //         else if (startCorner != edgeType && onEdge)
            //         {
            //             onEdge = false;
            //             // startCorner = EdgeType.Unknown;
            //             startCorner = edgeType;//startCorner == EdgeType.CornerUp ? EdgeType.CornerDown : EdgeType.CornerUp;
            //             // Reset, since it will be added again in the next if
            //             edgeCount = 0;
            //             // sum += edge.Y - startY + 1;
            //             // startY = edge.Y + 1;
            //         }
            //     }

            //     if (edgeType == EdgeType.Vertical || startCorner == edgeType)
            //     {
            //         edgeCount++;
            //     }

            //     if (edgeCount == 1 && startY == -1)
            //     {
            //         startY = edge.Y;
            //     }

            //     if (edgeCount == 2)
            //     {
            //         sum += edge.Y - startY + 1;
            //         startY = -1;
            //     }




                // Corner
            //     if (edge.Type != EdgeType.Vertical)
            //     {
            //         if (!onCornerBorder)
            //         {
            //             onCornerBorder = true;
                        
            //             if (!inSurface)
            //             {
            //                 inSurface = true;
            //                 startCorner = edge.Type == EdgeType.Unknown ? EdgeType.CornerDown : edge.Type;
            //                 startY = edge.Y;
            //             }
            //         }
            //         else
            //         {
            //             // Corner switch, still in the figure
            //             if (edge.Type != startCorner)
            //             {
            //                 startCorner = startCorner == EdgeType.CornerUp ? EdgeType.CornerDown : EdgeType.CornerUp;
            //                 onCornerBorder = false;
            //             }
            //             // End or border, wrap it up
            //             else
            //             {
            //                 inSurface = false;
            //                 onCornerBorder = false;
            //                 sum += edge.Y - startY + 1;
            //             }
            //         }
            //     }
            //     else
            //     {
            //         if (!inSurface)
            //         {
            //             inSurface = true;
            //             startY = edge.Y;
            //         }
            //         else
            //         {
            //             inSurface = false;
            //             // Include endpoint
            //             sum += edge.Y - startY + 1;
            //         }
            //     }

            }
            Console.WriteLine($"{sum}");
            result += sum;
        }
        // Console.WriteLine(" done");
        Console.WriteLine();

        // 111131594432791 too low
        Console.WriteLine($"Result 2: {result}");
    }
}


// 952408046582 mine
// 952408144115 reference