/// <summary>
/// Conjunction modules (prefix &) remember the type of the most recent pulse received from each of their connected input modules.
/// </summary>
/// They initially default to remembering a low pulse for each input. When a pulse is received, 
/// the conjunction module first updates its memory for that input. Then, if it remembers high pulses for 
/// all inputs, it sends a low pulse; otherwise, it sends a high pulse.
class Conjunction : Module
{
    public Conjunction(Dictionary<string, IModule> modules, List<string> destinations) :
        base(modules, destinations)
    { }

    private Dictionary<string, PulseType> _memory = new Dictionary<string, PulseType>();
    public Dictionary<string, bool> Periods { get; private set; } = new Dictionary<string, bool>();

    public override void AddInput(string name)
    {
        _memory[name] = PulseType.Low;
        Periods[name] = false;
    }

    public override List<string>? ProcessPulse()
    {
        if (_pulses.Count == 0) return null;
        var pulse = _pulses.Dequeue();
        _memory[pulse.Sender] = pulse.Type;

        if (pulse.Type == PulseType.High)
        {
            Periods[pulse.Sender] = true;
        }

        foreach (var destination in Destinations)
        {
            var pulseType = PulseType.High;
            if (_memory.Values.All(t => t == PulseType.High))
            {
                pulseType = PulseType.Low;
            }
            StorePulse(pulseType);
            var type = pulseType == PulseType.Low ? "low" : "high";

            if (_modules.ContainsKey(destination))
            {
                _modules[destination].SendPulse(pulseType, Name);
            }
            // Console.WriteLine($"{Name} -{type}-> {destination}");
        }

        return Destinations;
    }

    internal int FoundPeriodCount()
    {
        return Periods.Values.Count(p => p);
    }
}
