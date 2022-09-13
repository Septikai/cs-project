using System.Collections.Generic;
using Project.Dungeon.Blockers;
using Project.Dungeon.Dungeons;
using Project.Dungeon.Generation;
using Project.Dungeon.Map;
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
        private readonly List<DungeonTemplate> _dungeonList = new List<DungeonTemplate>() {Cavern.Instance, Temple.Instance};
        private DungeonId _currentDungeonId;

        private DungeonManager()
        {
            // Create the instance of the DungeonManager
        }

        public static DungeonManager GetInstance()
        {
            // Get the DungeonManager instance
            return Instance;
        }

        public void NewGame(bool storyMode = true)
        {
            // This will run whenever a new game is started
            //
            // Here the starting dungeon is selected and the current view on the BaseForm is set to the RoomView
            this.SelectDungeon(storyMode ? DungeonId.Cavern : DungeonId.RandomDungeon);
            RoomView.GetInstance().SetRoom(this._currentRoom);
            BaseForm.GetInstance().SwitchView(RoomView.GetInstance());
            GameTracker.GetInstance().SetPaused(false, false);
            // Finally, enter the starting room
            this._currentRoom.EnterRoom();
        }

        public void SelectDungeon(DungeonId dungeonId)
        {
            // Set the current dungeon to be the desired one
            this._currentDungeonId = dungeonId;
            if (dungeonId == DungeonId.NullDungeon)
            {
                // Set everything to null
                this._currentDungeon = null;
                this._currentCoordinate = null;
                this._currentFloor = null;
                this._currentRoom = null;
                this._floorNumber = 1;
                DungeonGenerator.GetInstance().ClearDungeon();
                RoomView.GetInstance().SetRoom(null);
                MapBackground.GetInstance().ClearMap();
                return;
            }
            if (dungeonId == DungeonId.RandomDungeon)
            {
                // Generate and fetch generated dungeon
                this._currentDungeon = null;
                this._currentFloor = DungeonGenerator.GetInstance().GetFloor(1);
                this._floorNumber = 1;
                this.SetCurrentCoordinate(DungeonGenerator.GetInstance().GetStartCoordinate());
                this._currentRoom = new Room(Direction.Centre);
                this._currentRoom.LoadRoomData(GetRoomData(this._currentFloor, this._currentCoordinate));
                return;
            }
            // Set the current dungeon
            foreach (var dungeon in this._dungeonList)
            {
                if (dungeon.DungeonId != dungeonId) continue;
                this._currentDungeon = dungeon;
            }

            // Set the current coordinate and floor
            this._currentFloor = this._currentDungeon.GetFloor(1);
            this._floorNumber = 1;
            this.SetCurrentCoordinate(this._currentDungeon.StartCoordinate);
            // Create the start room for the dungeon
            this._currentRoom = new Room(Direction.Centre);
            this._currentRoom.LoadRoomData(GetRoomData(this._currentFloor, this._currentCoordinate));
        }

        public RoomData GetRoomData(List<List<RoomData>> floor, FloorCoordinate coordinate)
        {
            // Returns the RoomData of position location on the floor
            return floor[coordinate.GetX()][coordinate.GetY()];
        }

        public Room GetCurrentRoom()
        {
            // Fetch the current Room
            return _currentRoom;
        }

        public List<List<RoomData>> GetCurrentFloor()
        {
            // Get the current floor
            return this._currentFloor;
        }

        public FloorCoordinate GetCurrentCoordinate()
        {
            // Fetch the current location of the player
            return this._currentCoordinate;
        }

        public void SetCurrentCoordinate(FloorCoordinate coordinate)
        {
            // Update the current coordinate
            this._currentCoordinate = coordinate;
            // Update the map
            this.GetRoomData(this._currentFloor, coordinate).Explored = true;
            MapBackground.GetInstance().UpdateMap();
        }

        public void MoveFloor()
        {
            // Fetch the direction of the staircase
            var staircaseDirection =
                this._currentFloor[this._currentCoordinate.GetX()][this._currentCoordinate.GetY()].StaircaseDirection;
            if (this._currentDungeonId != DungeonId.RandomDungeon)
            {
                // If in story mode, check staircase direction against dungeon direction
                if (staircaseDirection == this._currentDungeon.StaircaseDirection) this._floorNumber++;
                else this._floorNumber--;
                // Fetch the new floor
                this._currentFloor = this._currentDungeon.GetFloor(this._floorNumber);
            }
            else
            {
                // If in endless mode, Down is used to progress forwards
                if (staircaseDirection == Direction.Down) this._floorNumber++;
                else this._floorNumber--;
                // Fetch the new floor
                this._currentFloor = DungeonGenerator.GetInstance().GetFloor(this._floorNumber);
            }
            // Reset the map for the new floor
            MapBackground.GetInstance().ClearMap();
            this.GetRoomData(this._currentFloor, this._currentCoordinate).Explored = true;
            MapBackground.GetInstance().UpdateMap();
        }

        public DungeonId GetCurrentDungeonId()
        {
            // Return the current dungeon Id
            return this._currentDungeonId;
        }

        public int GetCurrentFloorNumber()
        {
            // Return the current floor number
            return this._floorNumber;
        }
    }
}