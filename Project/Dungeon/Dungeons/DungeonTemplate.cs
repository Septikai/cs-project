using System.Collections.Generic;
using Project.Util;

namespace Project.Dungeon.Dungeons
{
    public abstract class DungeonTemplate
    { 
        // public static readonly DungeonTemplate Instance;
        public DungeonId DungeonId;
        public int FloorCount;
        public FloorCoordinate StartCoordinate;
        public Direction StaircaseDirection = Direction.Up;
        public List<List<RoomData>> FloorOne;
        public List<List<RoomData>> FloorTwo;
        public List<List<RoomData>> FloorThree;
        public List<List<RoomData>> FloorFour;
        public List<List<RoomData>> FloorFive;
        public List<List<RoomData>> FloorSix;
        public List<List<RoomData>> FloorSeven;
        public List<List<RoomData>> FloorEight;
        public List<List<RoomData>> FloorNine;
        public List<List<RoomData>> FloorTen;

        public List<List<RoomData>> GetFloor(int floorNumber)
        {
            // Return the requested floor
            switch (floorNumber)
            {
                case 1:
                    return FloorOne;
                case 2:
                    return FloorTwo;
                case 3:
                    return FloorThree;
                case 4:
                    return FloorFour;
                case 5:
                    return FloorFive;
                case 6:
                    return FloorSix;
                case 7:
                    return FloorSeven;
                case 8:
                    return FloorEight;
                case 9:
                    return FloorNine;
                case 10:
                    return FloorTen;
            }

            return null;
        }

        protected DungeonTemplate()
        {
            
        }

        protected abstract List<List<RoomData>> GenerateFloorOne();
        protected virtual List<List<RoomData>> GenerateFloorTwo()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorThree()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorFour()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorFive()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorSix()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorSeven()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorEight()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorNine()
        {
            return null;
        }
        protected virtual List<List<RoomData>> GenerateFloorTen()
        {
            return null;
        }
        
        // EXAMPLE:
        //
        // protected override List<List<RoomData>> GenerateFloorOne()
        // {
        //     // GenerateFloorOne()[xCoord][yCoord]
        //     // Where coords start at 0 in top left and increase going out
        //     // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
        //     return new List<List<RoomData>>()
        //     {
        //         // col 0
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 1
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 2
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 3
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 4
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 5
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 6
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 7
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         },
        //         // col 8
        //         new List<RoomData>()
        //         {
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null),
        //             new RoomData(null)
        //         }
        //     };
        // }
    }
}