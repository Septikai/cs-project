using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Project.Dungeon.Blockers;
using Project.Dungeon.Entities;
using Project.Dungeon.Map;
using Project.Util;

namespace Project.Dungeon.Rooms
{
    public class Room : PictureBox
    {
        private Direction _entryDirection;
        private readonly Player _player = Player.GetInstance();
        private readonly List<Wall> _walls = new List<Wall>();
        private readonly List<Door> _doors = new List<Door>();
        private int _smallerSize;
        private readonly List<Direction> _doorLocations = new List<Direction>();
        private Staircase _staircase;

        public Room(Direction entryDirection)
        {
            // Construct a room
            this._entryDirection = entryDirection;
            this.InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            // The map should display over everything else
            MapBackground.GetInstance().Size = new Size(this.Width * 8 / 10, this.Height * 8 / 10);
            MapBackground.GetInstance().Location = new Point(this.Width * 1 / 10, this.Height * 1 / 10);
            MapBackground.GetInstance().SetComponents();
            MapBackground.GetInstance().Hide();
            this.Controls.Add(MapBackground.GetInstance());
            // The player must be in the room
            this.Controls.Add(this._player);
            // Add and position the walls
            this._smallerSize = this.Height < this.Width ? this.Height / 20 : this.Width / 20;
            for (var i = 0; i < 4; i++)
            {
                // Create and position 4 walls and doors, one for each side of the screen
                var wall = new Wall();
                var door = new Door(this);
                if (i % 2 == 0)
                {
                    wall.Width = this.Width;
                    wall.Height = this._smallerSize;
                    wall.Location = i == 0 ? new Point(0, 0) : new Point(0, this.Height - wall.Height);
                    wall.Name = i == 0 ? "NORTH_WALL" : "SOUTH_WALL";
                    door.Width = this._smallerSize * 2;
                    door.Height = this._smallerSize;
                    if (i == 0)
                    {
                        door.Location = new Point(this.Width / 2 - door.Width / 2, 0);
                        door.Name = "NORTH_DOOR";
                        door.SetLocation(Direction.North);
                        if (this._doorLocations.Contains(Direction.North)) this.Controls.Add(door);
                    }
                    else
                    {
                        door.Location = new Point(this.Width / 2 - door.Width / 2, this.Height - door.Height);
                        door.Name = "SOUTH_DOOR";
                        door.SetLocation(Direction.South);
                        if (this._doorLocations.Contains(Direction.South)) this.Controls.Add(door);
                    }
                }
                else
                {
                    wall.Width = this._smallerSize;
                    wall.Height = this.Height;
                    wall.Location = i == 1 ? new Point(this.Width - wall.Width, 0) : new Point(0, 0);
                    wall.Name = i == 1 ? "EAST_WALL" : "WEST_WALL";
                    door.Width = this._smallerSize;
                    door.Height = this._smallerSize * 2;
                    if (i == 1)
                    {
                        door.Location = new Point(this.Width - door.Width, this.Height / 2 - door.Height / 2);
                        door.Name = "EAST_DOOR";
                        door.SetLocation(Direction.East);
                        if (this._doorLocations.Contains(Direction.East)) this.Controls.Add(door);
                    }
                    else
                    {
                        door.Location = new Point(0, this.Height / 2 - door.Height / 2);
                        door.Name = "WEST_DOOR";
                        door.SetLocation(Direction.West);
                        if (this._doorLocations.Contains(Direction.West)) this.Controls.Add(door);
                    }
                }
                // Add the walls to both the list of walls and the room
                // The list of walls will be used to check for collision and to reposition when the form is resized
                this._walls.Add(wall);
                this.Controls.Add(wall);
                this._doors.Add(door);
                door.BringToFront();

                // Add a staircase
                this._staircase = new Staircase();
                this._staircase.Size = new Size(this._smallerSize * 3, this._smallerSize * 3);
                this._staircase.Location = new Point(
                    this.Width / 2 - this._staircase.Width / 2,
                    this.Height / 2 - this._staircase.Height / 2);
            }
        }

        public void SetComponents()
        {
            // Resize the map and all components on it
            MapBackground.GetInstance().Size = new Size(this.Width * 8 / 10, this.Height * 8 / 10);
            MapBackground.GetInstance().Location = new Point(this.Width * 1 / 10, this.Height * 1 / 10);
            MapBackground.GetInstance().SetComponents();
            // Setting the player size
            this._player.SetDimensions(this.Size);
            // Position the walls
            this._smallerSize = this.Height < this.Width ? this.Height / 20 : this.Width / 20;
            foreach (var wall in this._walls)
            {
                // Walls should be positioned differently to cover all 4 sides
                switch (wall.Name)
                {
                    case "NORTH_WALL":
                        wall.Width = this.Width;
                        wall.Height = this._smallerSize;
                        wall.Location = new Point(0, 0);
                        break;
                    case "EAST_WALL":
                        wall.Width = this._smallerSize;
                        wall.Height = this.Height;
                        wall.Location = new Point(this.Width - wall.Width, 0);
                        break;
                    case "SOUTH_WALL":
                        wall.Width = this.Width;
                        wall.Height = this._smallerSize;
                        wall.Location = new Point(0, this.Height - wall.Height);
                        break;
                    case "WEST_WALL":
                        wall.Width = this._smallerSize;
                        wall.Height = this.Height;
                        wall.Location = new Point(0, 0);
                        break;
                }
            }
            
            foreach (var door in this._doors)
            {
                // Walls should be positioned differently to cover all 4 sides
                door.BringToFront();
                switch (door.Name)
                {
                    case "NORTH_DOOR":
                        door.Width = this._smallerSize * 2;
                        door.Height = this._smallerSize;
                        door.Location = new Point(this.Width / 2 - door.Width / 2, 0);
                        break;
                    case "EAST_DOOR":
                        door.Width = this._smallerSize;
                        door.Height = this._smallerSize * 2;
                        door.Location = new Point(this.Width - door.Width, this.Height / 2 - door.Height / 2);
                        break;
                    case "SOUTH_DOOR":
                        door.Width = this._smallerSize * 2;
                        door.Height = this._smallerSize;
                        door.Location = new Point(this.Width / 2 - door.Width / 2, this.Height - door.Height);
                        break;
                    case "WEST_DOOR":
                        door.Width = this._smallerSize;
                        door.Height = this._smallerSize * 2;
                        door.Location = new Point(0, this.Height / 2 - door.Height / 2);
                        break;
                }
            }

            // Reposition the staircase
            this._staircase.Size = new Size(this._smallerSize * 3, this._smallerSize * 3);
            this._staircase.Location = new Point(
                this.Width / 2 - this._staircase.Width / 2,
                this.Height / 2 - this._staircase.Height / 2);
        }

        public void LoadRoomData(RoomData roomData)
        {
            if (roomData.NorthDoor) this._doorLocations.Add(Direction.North);
            else this._doorLocations.RemoveAll(d => d == Direction.North);
            if (roomData.EastDoor) this._doorLocations.Add(Direction.East);
            else this._doorLocations.RemoveAll(d => d == Direction.East);
            if (roomData.SouthDoor) this._doorLocations.Add(Direction.South);
            else this._doorLocations.RemoveAll(d => d == Direction.South);
            if (roomData.WestDoor) this._doorLocations.Add(Direction.West);
            else this._doorLocations.RemoveAll(d => d == Direction.West);
            // Add and remove doors from the form as necessary
            foreach (var door in this._doors)
            {
                if (this._doorLocations.Contains(door.GetLocation()))
                {
                    // Bring the door to the front so it isn't hidden behind the wall
                    if (!this.Controls.Contains(door)) this.Controls.Add(door);
                    door.BringToFront();
                }
                else
                {
                    if (this.Controls.Contains(door)) this.Controls.Remove(door);
                }
            }

            // If the staircase should be in the room, add it to controls and set it's image
            // Otherwise, remove it from controls
            if (roomData.StaircaseDirection != Direction.NullDirection)
            {
                this._staircase.SetImage(roomData.StaircaseDirection);
                this.Controls.Add(this._staircase);
            }
            else if (this.Controls.Contains(this._staircase)) this.Controls.Remove(this._staircase);
        }

        public void EnterRoom(Direction entryPoint = Direction.Centre, Direction entryDirection = Direction.NullDirection)
        {
            // Called upon entering the room in order to set it up
            //
            // Determines where in the room the player should initially be placed
            this._entryDirection = entryPoint;
            switch (this._entryDirection)
            {
                case Direction.North:
                    this._player.Location = new Point(
                        this.Bounds.Width / 2 - this._player.Width / 2,
                        _smallerSize + 10
                    );
                    break;
                case Direction.East:
                    this._player.Location = new Point(
                        this.Width - _smallerSize - this._player.Width - 10,
                        this.Bounds.Height / 2 - this._player.Height / 2
                    );
                    break;
                case Direction.South:
                    this._player.Location = new Point(
                        this.Bounds.Width / 2 - this._player.Width / 2,
                        this.Height - _smallerSize - this._player.Height - 10
                    );
                    break;
                case Direction.West:
                    this._player.Location = new Point(
                        _smallerSize + 10,
                        this.Bounds.Height / 2 - this._player.Height / 2
                    );
                    break;
                case Direction.Centre:
                    this._player.Location = new Point(
                        this.Bounds.Width / 2 - this._player.Width / 2,
                        this.Bounds.Height / 2 - this._player.Height / 2
                        );
                    // Determine which side of the staircase the player should be placed
                    switch (entryDirection)
                    {
                        case Direction.North:
                            this._player.Top = this._staircase.Top - this._player.Height - this._smallerSize / 2;
                            break;
                        case Direction.East:
                            this._player.Left = this._staircase.Right + this._smallerSize / 2;
                            break;
                        case Direction.South:
                            this._player.Top = this._staircase.Bottom + this._smallerSize / 2;
                            break;
                        case Direction.West:
                            this._player.Left = this._staircase.Left - this._player.Width - this._smallerSize / 2;
                            break;
                    }
                    break;
            }
        }

        public List<Blocker> GetBlockers()
        {
            // Collect all blockers in the room into a list and return it
            var blockers = new List<Blocker>();
            foreach (var wall in this._walls)
            {
                blockers.Add(wall);
            }

            foreach (var door in this._doors)
            {
                blockers.Add(door);
            }
            
            if (this.Controls.Contains(this._staircase)) blockers.Add(this._staircase);

            return blockers;
        }

        public List<Door> GetDoors()
        {
            // Return the list of doors
            return this._doors;
        }

        public Staircase GetStaircase()
        {
            // Return the staircase
            return this._staircase;
        }
    }
}