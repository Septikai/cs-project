using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Project.Dungeon.Blockers;
using Project.Dungeon.Entities;
using Project.Util;

namespace Project.Dungeon.Rooms
{
    public class Room : PictureBox
    {
        private Direction _entryDirection;
        private readonly Player _player = Player.GetInstance();
        private readonly List<Wall> _walls = new List<Wall>();
        private int _smallerSize;

        public Room(Direction entryDirection)
        {
            // Construct a room
            this._entryDirection = entryDirection;
            this.InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            // The player must be in the room
            this.Controls.Add(this._player);
            // Add and position the walls
            this._smallerSize = this.Height < this.Width ? this.Height / 20 : this.Width / 20;
            for (var i = 0; i < 4; i++)
            {
                // Create and position 4 walls, one for each side of the screen
                var wall = new Wall();
                if (i % 2 == 0)
                {
                    wall.Width = this.Width;
                    wall.Height = this._smallerSize;
                    wall.Location = i == 0 ? new Point(0, 0) : new Point(0, this.Height - wall.Height);
                    wall.Name = i == 0 ? "NORTH_WALL" : "SOUTH_WALL";
                }
                else
                {
                    wall.Width = this._smallerSize;
                    wall.Height = this.Height;
                    wall.Location = i == 1 ? new Point(this.Width - wall.Width, 0) : new Point(0, 0);
                    wall.Name = i == 1 ? "EAST_WALL" : "WEST_WALL";
                }
                // Add the walls to both the list of walls and the room
                // The list of walls will be used to check for collision and to reposition when the form is resized
                this._walls.Add(wall);
                this.Controls.Add(wall);
            }
        }

        public void SetComponents()
        {
            // Setting the player size
            this._player.SetDimensions(this.Size);
            // Position the walls
            this._smallerSize = this.Height < this.Width ? this.Height / 20 : this.Width / 20;
            foreach (var wall in _walls)
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
        }

        public void EnterRoom(Direction entryPoint = Direction.Centre)
        {
            // Called upon entering the room in order to set it up
            
            // Determines where in the room the player should initially be placed
            this._entryDirection = entryPoint;
            switch (this._entryDirection)
            {
                case Direction.Centre:
                    this._player.Location = new Point(
                        this.Bounds.Width / 2 - this._player.Width / 2,
                        this.Bounds.Height / 2 - this._player.Height / 2);
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

            return blockers;
        }
    }
}