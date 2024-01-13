
/// <summary>
/// There is a single broadcast module (named broadcaster).
/// </summary>
/// When it receives a pulse, it sends the same pulse to all of its destination modules.
class Broadcaster : Module
{
    public Broadcaster(Dictionary<string, IModule> modules, List<string> destinations) : 
        base(modules, destinations) {}

    public override List<string>? ProcessPulse()
    {
        if (_pulses.Count == 0) return null;
        var pulse = _pulses.Dequeue();
        foreach (var destination in Destinations)
        {
            _modules[destination].SendPulse(pulse.Type, Name);
            StorePulse(pulse.Type);

            var type = pulse.Type == PulseType.Low ? "low" : "high";

            // Console.WriteLine($"broadcaster -{type}-> {destination}");
        }

        return Destinations;
    }
}
