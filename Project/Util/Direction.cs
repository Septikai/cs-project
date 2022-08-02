namespace Project.Util
{
    public enum Direction
    {
        // Create an Enum of Directions
        North,
        East,
        South,
        West,
        // Centre will be used for the starting room and staircases
        Centre,
        // Up and Down will be used for staircases
        Up,
        Down,
        // If no other Direction applies, NullDirection will be used
        // In most cases, a NullDirection should cause an error
        // In RoomData, NullDirection will also be used to indicate lack of staircase
        NullDirection
    }

    public static class DirectionExtension
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.Centre:
                    return Direction.Centre;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                default:
                    return Direction.NullDirection;
                
            }
        }
    }
}