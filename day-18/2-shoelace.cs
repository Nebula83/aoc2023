using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Day2_Shoelace
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
        var day = new Day2_Shoelace();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        var start = new Coord(0, 0);

        // Gather all the holes
        var current = start;
        var points = new List<Coord>{start};

        var part1 = false;
        // var part1 = true;
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

            points.Add(newPoint);
            current = newPoint;
        }

        // Ain't math cool?
        var left = 0L;
        var right = 0L;
        var border = 0L;
        for (int pointIndex = 0; pointIndex < points.Count; pointIndex++)
        {
            var point = points[pointIndex];
            if (pointIndex < points.Count - 1)
            {
                // Shoelace theorem to calculate the surface of any simple polygon
                var nextPoint = points[pointIndex + 1];
                left += point.X * nextPoint.Y;
                right += nextPoint.X * point.Y;

                border += Math.Abs(point.X - nextPoint.X) + Math.Abs(point.Y - nextPoint.Y); 
            }
        }

        // Calculate true surface of the figure
        var surface = Math.Abs((left - right) / 2L);

        // Use Pick's theorem to convert surface to discrete interior points
        // A = i + b/2 - 1 
        // A = shoelace surface
        // i = interior points 
        // b = boundary points
        // i = -b/2 + 1 + A
        var result = 1 + surface - (border / 2);

        // Now include the border in the result.
        result += border;

        Console.WriteLine($"Result 2: {result}");
    }
}
