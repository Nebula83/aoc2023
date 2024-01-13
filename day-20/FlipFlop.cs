
/// <summary>
/// Flip-flop modules (prefix %) are either on or off; they are initially off.
/// </summary>
/// If a flip-flop module receives a high pulse, it is ignored and nothing happens. 
/// However, if a flip-flop module receives a low pulse, it flips between on and off. 
/// If it was off, it turns on and sends a high pulse. If it was on, it turns off and sends a low pulse.
class FlipFlop : Module
{
    private bool _stateOn = false;
    public FlipFlop(Dictionary<string, IModule> modules, List<string> destinations) : 
        base(modules, destinations) {}

    public override List<string>? ProcessPulse()
    {
        if (_pulses.Count == 0) return null;
        var pulse = _pulses.Dequeue();
        if (pulse.Type == PulseType.Low)
        {
            _stateOn = !_stateOn;
            var newPulse = _stateOn ? PulseType.High : PulseType.Low;

            foreach (var destination in Destinations)
            {
                _modules[destination].SendPulse(newPulse, Name);
                StorePulse(newPulse);

                var type = newPulse == PulseType.Low ? "low" : "high";
                // Console.WriteLine($"{Name} -{type}-> {destination}");
            }
        }

        return Destinations;
    }
}
