public class Rule
{
    public required char Type { get; set; }
    public required Func<int, int, bool> Compare { get; set; }
    public required int Value { get; set; }
    public required string Refer { get; set; }

    public bool Matches(Dictionary<char, int> part)
    {
        return Compare(part[Type], Value);
    }
}
