using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;

class Day2
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
        var seeds = lines[0].Substring(7).Split(' ').Select(Int64.Parse).ToList();
        return seeds;
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
        var ranges = ParseSeeds(lines);
        var seeds = new List<RangeMap>();
        for (int index = 0; index < ranges.Count; index += 2)
        {
            seeds.Add(new RangeMap{
                Source = ranges[index],
                Destination = ranges[index],
                Length = ranges[index + 1],
            });
        }

        var result = new Almanac
        {
            SeedRanges = seeds,
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

    Int64 GetReverseMappedValue(Int64 source, List<RangeMap> maps)
    {
        Int64 result= source;

        foreach (var map in maps)
        {
            if ((source >= map.Destination) && (source < (map.Destination + map.Length)))
            {
                result = (source - map.Destination) + map.Source;
                break;
            }
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

    List<Int64> GetLocations(Almanac almanac, List<long> seeds)
    {
        var result = new List<Int64>();

        // foreach (var seed in almanac.Seeds)
        foreach (var seed in seeds)
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

    internal static void Run()
    {
        var day = new Day2();
        // var lines = day.ReadFile("test-1.txt");
        var lines = day.ReadFile("input.txt");
        var almanac = day.ParseInput(lines);

        // Sort the seeds by lowest destination
        almanac.HumidityToLocation.Sort((lhs, rhs) => lhs.Destination.CompareTo(rhs.Destination));
        var locationRange = almanac.HumidityToLocation[0];

        // Find the theoretical seeds that would yield the lowest location
        var candidateSeeds = new List<Int64>();
        for (long location = locationRange.Destination; location < (locationRange.Destination + locationRange.Length); location++)
        {
            var humidity = day.GetReverseMappedValue(location, almanac.HumidityToLocation);
            var temperature = day.GetReverseMappedValue(humidity, almanac.TemperatureToHumidity);
            var light = day.GetReverseMappedValue(temperature, almanac.LightToTemperature);
            var water = day.GetReverseMappedValue(light, almanac.WaterToLight);
            var fertilizer = day.GetReverseMappedValue(water, almanac.FertilizerToWater);
            var soil = day.GetReverseMappedValue(fertilizer, almanac.SoilToFertilizer);
            var seed = day.GetReverseMappedValue(soil, almanac.SeedToSoil);

            candidateSeeds.Add(seed);
        }

        var selectedSeeds = new List<long>();
        foreach (var candidateSeed in candidateSeeds)
        {
            // Check if the candidate is in range
            foreach (var seeds in almanac.SeedRanges)
            {
                if (candidateSeed >= seeds.Source && candidateSeed <= (seeds.Source + seeds.Length))
                {
                    selectedSeeds.Add(candidateSeed);
                    break;
                }
            }
        }

        var locations = day.GetLocations(almanac, selectedSeeds);

        Console.WriteLine($"Result 2: {locations.Min()}");
    }
}