public class Coord: IEquatable<Coord>
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }public Coord(Coord coord)
    {
        X = coord.X;
        Y = coord.Y;
    }

    internal Coord Move(Direction direction, int count = 1)
    {
        var coord = new Coord(this);
        switch (direction)
        {
            case Direction.Up: coord.X -= count; break;
            case Direction.Down: coord.X+= count; break;
            case Direction.Left: coord.Y-= count; break;
            case Direction.Right: coord.Y+= count; break;
        }
        return coord;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public bool Equals(Coord? other)
    {
        return other?.X == X && other?.Y == Y;
    }
}
