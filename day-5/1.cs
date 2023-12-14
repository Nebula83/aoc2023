using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;

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
        }

        return lines;
    }

    private List<Int64> ParseSeeds(List<string> lines)
    {
        return lines[0].Substring(7).Split(' ').Select(Int64.Parse).ToList();
    }

    private List<RangeMap> ParseMap(string mapName, List<string> lines)
    {
        int startOfSection = lines.FindIndex(l => l.Contains(mapName));
        int endOfSection = lines.FindIndex(startOfSection, string.IsNullOrWhiteSpace);
        var ranges = new List<RangeMap>();
        for (int line = startOfSection + 1; line < endOfSection; line++)
        {
            var range = lines[line].Split(' ').Select(Int64.Parse).ToList();
            ranges.Add(
                new RangeMap{
                    Destination = range[0],
                    Source = range[1],
                    Length = range[2],
                });
        }

        return ranges;
    }


    Almanac ParseInput(List<string> lines)
    {
        var result = new Almanac
        {
            Seeds = ParseSeeds(lines),
            SeedToSoil = ParseMap("seed-to-soil", lines),
            SoilToFertilizer = ParseMap("soil-to-fertilizer", lines),
            FertilizerToWater = ParseMap("fertilizer-to-water", lines),
            WaterToLight = ParseMap("water-to-light", lines),
            LightToTemperature = ParseMap("light-to-temperature", lines),
            TemperatureToHumidity = ParseMap("temperature-to-humidity", lines),
            HumidityToLocation = ParseMap("humidity-to-location", lines),
        };

        return result;
    }

    List<Int64> GetLocations(Almanac almanac)
    {
        var result = new List<Int64>();

        foreach (var seed in almanac.Seeds)
        {
            var soil = GetMappedValue(seed, almanac.SeedToSoil);
            var fertilizer = GetMappedValue(soil, almanac.SoilToFertilizer);
            var water = GetMappedValue(fertilizer, almanac.FertilizerToWater);
            var light = GetMappedValue(water, almanac.WaterToLight);
            var temperature = GetMappedValue(light, almanac.LightToTemperature);
            var humidity = GetMappedValue(temperature, almanac.TemperatureToHumidity);
            var location = GetMappedValue(humidity, almanac.HumidityToLocation);

            result.Add(location);
        }

        return result;
    }

    Int64 GetMappedValue(Int64 source, List<RangeMap> maps)
    {
        Int64 result= source;

        foreach (var map in maps)
        {
            if ((source >= map.Source) && (source < (map.Source + map.Length)))
            {
                result = (source - map.Source) + map.Destination;
                break;
            }
        }

        return result;
    }

    internal static void Run()
    {
        var day = new Day1();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");

        var almanac = day.ParseInput(lines);
        var locations = day.GetLocations(almanac);

        Console.WriteLine($"Result 1: {locations.Min()}");
    }
}
