using System.Collections.Generic;
using Project.Util;

namespace Project.Dungeon.Generation
{
    public class PossibleRoom
    {
        public readonly List<PossibleRoomChoice> RoomPossibilities = new List<PossibleRoomChoice>();
        public bool Determined;
        private PossibleRoomChoice _roomChoice;
        public readonly FloorCoordinate Location;

        public PossibleRoom(FloorCoordinate location)
        {
            // Set the coordinates the PossibleRoom refers to
            this.Location = location;
            // Add all possible room configurations to the list of possibilities
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 0));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 2));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 3));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0));
        }

        public void Collapse()
        {
            if (this.RoomPossibilities.Count == 1)
            {
                // If there is only one possible choice, then pick that
                this._roomChoice = this.RoomPossibilities[0];
                this.RoomPossibilities.RemoveAll(r => r != this._roomChoice);
                this.Determined = true;
            }
            else if (this.RoomPossibilities.Count > 1)
            {
                // If there are multiple possibilities, pick one at random
                this._roomChoice = this.RoomPossibilities[DungeonGenerator.GetInstance().GetRandom().Next(this.RoomPossibilities.Count)];
                this.RoomPossibilities.RemoveAll(r => r != this._roomChoice);
                this.Determined = true;
            }
        }

        public RoomData ToRoomData()
        {
            // If there is no final state for the room, it cannot be converted into RoomData
            if (!this.Determined) return null;
            // Create a list to store the door locations
            var doorDirections = new List<Direction>();
            // Determine how many doors there are in the room, and where they are
            switch (this._roomChoice.PossibleRoomType)
            {
                case PossibleRoomType.Four:
                    // A room with 4 doors has doors in all 4 
                    doorDirections.Add(Direction.North);
                    doorDirections.Add(Direction.East);
                    doorDirections.Add(Direction.South);
                    doorDirections.Add(Direction.West);
                    break;
                case PossibleRoomType.Three:
                    // Determine which doors should be added by ensuring that the rotation includes each one
                    if (this._roomChoice.RoomRotation != 1) doorDirections.Add(Direction.North);
                    if (this._roomChoice.RoomRotation != 2) doorDirections.Add(Direction.East);
                    if (this._roomChoice.RoomRotation != 3) doorDirections.Add(Direction.South);
                    if (this._roomChoice.RoomRotation != 0) doorDirections.Add(Direction.West);
                    break;
                case PossibleRoomType.TwoLine:
                    // Check which orientation the room has to determine where the doors are
                    if (this._roomChoice.RoomRotation == 0)
                    {
                        doorDirections.Add(Direction.North);
                        doorDirections.Add(Direction.South);
                    }
                    else
                    {
                        doorDirections.Add(Direction.East);
                        doorDirections.Add(Direction.West);
                    }
                    break;
                case PossibleRoomType.TwoCorner:
                    // If the rotation contains that door, add it
                    if (this._roomChoice.RoomRotation == 0 || this._roomChoice.RoomRotation == 3) doorDirections.Add(Direction.North);
                    if (this._roomChoice.RoomRotation == 1 || this._roomChoice.RoomRotation == 0) doorDirections.Add(Direction.East);
                    if (this._roomChoice.RoomRotation == 2 || this._roomChoice.RoomRotation == 1) doorDirections.Add(Direction.South);
                    if (this._roomChoice.RoomRotation == 3 || this._roomChoice.RoomRotation == 2) doorDirections.Add(Direction.West);
                    break;
                case PossibleRoomType.One:
                    // Add the appropriate door depending on the orientation of the room
                    switch (this._roomChoice.RoomRotation)
                    {
                        case 0:
                            doorDirections.Add(Direction.North);
                            break;
                        case 1:
                            doorDirections.Add(Direction.East);
                            break;
                        case 2:
                            doorDirections.Add(Direction.South);
                            break;
                        case 3:
                            doorDirections.Add(Direction.West);
                            break;
                    }
                    break;
                case PossibleRoomType.Zero:
                    // Rooms with no doors should pass null into the RoomData constructor
                    doorDirections = null;
                    break;
            }
            return new RoomData(doorDirections);
        }
    }
}