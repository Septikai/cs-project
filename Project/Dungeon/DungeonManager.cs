using Project.Dungeon.Rooms;
using Project.Util;

namespace Project.Dungeon
{
    public class DungeonManager
    {
        private static readonly DungeonManager Instance = new DungeonManager();
        private Room _currentRoom;

        private DungeonManager()
        {
            // Create the instance of the DungeonManager
        }

        public static DungeonManager GetInstance()
        {
            // Get the DungeonManager instance
            return Instance;
        }

        public void NewGame()
        {
            // This will run whenever a new game is started
            //
            // Here the starting room is created and the current view on the BaseForm is set to the RoomView
            this._currentRoom = new Room(Direction.Centre);
            RoomView.GetInstance().SetRoom(this._currentRoom);
            BaseForm.GetInstance().SwitchView(BaseForm.RoomViewInstance);
            GameTracker.GetInstance().SetPaused(false);
            // Finally, setup the starting room
            this._currentRoom.EnterRoom();
        }
    }
}