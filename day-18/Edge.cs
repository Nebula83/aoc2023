
public struct Edge
{
    public EdgeType Type { get; set; }
    public long Y { get; set; }

    internal static EdgeType DirectionsToCornerType(Direction currentDirection, Direction newDirection)
    {
        if (newDirection == Direction.Down || currentDirection == Direction.Up)
        {
            return EdgeType.CornerDown;
        }
        else if (newDirection == Direction.Up || currentDirection == Direction.Down)
        {
            return EdgeType.CornerUp;
        }

        return EdgeType.Unknown;
    }
}
