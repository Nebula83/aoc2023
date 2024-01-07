using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

public class Coord: IEquatable<Coord>
{
    public long X { get; set; }
    public long Y { get; set; }
    public Direction Direction { get; set; } = Direction.Any;

    public Coord(long x, long y)
    {
        X = x;
        Y = y;
    }
    
    public Coord(Coord coord)
    {
        X = coord.X;
        Y = coord.Y;
    }

    public static bool operator ==(Coord lhs, Coord rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Coord lhs, Coord rhs)
    {
        return !lhs.Equals(rhs);
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    public bool Equals(Coord? other)
    {
        return other?.X == X && other?.Y == Y;
    }

    public override bool Equals(Object? obj)
    {
        return obj is Coord position && Equals(position);
    }

    internal Coord Move(Direction direction, int count = 1)
    {
        var coord = new Coord(this);
        switch (direction)
        {
            case Direction.Up:
                coord.X -= count;
                coord.Direction = direction;
                break;
            case Direction.Down:
                coord.X+= count;
                coord.Direction = direction;
                break;
            case Direction.Left:
                coord.Y-= count;
                coord.Direction = direction;
                break;
            case Direction.Right:
                coord.Y+= count;
                coord.Direction = direction;
                break;
        }
        return coord;
    }
}
