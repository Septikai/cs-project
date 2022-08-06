namespace Project.Dungeon.Generation
{
    public class PossibleRoomChoice
    {
        public readonly PossibleRoomType PossibleRoomType;
        public readonly int RoomRotation;

        public PossibleRoomChoice(PossibleRoomType possibleRoomType, int roomRotation)
        {
            // Create a PossibleRoomChoice object
            // Used to store a room type and rotation easily
            this.PossibleRoomType = possibleRoomType;
            this.RoomRotation = roomRotation;
        }
    }
}