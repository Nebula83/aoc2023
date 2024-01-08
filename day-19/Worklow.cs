public class Worklow
{
    public required string Name { get; set; }
    public required List<Rule> Rules { get; set; }
    public required string LastRule { get; set; }

    internal string Process(Dictionary<char, int> part)
    {
        foreach (var rule in Rules)
        {
            if (rule.Matches(part))
            {
                return rule.Refer;
            }
        }

        return LastRule;
    }
}
