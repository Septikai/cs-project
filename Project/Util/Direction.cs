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
        NullDirection
    }
}