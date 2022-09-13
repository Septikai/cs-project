using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Project.Dungeon.Dungeons;
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
        private readonly Dictionary<int, List<List<RoomData>>> _generatedDungeon = new Dictionary<int, List<List<RoomData>>>();
        private int _currentFloorBatch = 0;
        private FloorCoordinate _generatedStartCoordinate;
        private readonly Dictionary<int, Dictionary<int, List<PossibleRoom>>> _areas = new Dictionary<int, Dictionary<int, List<PossibleRoom>>>();
        private readonly Dictionary<int, Dictionary<int, bool>> _validAreas = new Dictionary<int, Dictionary<int, bool>>();
        private Thread _backgroundFloorGenerationThread = new Thread(() => {});
        private FloorCoordinate _entryStaircaseLocation;


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

        private void GenerateNewFloorBatch()
        {
            // Generate a starting coordinate
            if (this._currentFloorBatch == 0) GenerateStartCoordinate();

            var toInclude = new List<FloorCoordinate> {this._entryStaircaseLocation};
            for (var floor = this._currentFloorBatch; floor < this._currentFloorBatch + 20; floor++)
            {
                // Fill the list of RoomPossibilities with new PossibleRooms
                InitialiseFloorPossibilities(floor, toInclude);
                // Generate a new floor
                GenerateFloor(floor);
                // Ensure the floor is valid
                WalkThroughFloor(floor, toInclude);
                // Position staircases on the floor
                PlaceStaircases(floor);

                // Set the entry location on the floor
                toInclude.Clear();
                toInclude.Add(this._entryStaircaseLocation);
            }

            // Convert the result of the generation into usable floor
            ConvertDataToFloor();
            // Increase the current batch number for next batch
            this._currentFloorBatch += 20;
        }

        private void PlaceStaircases(int floor)
        {
            // Find the area with the entry location
            var targetArea = GetTargetArea(this._entryStaircaseLocation);
            // Set the target distance between the entry and exit staircases
            var targetDist = 9;
            var possibleLocations = new List<PossibleRoom>();
            
            // Iterate to find possible staircase locations
            while (possibleLocations.Count == 0)
            {
                // Add all rooms in the area that are far enough away
                possibleLocations.AddRange(targetArea.Where(r =>
                        r.Location.GetFloor() == this._entryStaircaseLocation.GetFloor() && 
                        GetDistanceBetween(r.Location, this._entryStaircaseLocation) >= targetDist));
                // If there are no valid locations then lower the target distance
                if (possibleLocations.Count == 0) targetDist -= 2;
                // If the target distance > 0, then there could be more possible locations
                if (targetDist >= 1) continue;
                // If there are no possible rooms in the target area for the
                // staircase, then regenerate the floor and try again
                RegenerateFloor(floor, new List<FloorCoordinate>() {this._entryStaircaseLocation});
                targetArea = GetTargetArea(this._entryStaircaseLocation);
                targetDist = 9;
            }
            
            // Select a random room from the possible locations
            var rand = this._random.Next(possibleLocations.Count);
            var location = possibleLocations[rand].Location;
            var room = this._floorPossibilities.Find( r => r.Location.Equals(location));
            
            // Add a staircase down in that room
            // Down is for progression forwards
            room.Staircase = Direction.Down;
            // If not on the first floor, add a staircase to the previous floor
            if (!this._entryStaircaseLocation.Equals(this._generatedStartCoordinate))
            {
                var entryRoom = targetArea.Find(r => r.Location.Equals(this._entryStaircaseLocation));
                // Up is for returning to previous floors
                entryRoom.Staircase = Direction.Up;
            }
            // Set the entry staircase location for the next floor
            this._entryStaircaseLocation = new FloorCoordinate(room.Location.GetX(),
                room.Location.GetY(), room.Location.GetFloor() + 1);
        }

        private List<PossibleRoom> GetTargetArea(FloorCoordinate location)
        {
            var targetArea = new List<PossibleRoom>();
            
            // Find the area containing the location provided
            foreach (var area in this._areas[location.GetFloor()])
            {
                var startRoom = area.Value.Find(r => r.Location.Equals(location));
                if (startRoom == null) continue;
                targetArea = new List<PossibleRoom>(area.Value);
                break;
            }
            
            // If the target area is found, return it
            if (targetArea.Count != 0) return targetArea;
            
            // If the target area is not found, regenerate the floor and recursively try again
            RegenerateFloor(location.GetFloor(), new List<FloorCoordinate>() {this._entryStaircaseLocation});
            
            return GetTargetArea(location);
        }

        private void RegenerateFloor(int floor, List<FloorCoordinate> toInclude = null)
        {
            // Remove all PossibleRooms for the floor
            this._floorPossibilities.RemoveAll(r => r.Location.GetFloor() == floor);
            for (var col = 0; col < 9; col++)
            {
                for (var row = 0; row < 9; row++)
                {
                    // Add new PossibleRooms for each invalid floor
                    this._floorPossibilities.Add(
                        new PossibleRoom(new FloorCoordinate(col, row, floor)));
                }
            }

            // If there are specific locations to include, ensure they cannot have no doors
            if (toInclude != null)
            {
                foreach (var location in toInclude)
                {
                    var room = this._floorPossibilities.Find(r => r.Location.Equals(location));
                    room.RoomPossibilities.RemoveAll(r => r.PossibleRoomType == PossibleRoomType.Zero);
                }
            }

            // Regenerate the invalid floor
            GenerateFloor(floor);
            WalkThroughFloor(floor, toInclude);
        }

        private void GenerateStartCoordinate()
        {
            // Generate a random coordinate and set it as the start coordinate and entry point
            var startCoordinate = new FloorCoordinate(this._random.Next(9),
                this._random.Next(9), 0);
            this._generatedStartCoordinate = startCoordinate;
            this._entryStaircaseLocation = startCoordinate;
        }

        private int GetDistanceBetween(FloorCoordinate one, FloorCoordinate two)
        {
            // Find the distance between the rooms in terms of coordinates
            var xDist = Math.Abs(one.GetX() - two.GetX());
            var yDist = Math.Abs(one.GetY() - two.GetY());
            return xDist + yDist;
        }

        private void WalkThroughFloor(int floor, List<FloorCoordinate> toInclude)
        {
            // Remove any existing entries for this batch/floor
            if (this._areas.Keys.Count > this._currentFloorBatch + 20) this._areas.Clear();
            if (this._areas.Keys.Contains(floor)) this._areas.Remove(floor);
            // Add an entry for this floor
            this._areas.Add(floor, new Dictionary<int, List<PossibleRoom>>());
            
            var checkedRooms = new List<PossibleRoom>();
            var currentIndex = new Dictionary<int, int>();
            // Loop over each room
            foreach (var room in this._floorPossibilities.Where(r => r.Location.GetFloor() == floor))
            {
                // If the room has already been checked, skip this iteration
                if (checkedRooms.Contains(room)) continue;
                // If the room has no doors, add it to the list of checked rooms
                // and skip the rest of this iteration
                if (room.RoomChoice.PossibleRoomType == PossibleRoomType.Zero)
                {
                    checkedRooms.Add(room);
                    continue;
                }
                // Create a stack of rooms to check
                var toCheck = new Stack<PossibleRoom>();
                PossibleRoom nextRoom;
                // Add the current room to the stack
                toCheck.Push(room);
                do
                {
                    // Pop a room from the stack
                    nextRoom = toCheck.Pop();
                    // If the rooms has already been checked, skip this iteration
                    if (checkedRooms.Contains(nextRoom)) continue;
                    // Add the room to it's area in the dictionary, or a new one if
                    // it's the first room in this area so far
                    if (!currentIndex.Keys.Contains(room.Location.GetFloor())) currentIndex.Add(room.Location.GetFloor(), 0);
                    if (this._areas[nextRoom.Location.GetFloor()].Keys.Contains(currentIndex[room.Location.GetFloor()]))
                        this._areas[nextRoom.Location.GetFloor()][currentIndex[room.Location.GetFloor()]].Add(nextRoom);
                    else this._areas[nextRoom.Location.GetFloor()][currentIndex[room.Location.GetFloor()]] = new List<PossibleRoom>() {nextRoom};
                    checkedRooms.Add(nextRoom);
                    // Get this room's representation in RoomData
                    var roomData = nextRoom.ToRoomData();
                    PossibleRoom adjacentRoom;
                    // If there is a door to the west, and the room to the west hasn't
                    // been checked, push it to the stack of rooms to check
                    if (roomData.WestDoor)
                    {
                        adjacentRoom = this._floorPossibilities.Find(r =>
                            r.Location.GetX() == nextRoom.Location.GetX() - 1 &&
                            r.Location.GetY() == nextRoom.Location.GetY() &&
                            r.Location.GetFloor() == nextRoom.Location.GetFloor());
                        if (adjacentRoom != null)
                        {
                            if (!checkedRooms.Contains(adjacentRoom)) toCheck.Push(adjacentRoom);
                        }
                    }
                    // If there is a door to the south, and the room to the west hasn't
                    // been checked, push it to the stack of rooms to check
                    if (roomData.SouthDoor)
                    {
                        adjacentRoom = this._floorPossibilities.Find(r =>
                            r.Location.GetX() == nextRoom.Location.GetX() &&
                            r.Location.GetY() == nextRoom.Location.GetY() + 1 &&
                            r.Location.GetFloor() == nextRoom.Location.GetFloor());
                        if (adjacentRoom != null)
                        {
                            if (!checkedRooms.Contains(adjacentRoom)) toCheck.Push(adjacentRoom);
                        }
                    }
                    // If there is a door to the east, and the room to the west hasn't
                    // been checked, push it to the stack of rooms to check
                    if (roomData.EastDoor)
                    {
                        adjacentRoom = this._floorPossibilities.Find(r =>
                            r.Location.GetX() == nextRoom.Location.GetX() + 1 &&
                            r.Location.GetY() == nextRoom.Location.GetY() &&
                            r.Location.GetFloor() == nextRoom.Location.GetFloor());
                        if (adjacentRoom != null)
                        {
                            if (!checkedRooms.Contains(adjacentRoom)) toCheck.Push(adjacentRoom);
                        }
                    }
                    // If there is a door to the north, and the room to the west hasn't
                    // been checked, push it to the stack of rooms to check
                    if (roomData.NorthDoor)
                    {
                        adjacentRoom = this._floorPossibilities.Find(r =>
                            r.Location.GetX() == nextRoom.Location.GetX() &&
                            r.Location.GetY() == nextRoom.Location.GetY() - 1 &&
                            r.Location.GetFloor() == nextRoom.Location.GetFloor());
                        if (adjacentRoom != null)
                        {
                            if (!checkedRooms.Contains(adjacentRoom)) toCheck.Push(adjacentRoom);
                        }
                    }
                } while (toCheck.Count > 0);
                // When all rooms in that area have been added to
                // the dictionary, increment the current index
                currentIndex[room.Location.GetFloor()]++;
            }

            // Remove any existing entries for this batch/floor
            if (this._validAreas.Keys.Count > this._currentFloorBatch + 20) this._validAreas.Clear();
            if (this._validAreas.Keys.Contains(floor)) this._validAreas.Remove(floor);
            // Add an entry for this floor
            this._validAreas.Add(floor, new Dictionary<int, bool>());
            // Iterate once for each floor
            // Iterate for each area in the floor
            for (var k = 0; k < this._areas[floor].Count; k++)
            {
                // If the area has 15 or more rooms in it, it is valid,
                // otherwise, it is not valid
                if (this._areas[floor][k].Count >= 15) this._validAreas[floor][k] = true;
                else this._validAreas[floor][k] = false;
            }

            // If there are no specific rooms to include, then return
            // true if there are any valid areas
            if (toInclude == null || toInclude.Count == 0)
            {
                if (this._validAreas[floor].Values.Any(v => v)) return;
            }
            else
            {
                // If specific rooms should be included, return true only if all of them are in a valid area
                var allValid = true;
                foreach (var location in toInclude)
                {
                    var requiredArea = -1;
                    foreach (var area in this._areas[floor].Where(area =>
                                 area.Value.Find(r => r.Location.Equals(location)) != null))
                    {
                        requiredArea = area.Key;
                        break;
                    }

                    if (requiredArea == -1 || this._validAreas[floor][requiredArea] is false) allValid = false;
                }

                if (allValid) return;
            }

            // If the floor is not valid, regenerate it
            RegenerateFloor(floor, toInclude);
        }

        private void InitialiseFloorPossibilities(int floor, List<FloorCoordinate> toInclude = null)
        {
            if (floor == this._currentFloorBatch) this._floorPossibilities.Clear();
            
            // Fill the list with new PossibleRooms, one for each coordinate of each floor
            for (var col = 0; col < 9; col++)
            {
                for (var row = 0; row < 9; row++)
                {
                    this._floorPossibilities.Add(
                        new PossibleRoom(new FloorCoordinate(col, row, floor)));
                }
            }

            if (toInclude is null) return;
            // If there are specific locations to include, ensure they cannot have no doors
            foreach (var location in toInclude)
            {
                var room = this._floorPossibilities.Find(r => r.Location.Equals(location));
                room.RoomPossibilities.RemoveAll(r => r.PossibleRoomType == PossibleRoomType.Zero);
            }
        }

        private void ConvertDataToFloor()
        {
            // Iterate for each floor in this batch
            for (var floor = this._currentFloorBatch; floor < this._currentFloorBatch + 20; floor++)
            {
                // Add a new floor to the dungeon
                this._generatedDungeon[floor] = new List<List<RoomData>>();
                for (var col = 0; col < 9; col++)
                {
                    // Add each column to the floor
                    this._generatedDungeon[floor].Add(new List<RoomData>());
                    for (var row = 0; row < 9; row++)
                    {
                        // Find the PossibleRoom representing this location and get the RoomData representing it
                        var roomChoice = this._floorPossibilities.Find(r => 
                            r.Location.GetX() == col && r.Location.GetY() == row && r.Location.GetFloor() == floor);
                        var roomData = roomChoice.ToRoomData();
                        // Add the RoomData to it's position in the dungeon
                        this._generatedDungeon[floor][col].Add(roomData);
                    }
                }
            }
            
            // Iterate for each floor in this batch
            for (var floor = this._currentFloorBatch; floor < this._currentFloorBatch + 20; floor++)
            {
                foreach (var kv in this._validAreas[floor])
                {
                    foreach (var room in this._areas[floor][kv.Key].Where(room => kv.Value == false))
                    {
                        this._generatedDungeon[room.Location.GetFloor()][room.Location.GetX()][room.Location.GetY()] = new RoomData(null);
                    }
                }
            }
        }

        private void GenerateFloor(int floor)
        {
            // Loop until every floor has been fully generated
            bool done;
            do
            {
                // Get the room with the lowest "entropy"
                var room = PickLowestEntropyPossibleRoom(floor);
                // Collapse that room and propagate
                CollapseFromRoom(room);
                // Check if every room on the floor has been determined
                done = this._floorPossibilities.Where(r => r.Location.GetFloor() == floor).All(possible => possible.Determined);
            } while (!done);
        }

        private PossibleRoom PickLowestEntropyPossibleRoom(int floor)
        {
            var lowestEntropyRooms = new List<PossibleRoom>();
            var lowestEntropyValue = 999;
            // Iterate over all PossibleRooms on the floor
            foreach (var possibleRoom in this._floorPossibilities.Where(r => r.Location.GetFloor() == floor))
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
            if (room.Location.GetX() != 0 && originDir != Direction.West)
            {
                // West Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() - 1 &&
                    r.Location.GetY() == room.Location.GetY() &&
                    r.Location.GetFloor() == room.Location.GetFloor());
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.East);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the south edge of the map, do not check for the room to the south
            if (room.Location.GetY() != 8 && originDir != Direction.South)
            {
                // South Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() &&
                    r.Location.GetY() == room.Location.GetY() + 1 &&
                    r.Location.GetFloor() == room.Location.GetFloor());
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.North);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the east edge of the map, do not check for the room to the east
            if (room.Location.GetX() != 8 && originDir != Direction.East)
            {
                // East Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() + 1 &&
                    r.Location.GetY() == room.Location.GetY() &&
                    r.Location.GetFloor() == room.Location.GetFloor());
                // Create a tuple of the adjacent room and the direction pointing to this room from it
                var nextRoomData = new Tuple<PossibleRoom, Direction>(nextRoom, Direction.West);
                // Push the adjacent room to the stack
                toPropagate.Push(nextRoomData);
            }
            // If along the north edge of the map, do not check for the room to the north
            if (room.Location.GetY() != 0 && originDir != Direction.North)
            {
                // North Room
                var nextRoom = this._floorPossibilities.Find(r =>
                    r.Location.GetX() == room.Location.GetX() &&
                    r.Location.GetY() == room.Location.GetY() - 1 &&
                    r.Location.GetFloor() == room.Location.GetFloor());
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

        public List<List<RoomData>> GetFloor(int floorNumber)
        {
            // If no floors exist, generate the first batch
            if (floorNumber - 1 == 0 && this._generatedDungeon.Count == 0) GenerateNewFloorBatch();
            // If the player is within 10 floors of the current highest floor and floors are not currently
            // being generated, generate a new batch in another thread
            if (DungeonManager.GetInstance().GetCurrentFloorNumber() + 10 >= this._generatedDungeon.Keys.Max() &&
                !this._backgroundFloorGenerationThread.IsAlive) StartBackgroundRoomGenerationThread();
            // Return the requested floor if it exists
            return this._generatedDungeon.Keys.Contains(floorNumber-1) ? this._generatedDungeon[floorNumber-1] : null;
        }

        private void StartBackgroundRoomGenerationThread()
        {
            // Create a new thread which will generate new floor batches until there are
            // at least 20 unexplored floors
            this._backgroundFloorGenerationThread = new Thread(() =>
            {
                while (DungeonManager.GetInstance().GetCurrentDungeonId() == DungeonId.RandomDungeon)
                {
                    if (DungeonManager.GetInstance().GetCurrentFloorNumber() + 20 >= this._generatedDungeon.Keys.Max())
                    {
                        GenerateNewFloorBatch();
                    }
                    else break;
                }
            });
            // Start the thread
            this._backgroundFloorGenerationThread.Start();
        }

        public void ClearDungeon()
        {
            // Reset everything necessary for dungeon generation
            this._generatedDungeon.Clear();
            this._generatedStartCoordinate = null;
            this._currentFloorBatch = 0;
        }
    }
}