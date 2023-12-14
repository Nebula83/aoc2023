struct RangeMap
{
    public Int64 Source { get; set; }
    public Int64 Destination { get; set; }
    public Int64 Length { get; set; }
}

struct Almanac
{
    public List<Int64> Seeds { get; set; }
    public List<RangeMap> SeedRanges { get; set; }
    public List<RangeMap> SeedToSoil { get; set; }
    public List<RangeMap> SoilToFertilizer { get; set; }
    public List<RangeMap> FertilizerToWater { get; set; }
    public List<RangeMap> WaterToLight { get; set; }
    public List<RangeMap> LightToTemperature { get; set; }
    public List<RangeMap> TemperatureToHumidity { get; set; }
    public List<RangeMap> HumidityToLocation { get; set; }
}