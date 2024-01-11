public record struct Position 
{
    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; set; }
    public int Column { get; set; }

    public readonly List<(Direction, Position)> GetNeighbors(int maxHeight, int maxWidth)
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