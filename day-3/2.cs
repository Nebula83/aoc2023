using System.IO;

struct GearSet
{
    int Line { get; set; }
    int Column { get; set; }
    SchematicNumber? First { get; set; }
    SchematicNumber? Second { get; set; }
}

class Day2
{
    private List<string> readFile(string name)
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

    List<SchematicNumber> ParseSchematicNumbers(int lineNr, string line)
    {
        var result = new List<SchematicNumber>();
        SchematicNumber? item = null;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (!Char.IsDigit(c) && item != null)
            {
                // Console.WriteLine(item.Text);
                item.Number = int.Parse(item.Text);
                result.Add(item);
                item = null;
            }

            if (Char.IsDigit(c))
            {
                item ??= new SchematicNumber(lineNr, i);
                item.Text += c;
            }
            // Lazy but it's late: EOL
            if ((i == (line.Length - 1)) && item != null)
            {
                // Console.WriteLine(item.Text);
                item.Number = int.Parse(item.Text);
                result.Add(item);
                item = null;
            }
        }

        return result;
    }
    
    void FindCogs(SchematicNumber schematicNumber, List<string> lines)
    {
        int startColumn = Math.Max(schematicNumber.Start - 1, 0);
        int startRow = Math.Max(schematicNumber.Line - 1, 0);

        // Assume all equal width
        int endColumn = Math.Min(schematicNumber.Start + schematicNumber.Size + 1, lines[0].Length);
        int endRow = Math.Min(schematicNumber.Line + 1, lines.Count - 1);
        
        for (int row = startRow; row <= endRow; row++)
        {
            for (int column = startColumn; column < endColumn; column++)
            {
                if (lines[row][column] == '*')
                {

                    schematicNumber.CogX = row;
                    schematicNumber.CogY = column;
                }
            }
        }
    }

    internal static void Run()
    {
        var day = new Day2();
        // var lines = day.readFile("test-1.txt");
        var lines = day.readFile("input.txt");

        var schematicNumbers = new List<SchematicNumber>();
        for (int i = 0; i < lines.Count; i++)
        {
            schematicNumbers.AddRange(day.ParseSchematicNumbers(i, lines[i]));
        }

        foreach (var schematicNumber in schematicNumbers)
        {
            day.FindCogs(schematicNumber, lines);
        }

        var gears = new Dictionary<string, List<SchematicNumber>>();
        foreach (var schematicNumber in schematicNumbers)
        {
            var key = $"{schematicNumber.CogX},{schematicNumber.CogY}";
            if (!gears.Keys.Contains(key))
            {
                gears.Add(key, new List<SchematicNumber>{schematicNumber});
            }
            else
            {
                gears[key].Add(schematicNumber);
            }
        }

        var result = 0;
        var gearSets = gears.Values.Where(g => g.Count == 2);
        foreach (var gearSet in gearSets)
        {
            result += gearSet[0].Number * gearSet[1].Number;
        }

        Console.WriteLine($"Result 2: {result}");
    }
}