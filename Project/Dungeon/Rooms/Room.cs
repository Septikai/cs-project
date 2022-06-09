using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Project.Dungeon.Entities;
using Project.Util;

namespace Project.Dungeon.Rooms
{
    public class Room : PictureBox
    {
        private Direction _entryDirection;
        private readonly Player _player = Player.GetInstance();
        
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
        }

        public void SetComponents()
        {
            // Setting the player size
            this._player.SetDimensions(this.Size);
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
    }
}