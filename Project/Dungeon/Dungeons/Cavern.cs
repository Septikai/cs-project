using System.Collections.Generic;
using Project.Util;

namespace Project.Dungeon.Dungeons
{
    public sealed class Cavern : DungeonTemplate
    {
        public static readonly Cavern Instance = new Cavern();
        
        private Cavern()
        {
            // Create the Cavern instance
            this.DungeonId = DungeonId.Cavern;
            this.FloorCount = 5;
            this.StartCoordinate = new FloorCoordinate(7, 2);
            this.StaircaseDirection = Direction.Down;
            this.FloorOne = GenerateFloorOne();
            this.FloorTwo = GenerateFloorTwo();
            this.FloorThree = GenerateFloorThree();
            this.FloorFour = GenerateFloorFour();
            this.FloorFive = GenerateFloorFive();
            this.FloorSix = GenerateFloorSix();
            this.FloorSeven = GenerateFloorSeven();
            this.FloorEight = GenerateFloorEight();
            this.FloorNine = GenerateFloorNine();
            this.FloorTen = GenerateFloorTen();
        }

        protected override List<List<RoomData>> GenerateFloorOne()
        {
            // GenerateFloorOne()[xCoord][yCoord]
            // Where coords start at 0 in top left and increase going out
            // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
            return new List<List<RoomData>>()
            {
                // col 0
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}, Direction.Down),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 1
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East})
                },
                // col 2
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.West})
                },
                // col 3
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 4
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West }),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 5
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 6
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null)
                },
                // col 7
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 8
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                }
            };
        }

        protected override List<List<RoomData>> GenerateFloorTwo()
        {
            // GenerateFloorTwo()[xCoord][yCoord]
            // Where coords start at 0 in top left and increase going out
            // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
            return new List<List<RoomData>>()
            {
                // col 0
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}, Direction.Up),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 1
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North})
                },
                // col 2
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(null)
                },
                // col 3
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null)
                },
                // col 4
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null)
                },
                // col 5
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 6
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North})
                },
                // col 7
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}, Direction.Down),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(null)
                },
                // col 8
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                }
            };
        }

        protected override List<List<RoomData>> GenerateFloorThree()
        {
            // GenerateFloorThree()[xCoord][yCoord]
            // Where coords start at 0 in top left and increase going out
            // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
            return new List<List<RoomData>>()
            {
                // col 0
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North,Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}, Direction.Down)
                },
                // col 1
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West})
                },
                // col 2
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}, Direction.Down),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.West})
                },
                // col 3
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 4
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}, Direction.Down),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 5
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 6
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 7
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}, Direction.Up),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 8
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null)
                }
            };
        }
        
        protected override List<List<RoomData>> GenerateFloorFour()
        {
            // GenerateFloorFour()[xCoord][yCoord]
            // Where coords start at 0 in top left and increase going out
            // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
            return new List<List<RoomData>>()
            {
                // col 0
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}, Direction.Up)
                },
                // col 1
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North})
                },
                // col 2
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}, Direction.Up),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East})
                },
                // col 3
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West})
                },
                // col 4
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South}, Direction.Down),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South}, Direction.Up),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West})
                },
                // col 5
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East})
                },
                // col 6
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.West})
                },
                // col 7
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West})
                },
                // col 8
                new List<RoomData>()
                {
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(new List<Direction>() {Direction.West})
                }
            };
        }
        
        protected override List<List<RoomData>> GenerateFloorFive()
        {
            // GenerateFloorFive()[xCoord][yCoord]
            // Where coords start at 0 in top left and increase going out
            // Therefore the first list of RoomData is the first column of rooms going down if shown on a map
            return new List<List<RoomData>>()
            {
                // col 0
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 1
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 2
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 3
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 4
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}, Direction.Up),
                    new RoomData(new List<Direction>() {Direction.North, Direction.East}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 5
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(new List<Direction>() {Direction.East, Direction.West}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 6
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(new List<Direction>() {Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.South}),
                    new RoomData(new List<Direction>() {Direction.North, Direction.West}),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 7
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                },
                // col 8
                new List<RoomData>()
                {
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null),
                    new RoomData(null)
                }
            };
        }
    }
}