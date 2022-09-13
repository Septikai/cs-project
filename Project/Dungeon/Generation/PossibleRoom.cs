using System.Collections.Generic;
using Project.Util;

namespace Project.Dungeon.Generation
{
    public class PossibleRoom
    {
        public readonly List<PossibleRoomChoice> RoomPossibilities = new List<PossibleRoomChoice>();
        public bool Determined;
        public PossibleRoomChoice RoomChoice;
        public Direction Staircase = Direction.NullDirection;
        public readonly FloorCoordinate Location;

        public PossibleRoom(FloorCoordinate location)
        {
            // Set the coordinates the PossibleRoom refers to
            this.Location = location;
            // Add all possible room configurations to the list of possibilities
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0, 4));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 0, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 1, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 2, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.One, 3, 1));
            RoomPossibilities.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0, 6));
            if (location.GetY() == 0)
            {
                // If the room is along the top of the floor, doors cannot lead north
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Four));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 2));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 3));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoLine && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 3));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.One && r.RoomRotation == 0));
            }
            if (location.GetX() == 8)
            {
                // If the room is along the right of the floor, doors cannot lead east
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Four));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 3));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoLine && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.One && r.RoomRotation == 1));
            }
            if (location.GetY() == 8)
            {
                // If the room is along the bottom of the floor, doors cannot lead south
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Four));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 2));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoLine && r.RoomRotation == 0));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 2));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.One && r.RoomRotation == 2));
            }
            if (location.GetX() == 0)
            {
                // If the room is along the left of the floor, doors cannot lead west
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Four));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 2));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.Three && r.RoomRotation == 3));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoLine && r.RoomRotation == 1));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 2));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.TwoCorner && r.RoomRotation == 3));
                RoomPossibilities.Remove(RoomPossibilities.Find(r =>
                    r.PossibleRoomType == PossibleRoomType.One && r.RoomRotation == 3));
            }
        }

        public void Collapse()
        {
            if (this.RoomPossibilities.Count == 1)
            {
                // If there is only one possible choice, then pick that
                this.RoomChoice = this.RoomPossibilities[0];
                this.RoomPossibilities.RemoveAll(r => r != this.RoomChoice);
                this.Determined = true;
            }
            else if (this.RoomPossibilities.Count > 1)
            {
                // If there are multiple possibilities, pick one at random
                var roomOptions = new List<PossibleRoomChoice>();
                // Form a new list using the Bias values
                foreach (var option in this.RoomPossibilities)
                {
                    for (var i = 0; i < option.Bias; i++)
                    {
                        roomOptions.Add(option);
                    }
                }
                this.RoomChoice = roomOptions[DungeonGenerator.GetInstance().GetRandom().Next(roomOptions.Count)];
                this.RoomPossibilities.RemoveAll(r => r != this.RoomChoice);
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
            switch (this.RoomChoice.PossibleRoomType)
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
                    if (this.RoomChoice.RoomRotation != 1) doorDirections.Add(Direction.North);
                    if (this.RoomChoice.RoomRotation != 2) doorDirections.Add(Direction.East);
                    if (this.RoomChoice.RoomRotation != 3) doorDirections.Add(Direction.South);
                    if (this.RoomChoice.RoomRotation != 0) doorDirections.Add(Direction.West);
                    break;
                case PossibleRoomType.TwoLine:
                    // Check which orientation the room has to determine where the doors are
                    if (this.RoomChoice.RoomRotation == 0)
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
                    if (this.RoomChoice.RoomRotation == 0 || this.RoomChoice.RoomRotation == 3) doorDirections.Add(Direction.North);
                    if (this.RoomChoice.RoomRotation == 1 || this.RoomChoice.RoomRotation == 0) doorDirections.Add(Direction.East);
                    if (this.RoomChoice.RoomRotation == 2 || this.RoomChoice.RoomRotation == 1) doorDirections.Add(Direction.South);
                    if (this.RoomChoice.RoomRotation == 3 || this.RoomChoice.RoomRotation == 2) doorDirections.Add(Direction.West);
                    break;
                case PossibleRoomType.One:
                    // Add the appropriate door depending on the orientation of the room
                    switch (this.RoomChoice.RoomRotation)
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
            return new RoomData(doorDirections, this.Staircase);
        }
    }
}