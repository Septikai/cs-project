using System.Collections.Generic;
using Project.Util;

namespace Project.Dungeon
{
    public class RoomData
    {
        public readonly bool Generated;
        public bool Explored;
        public readonly bool NorthDoor;
        public readonly bool EastDoor;
        public readonly bool SouthDoor;
        public readonly bool WestDoor;
        public readonly Direction StaircaseDirection;

        public RoomData(List<Direction> doorLocations, Direction staircaseDirection = Direction.NullDirection)
        {
            // If there are no doors then create an "empty" RoomData object
            // It will have every field set to false
            if (doorLocations is null) return;
            // Set door locations
            foreach (var door in doorLocations)
            {
                switch (door)
                {
                    case (Direction.North):
                        this.NorthDoor = true;
                        break;
                    case (Direction.East):
                        this.EastDoor = true;
                        break;
                    case (Direction.South):
                        this.SouthDoor = true;
                        break;
                    case (Direction.West):
                        this.WestDoor = true;
                        break;
                }
            }
            // Set Staircase direction
            this.StaircaseDirection = staircaseDirection;

            this.Generated = true;
        }
    }
}