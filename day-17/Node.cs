partial class Day1
{
    class Node : IEquatable<Node>
    {
        public required Position Position { get; set; }
        public required int Steps { get; set; }
        public required Direction Direction { get; set; }

        // To use the object as an efficient key into the heat map dictionary.
        public bool Equals(Node? other)
        {
            return Position == other?.Position
                && Steps == other?.Steps
                && Direction == other?.Direction;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Position.GetHashCode()
                       ^ Steps.GetHashCode()
                       ^ Direction.GetHashCode();
            }
        }
    }
}
