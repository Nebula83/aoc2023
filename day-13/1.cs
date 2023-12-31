using System.IO;

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
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        var images = day.ParseLines(lines);

        var result = 0;
        foreach (var image in images)
        {
            // Horizontal mirror
            var (horValid, horScore) = FindMirror(image);
            var horCount = horValid ? horScore + 1 : 0;

            // Vertical mirror
            var columns = new List<string>();
            // Assume rectangle
            for (int column = 0; column < image[0].Length; column++)
            {
                var columnString = string.Empty;
                for (int row = 0; row < image.Count; row++)
                {
                    columnString += image[row][column];
                }
                columns.Add(columnString);
            }

            var (vertValid, vertScore) = FindMirror(columns);
            var vertCount = vertValid ? vertScore + 1 : 0;

            result += 100 * horCount + vertCount;
        }


        Console.WriteLine($"Result 1: {result}");
    }

    private static (bool, int) FindMirror(List<string> image)
    {
        var prevIndex = -1;
        var startIndex = 0;
        var done = false;
        for (var lineIndex = 0; lineIndex < image.Count; lineIndex++)
        {
            var line = image[lineIndex];
            if (!done)
            {
                var prevLine = prevIndex >= 0 ? image[prevIndex] : "";

                int diffCount = string.IsNullOrEmpty(prevLine) ? -1 : 0;
                for (int charIndex = 0; charIndex < prevLine.Length; charIndex++)
                {
                    if (prevLine[charIndex] != line[charIndex])
                    {
                        diffCount++;
                    }
                }

                if (prevLine == line)
                {
                    if ((lineIndex - prevIndex) == 1)
                    {
                        startIndex = prevIndex;
                    }

                    if (lineIndex == image.Count - 1)
                    {
                        done = true;
                    }
                    if (prevIndex == 0)
                    {
                        done = true;
                    }
                    prevIndex--;
                }
                else
                {
                    // Split compare didn't work, try the neighbor next
                    if ((lineIndex - prevIndex) > 1)
                    {
                        lineIndex--;
                    }
                    prevIndex = lineIndex;
                }
            }
        }

        return (done, startIndex);
    }

    private List<List<string>> ParseLines(List<string> lines)
    {
        var images = new List<List<string>>();

        var image = new List<string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                images.Add(image);
                image = new List<string>();
            }
            else
            {
                image.Add(line);
            }
        }
        images.Add(image);
        
        return images;
    }
}