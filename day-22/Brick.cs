record class Brick
{
    private Coord _start;
    private Coord _end;

    public int Index { get; set; } = 0;
    public int LowestZ { get; private set; }
    public int HighestZ { get; private set; }

    public List<Brick> Supports { get; set; } = new();
    public List<Brick> SupportedBy { get; set; } = new();
    public int LowestX { get; private set; }
    public int HighestX { get; private set; }
    public int LowestY { get; private set; }
    public int HighestY { get; private set; }
    public Coord Start
    {
        get => _start;
        private set { _start = value; UpdateZs(); }
    }
    public Coord End
    {
        get => _end;
        private set { _end = value; UpdateZs(); }
    }

    public Brick(Coord start, Coord end)
    {
        _start = start;
        _end = end;

        LowestX = int.Min(Start.X, End.X);
        HighestX = int.Max(Start.X, End.X);

        LowestY = int.Min(Start.Y, End.Y);
        HighestY = int.Max(Start.Y, End.Y);

        UpdateZs();
    }

    private void UpdateZs()
    {
        LowestZ = int.Min(Start.Z, End.Z);
        HighestZ = int.Max(Start.Z, End.Z);
    }

    internal void Drop(int newZ)
    {
        var diff = LowestZ - newZ;

        _start.Z -= diff;
        _end.Z -= diff;

        UpdateZs();
    }

    internal bool WouldCollide(Brick target)
    {
        var lowX = int.Max(target.LowestX, LowestX);
        var highX = int.Min(target.HighestX, HighestX);

        var lowY = int.Max(target.LowestY, LowestY);
        var highY = int.Min(target.HighestY, HighestY);

        return (lowX <= highX) && (lowY <= highY);
    }
}