using System.Collections.Specialized;
using System.Text;

class Day2
{
    private string ReadFile(string name)
    {
        string? line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(name);
            //Read the first line of text
            line = sr.ReadLine() ?? "";
                //Continue to read until you reach end of file
            //close the file
            sr.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }

        return line;
    }

    internal static void Run()
    {
        var day = new Day2();
        // var line = day.ReadFile("test-1.txt");
        var line = day.ReadFile("input.txt");
        var instructions = line.Split(',');

        OrderedDictionary[] boxes = new OrderedDictionary[256];
        for (var boxIndex = 0; boxIndex < boxes.Length; boxIndex++)
        {
            boxes[boxIndex] = new OrderedDictionary();
        }

        // Put lenses in boxes
        foreach (var instruction in instructions)
        {
            var removeIndex = instruction.IndexOf('-'); 
            var addIndex = instruction.IndexOf('=');            
            var splitIndex =  removeIndex != -1 ? removeIndex : addIndex;

            var label = instruction.Substring(0, splitIndex);
            var hash = GetHash(label);
            var fl = instruction.Substring(splitIndex + 1);

            if (removeIndex != -1)
            {
                boxes[hash].Remove(label);
            }
            else if (addIndex != -1)
            {
                if (boxes[hash].Contains(label))
                {
                    boxes[hash][label] = fl;
                }
                else
                {
                    boxes[hash].Add(label, fl);
                }
            }
        }

        // Calculate POWERRRR
        var powerrr = 0L;
        for (var boxIndex = 0; boxIndex < boxes.Length; boxIndex++)
        {
            var slotNumber = 1;
            foreach (var fl in boxes[boxIndex].Values)
            {
                powerrr += (boxIndex + 1) * slotNumber * long.Parse((string)fl);
                slotNumber++;
            }
        }
        
        Console.WriteLine($"Result 2: {powerrr}");
    }

    private static int GetHash(string instruction)
    {
        var result = 0;

        // Determine the ASCII code for the current character of the string.
        foreach (var c in Encoding.ASCII.GetBytes(instruction))
        {
            // Increase the current value by the ASCII code you just determined.
            result += c;
            // Set the current value to itself multiplied by 17.
            result *= 17;
            // Set the current value to the remainder of dividing itself by 256.
            result %= 256;
        }

        return result;
    }
}