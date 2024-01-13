
public abstract class Module : IModule
{
    private static long _lows = 0;
    private static long _highs = 0;

    protected Queue<PulseSignal> _pulses = new Queue<PulseSignal>();
    protected Dictionary<string, IModule> _modules;

    public List<string> Destinations { get; private set; }
    public required string Name { get; set; }

    public Module(Dictionary<string, IModule> modules, List<string> destinations)
    {
        _modules = modules;
        Destinations = destinations;
    }

    public virtual void AddInput(string name) {}

    public abstract List<string>? ProcessPulse();

    public void SendPulse(PulseType pulseType, string sender)
    {   
        _pulses.Enqueue(new PulseSignal
        {
            Type = pulseType,
            Sender = sender,
        });
    }

    public void StorePulse(PulseType pulseType)
    {
        if (pulseType == PulseType.Low)
        {
            _lows++;
        }

        if (pulseType == PulseType.High)
        {
            _highs++;
        }
    }

    internal static long PulseProduct()
    {
        return _lows * _highs;
    }
}
