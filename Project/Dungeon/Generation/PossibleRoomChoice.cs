namespace Project.Dungeon.Generation
{
    public class PossibleRoomChoice
    {
        public readonly PossibleRoomType PossibleRoomType;
        public readonly int RoomRotation;
        public readonly int Bias;

        public PossibleRoomChoice(PossibleRoomType possibleRoomType, int roomRotation, int bias = 0)
        {
            // Create a PossibleRoomChoice object
            // Used to store a room type and rotation easily
            this.PossibleRoomType = possibleRoomType;
            this.RoomRotation = roomRotation;
            this.Bias = bias;
        }
    }
}