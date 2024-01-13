public interface IModule
{
    public List<string> Destinations { get; }
    public string Name { get; internal set; }

    void StorePulse(PulseType pulse);
    
    void AddInput(string name);
    public List<string>? ProcessPulse();
    public void SendPulse(PulseType pulseType, string sender);
}
