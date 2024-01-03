public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public static class DirectionExtensions
{
    public static Direction Parse(this char direction)
    {
        return direction switch
        {
            'L' => Direction.Left,
            'R' => Direction.Right,
            'U' => Direction.Up,
            'D' => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
    }
    public static Direction Parse(this int direction)
    {
        // 0 means R, 1 means D, 2 means L, and 3 means U.
        return direction switch
        {
            0 => Direction.Right,
            1 => Direction.Down,
            2 => Direction.Left,
            3 => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
    }
}
