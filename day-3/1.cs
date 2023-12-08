using System.IO;

class Day1
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
    
    string GetBlock(SchematicNumber schematicNumber, List<string> lines)
    {
        int startColumn = Math.Max(schematicNumber.Start - 1, 0);
        int startRow = Math.Max(schematicNumber.Line - 1, 0);

        // Assume all equal width
        int endColumn = Math.Min(schematicNumber.Start + schematicNumber.Size + 1, lines[0].Length);
        int endRow = Math.Min(schematicNumber.Line + 1, lines.Count - 1);

        string result = "";

        // Console.WriteLine("");
        // Console.WriteLine($"Number: {schematicNumber.Number}");
            
        for (int row = startRow; row <= endRow; row++)
        {
            var line = lines[row].Substring(startColumn, endColumn - startColumn);
            result += line;
            // Console.WriteLine(line);
        }

        return result;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.readFile("test-1.txt");
        var lines = day.readFile("input.txt");

        var schematicNumbers = new List<SchematicNumber>();
        for (int i = 0; i < lines.Count; i++)
        {
            schematicNumbers.AddRange(day.ParseSchematicNumbers(i, lines[i]));
        }

        int result = 0;
        foreach (var schematicNumber in schematicNumbers)
        {
            var block = day.GetBlock(schematicNumber, lines);

            // Console.WriteLine("");
            // Console.WriteLine($"Number: {schematicNumber.Number}");
            // Console.WriteLine(block);

            block = block.Replace(".", "");
            // Check if there is anything other than the number
            if (block.Length > schematicNumber.Text.Length)
            {
                schematicNumber.IsPart = true;
                result += schematicNumber.Number;
                // Console.WriteLine("match");
            }
        }

        Console.WriteLine($"Result 1: {result}");
    }
}