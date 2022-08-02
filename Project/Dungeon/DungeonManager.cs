using System.Collections.Generic;
using Project.Dungeon.Dungeons;
using Project.Dungeon.Rooms;
using Project.Util;

namespace Project.Dungeon
{
    public class DungeonManager
    {
        private static readonly DungeonManager Instance = new DungeonManager();
        private Room _currentRoom;
        private int _floorNumber;
        private List<List<RoomData>> _currentFloor;
        private FloorCoordinate _currentCoordinate;
        private DungeonTemplate _currentDungeon;
        private readonly List<DungeonTemplate> _dungeonList = new List<DungeonTemplate>() {Cavern.Instance};

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
            // Here the starting dungeon is selected and the current view on the BaseForm is set to the RoomView
            this.SelectDungeon(DungeonId.Cavern);
            RoomView.GetInstance().SetRoom(this._currentRoom);
            BaseForm.GetInstance().SwitchView(RoomView.GetInstance());
            GameTracker.GetInstance().SetPaused(false, false);
            // Finally, enter the starting room
            this._currentRoom.EnterRoom();
        }

        public void SelectDungeon(DungeonId dungeonId)
        {
            // Set the current dungeon to be the desired one
            if (dungeonId == DungeonId.NullDungeon)
            {
                this._currentDungeon = null;
                this._currentCoordinate = null;
                this._currentFloor = null;
                this._currentRoom = null;
                RoomView.GetInstance().SetRoom(null);
                return;
            }
            foreach (var dungeon in this._dungeonList)
            {
                if (dungeon.DungeonId != dungeonId) continue;
                this._currentDungeon = dungeon;
            }

            // Set the current coordinate and floor
            this._currentCoordinate = this._currentDungeon.StartCoordinate;
            this._currentFloor = this._currentDungeon.FloorOne;
            // Create the start room for the dungeon
            this._currentRoom = new Room(Direction.Centre);
            this._currentRoom.LoadRoomData(GetRoomData(this._currentFloor, this._currentCoordinate));
        }

        public RoomData GetRoomData(List<List<RoomData>> floor, FloorCoordinate coordinate)
        {
            return floor[coordinate.GetX()][coordinate.GetY()];
        }

        public Room GetCurrentRoom()
        {
            // Fetch the current Room
            return _currentRoom;
        }

        public List<List<RoomData>> GetCurrentFloor()
        {
            return this._currentFloor;
        }

        public FloorCoordinate GetCurrentCoordinate()
        {
            return this._currentCoordinate;
        }

        public void SetCurrentCoordinate(FloorCoordinate coordinate)
        {
            this._currentCoordinate = coordinate;
        }
    }
}