public enum Direction
{
    Any,
    Left,
    Right,
    Down,
    Up,
}

public static class  DirectionMethods
{
    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Any => Direction.Any,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Up => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
