using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Project.Util;

namespace Project.Dungeon.Generation
{
    public class DungeonGenerator
    {
        private static readonly DungeonGenerator Instance = new DungeonGenerator();
        private readonly List<PossibleRoom> _floorPossibilities = new List<PossibleRoom>();
        private readonly List<PossibleRoomChoice> _northDoorMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _eastDoorMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _southDoorMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _westDoorMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _northWallMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _eastWallMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _southWallMatches = new List<PossibleRoomChoice>();
        private readonly List<PossibleRoomChoice> _westWallMatches = new List<PossibleRoomChoice>();
        private readonly Random _random = new Random();
        private readonly List<List<RoomData>> _generatedFloor = new List<List<RoomData>>();
        private FloorCoordinate _generatedStartCoordinate;

        private DungeonGenerator()
        { 
            // Specify the constraints for floor generation
            // _northDoorMatches includes all room configurations that are
            // valid next to a room which has a door on it's north face, and so on with the rest
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2));
            this._northDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 2));
            
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3));
            this._eastDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 3));
            
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3));
            this._southDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 0));
            
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Four, 0));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1));
            this._westDoorMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 1));
            
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 3));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 0));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 1));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 3));
            this._northWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0));
            
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 0));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 0));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 0));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 1));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 2));
            this._eastWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0));
            
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 1));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 1));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 1));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 1));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 2));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 3));
            this._southWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0));
            
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Three, 2));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoLine, 0));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 2));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.TwoCorner, 3));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 0));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 2));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.One, 3));
            this._westWallMatches.Add(new PossibleRoomChoice(PossibleRoomType.Zero, 0));
        }

        public static DungeonGenerator GetInstance()
        {
            // Fetch the DungeonGenerator
            return Instance;
        }

        public List<List<RoomData>> GetNewFloor()
        {
            // Fill the list of RoomPossibilities with new PossibleRooms
            InitialiseFloorPossibilities();
            // Generate a new floor
            GenerateFloor();
            // Convert the result of the generation into usable floor
            ConvertDataToFloor();
            return this._generatedFloor;
        }

        private void InitialiseFloorPossibilities()
        {
            // Empty the list
            this._floorPossibilities.Clear();
            // Fill the list with new PossibleRooms, one for each coordinate of the floor
            for (var col = 0; col < 9; col++)
            {
                for (var row = 0; row < 9; row++)
                {
                    this._floorPossibilities.Add(new PossibleRoom(new FloorCoordinate(col, row)));
                }
            }
        }

        private void ConvertDataToFloor()
        {
            // Empty the list
            this._generatedFloor.Clear();
            // Create a new list to all possible start coordinate
            var possibleStartCoordinates = new List<FloorCoordinate>();
            for (var col = 0; col < 9; col++)
            {
                this._generatedFloor.Add(new List<RoomData>());
                for (var row = 0; row < 9; row++)
                {
                    // Find the PossibleRoom representing this location and get the RoomData representing it
                    var roomChoice = this._floorPossibilities.Find(r => 
                        r.Location.GetX() == col && r.Location.GetY() == row);
                    var roomData = roomChoice.ToRoomData();
                    // Add the RoomData to it's position in the floor
                    this._generatedFloor[col].Add(roomData);
                    // If there is an exit to the room, add it as a possible start location
                    if (roomData.DoorLocations != null && roomData.DoorLocations.Count != 0)
                    {
                        possibleStartCoordinates.Add(new FloorCoordinate(col, row));
                    }
                }
            }
            // Choose the start room from the list
            this._generatedStartCoordinate =
                possibleStartCoordinates[this._random.Next(possibleStartCoordinates.Count)];
        }

        private void GenerateFloor()
        {
            // Loop until the floor has been fully generated
            bool done;
            do
            {
                // Get the room with the lowest "entropy"
                var room = PickLowestEntropyPossibleRoom();
                // Collapse that room and propagate
                CollapseFromRoom(room);
                // Check if every room in the floor has been determined
                done = true;
                foreach (var possible in this._floorPossibilities)
                {
                    if (possible.Determined == false)
                    {
                        done = false;
                        break;
                    }
                }
            } while (!done);
        }

        private PossibleRoom PickLowestEntropyPossibleRoom()
        {
            var lowestEntropyRooms = new List<PossibleRoom>();
            var lowestEntropyValue = 999;
            // Iterate over all PossibleRooms
            foreach (var possibleRoom in this._floorPossibilities)
            {
                if (possibleRoom.RoomPossibilities.Count < lowestEntropyValue && possibleRoom.RoomPossibilities.Count != 1)
                {
                    // If the room has lower entropy than the current lowest, update the lowest and clear the list
                    lowestEntropyValue = possibleRoom.RoomPossibilities.Count;
                    lowestEntropyRooms.Clear();
                    lowestEntropyRooms.Add(possibleRoom);
                }
                else if (possibleRoom.RoomPossibilities.Count == lowestEntropyValue)
                {
                    // If the room has entropy equal to the lowest current entropy, add it to the list
                    lowestEntropyRooms.Add(possibleRoom);
                }
            }

            // Select the room with lowest entropy, or if there a multiple, select one of them at random
            PossibleRoom choice;
            if (lowestEntropyRooms.Count == 1) choice = lowestEntropyRooms[0];
            else choice = lowestEntropyRooms[this._random.Next(lowestEntropyRooms.Count)];

            return choice;
        }

        private void CollapseFromRoom(PossibleRoom room)
        {
            // Collapse the room
            room.Collapse();
            // Propagate the result to nearby rooms
            PropagateFrom(room);
        }
        
        private void PropagateFrom(PossibleRoom room, PossibleRoom origin = null, Direction originDir = Direction.NullDirection)
        {
            // If the room is not the initial room and is already determined, return
            if (origin != null && room.Determined) return;
            var possibilitiesChanged = false;
            // If this is not the initial room, we need to check to see if the possible states can be narrowed down
            if (origin != null)
            {
                var door = false;
                var wall = false;
                // Iterate over all possible choices for the previous room and see if it can have a door and wall
                // facing this room
                foreach (var possibility in origin.RoomPossibilities)
                {
                    switch (possibility.PossibleRoomType)
                    {
                        case PossibleRoomType.Four:
                            // If there are 4 doors, there will always be a door available
                            door = true;
                            break;
                        case PossibleRoomType.Three:
                            // See if there is a door or wall facing the current room
                            switch (possibility.RoomRotation)
                            {
                                case 0:
                                    if (originDir != Direction.East) door = true;
                                    else wall = true;
                                    break;
                                case 1:
                                    if (originDir != Direction.South) door = true;
                                    else wall = true;
                                    break;
                                case 2:
                                    if (originDir != Direction.West) door = true;
                                    else wall = true;
                                    break;
                                case 3:
                                    if (originDir != Direction.North) door = true;
                                    else wall = true;
                                    break;
                            }
                            break;
                        case PossibleRoomType.TwoLine:
                            // Check if there is a door or wall facing the current room
                            switch (possibility.RoomRotation)
                            {
                                case 0:
                                    if (originDir == Direction.South || originDir == Direction.North) door = true;
                                    else wall = true;
                                    break;
                                case 1:
                                    if (originDir == Direction.West || originDir == Direction.East) door = true;
                                    else wall = true;
                                    break;
                            }
                            break;
                        case PossibleRoomType.TwoCorner:
                            // Check if there is a door or wall facing the current room
                            switch (possibility.RoomRotation)
                            {
                                case 0:
                                    if (originDir == Direction.South || originDir == Direction.West) door = true;
                                    else wall = true;
                                    break;
                                case 1:
                                    if (originDir == Direction.West || originDir == Direction.North) door = true;
                                    else wall = true;
                                    break;
                                case 2:
                                    if (originDir == Direction.North || originDir == Direction.East) door = true;
                                    else wall = true;
                                    break;
                                case 3:
                                    if (originDir == Direction.East || originDir == Direction.South) door = true;
                                    else wall = true;
                                    break;
                            }
                            break;
                        case PossibleRoomType.One:
                            // See if the door is facing this room or not
                            switch (possibility.RoomRotation)
                            {
                                case 0:
                                    if (originDir == Direction.South) door = true;
                                    else wall = true;
                                    break;
                                case 1:
                                    if (originDir == Direction.West) door = true;
                                    else wall = true;
                                    break;
                                case 2:
                                    if (originDir == Direction.North) door = true;
                                    else wall = true;
                                    break;
                                case 3:
                                    if (originDir == Direction.East) door = true;
                                    else wall = true;
                                    break;
                            }
                            break;
                        case PossibleRoomType.Zero:
                            // If there are no doors, there will always be a wall facing this room
                            wall = true;
                            break;
                    }
                }

                // If there is either a door or wall available but not the other,
                // we might be able to narrow down possible choices
                if (door && !wall)
                {
                    // Determine which room configurations are valid
                    List<PossibleRoomChoice> validList;
                    switch (originDir)
                    {
                        case Direction.North:
                            // Anything with a door to the south is valid
                            validList = _southDoorMatches;
                            break;
                        case Direction.East:
                            // Anything with a door to the west is valid
                            validList = _westDoorMatches;
                            break;
                        case Direction.South:
                            // Anything with a door to the north is valid
                            validList = _northDoorMatches;
                            break;
                        case Direction.West:
                            // Anything with a door to the east is valid
                            validList = _eastDoorMatches;
                            break;
                        default:
                            validList = new List<PossibleRoomChoice>();
                            break;
                    }

                    // Iterate over all of this room's possible choices
                    foreach (var possibleChoice in new List<PossibleRoomChoice>(room.RoomPossibilities))
                    {
                        // If the current choice is in the list of valid choices, check the next choice
                        if (validList.Find(r =>
                                r.PossibleRoomType == possibleChoice.PossibleRoomType &&
                                r.RoomRotation == possibleChoice.RoomRotation) != null) continue;
                        // If the current choice is not valid, remove it
                        possibilitiesChanged = true;
                        room.RoomPossibilities.Remove(possibleChoice);
                    }
                }
                else if (!door && wall)
                {
                    // Determine which room configurations are valid
                    List<PossibleRoomChoice> validList;
                    switch (originDir)
                    {
                        case Direction.North:
                            // Anything with a wall to the south is valid
                            validList = _southWallMatches;
                            break;
                        case Direction.East:
                            // Anything with a wall to the west is valid
                            validList = _westWallMatches;
                            break;
                        case Direction.South:
                            // Anything with a wall to the north is valid
                            validList = _northWallMatches;
                            break;
                        case Direction.West:
                            // Anything with a wall to the east is valid
                            validList = _eastWallMatches;
                            break;
                        default:
                            validList = new List<PossibleRoomChoice>();
                            break;
                    }

                    // Iterate over all of this room's possible choices
                    foreach (var possibleChoice in new List<PossibleRoomChoice>(room.RoomPossibilities))
                    {
                        // If the current choice is in the list of valid choices, check the next choice
                        if (validList.Find(r =>
                                r.PossibleRoomType == possibleChoice.PossibleRoomType &&
                                r.RoomRotation == possibleChoice.RoomRotation) != null) continue;
                        // If the current choice is not valid, remove it
                        possibilitiesChanged = true;
                        room.RoomPossibilities.Remove(possibleChoice);
                    }
                }
                // If the possible choices for the current room were not modified,
                // then there is no need to check the adjacent rooms
                if (!possibilitiesChanged) return;
                // If there is only one possible choice for the current room,
                // then that is the room's final state
                if (room.RoomPossibilities.Count == 1) room.Collapse();
            }

            // Create a stack made up of all of the adjacent rooms
            var toPropagate = new Stack<Tuple<PossibleRoom, Direction>>();
            // If along the west edge of the map, do not check for the room to the west
            if (room.Location.GetX() != 0)
            {
                // West Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() - 1 &&
                    r.Location.GetY() == room.Location.GetY());
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.East);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the south edge of the map, do not check for the room to the south
            if (room.Location.GetY() != 8)
            {
                // South Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() &&
                    r.Location.GetY() == room.Location.GetY() + 1);
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.North);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the east edge of the map, do not check for the room to the east
            if (room.Location.GetX() != 8)
            {
                // East Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() + 1 &&
                    r.Location.GetY() == room.Location.GetY());
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.West);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the north edge of the map, do not check for the room to the north
            if (room.Location.GetY() != 0)
            {
                // North Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() &&
                    r.Location.GetY() == room.Location.GetY() - 1);
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.South);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }

            // Loop while there are items in the stack
            while (toPropagate.Count > 0)
            {
                // Pop a room off the stack
                var nextRoomData = toPropagate.Pop();
                // Propagate from that room
                PropagateFrom(nextRoomData.Item1, room, nextRoomData.Item2);
            }
        }

        public Random GetRandom()
        {
            // Fetch the instance of Random
            return this._random;
        }

        public FloorCoordinate GetStartCoordinate()
        {
            // Get the generated floor's start location
            return this._generatedStartCoordinate;
        }
    }
}