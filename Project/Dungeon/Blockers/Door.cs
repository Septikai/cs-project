using System.Drawing;
using Project.Dungeon.Rooms;
using Project.Util;

namespace Project.Dungeon.Blockers
{
    public sealed class Door : Blocker
    {
        private Direction _location;
        private Room _room;

        public Door(Room room, Direction location = Direction.NullDirection)
        {
            // Create a door
            this._room = room;
            this._location = location;
            this.BackColor = Color.SaddleBrown;
        }

        public void SetLocation(Direction location)
        {
            // Set the wall the door belongs on
            // For when it isn't initialised with it
            this._location = location;
        }

        public Direction GetLocation()
        {
            // Fetch the location of the wall
            return this._location;
        }

        public void Open()
        {
            // Fetch the current room
            var room = DungeonManager.GetInstance().GetCurrentRoom();
            if (room is null) return;
            
            // Find the current coordinates
            FloorCoordinate coordinate;
            var oldCoordinate = DungeonManager.GetInstance().GetCurrentCoordinate();
            // Use the current coordinate and the location of the door to determine
            // the coordinates of the room the player is moving to
            switch (this._location)
            {
                case Direction.North:
                    coordinate = new FloorCoordinate(oldCoordinate.GetX(), oldCoordinate.GetY() - 1);
                    break;
                case Direction.East:
                    coordinate = new FloorCoordinate(oldCoordinate.GetX() + 1, oldCoordinate.GetY());
                    break;
                case Direction.South:
                    coordinate = new FloorCoordinate(oldCoordinate.GetX(), oldCoordinate.GetY() + 1);
                    break;
                case Direction.West:
                    coordinate = new FloorCoordinate(oldCoordinate.GetX() - 1, oldCoordinate.GetY());
                    break;
                default:
                    coordinate = oldCoordinate;
                    break;
            }
            // Change the room to represent the new room
            var newRoomData = DungeonManager.GetInstance().GetRoomData(DungeonManager.GetInstance().GetCurrentFloor(), coordinate);
            room.LoadRoomData(newRoomData);
            // Set the new coordinates
            DungeonManager.GetInstance().SetCurrentCoordinate(coordinate);
            // Position the player in the new room
            room.EnterRoom(this._location.Opposite());
        }
    }
}