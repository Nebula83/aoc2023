public struct Position : IEquatable<Position>
{
    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; set; }
    public int Column { get; set; }

    public override bool Equals(Object? obj)
    {
        return obj is Position position && Equals(position);
    }

    public bool Equals(Position other)
    {
        return Row == other.Row && Column == other.Column;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return Row.GetHashCode() * 17 + Column.GetHashCode();
        }
    }

    public static bool operator ==(Position lhs, Position rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Position lhs, Position rhs)
    {
        return !lhs.Equals(rhs);
    }

    public List<(Direction, Position)> GetNeighbors(int maxWidth, int maxHeight)
    {
        var neighbors = new List<(Direction, Position)>();
        if (Row > 0)
        {
            neighbors.Add((Direction.Up, new Position(Row - 1, Column)));
        }
        if (Row < maxHeight - 1)
        {
            neighbors.Add((Direction.Down, new Position(Row + 1, Column)));
        }
        if (Column > 0)
        {
            neighbors.Add((Direction.Left, new Position(Row, Column - 1)));
        }
        if (Column < maxWidth - 1)
        {
            neighbors.Add((Direction.Right, new Position(Row, Column + 1)));
        }
        return neighbors;
    }
}