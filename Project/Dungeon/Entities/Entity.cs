using System.Collections.Generic;
using System.Windows.Forms;
using Project.Dungeon.Blockers;
using Project.Dungeon.Rooms;
using Project.Util;

namespace Project.Dungeon.Entities
{
    public class Entity : PictureBox
    {
        private bool _isPlayer;
        private int _speed;

        protected Entity(int speed = 0)
        {
            // Create an entity
            this._isPlayer = this is Player;
            // This speed will only apply if the Entity is not the Player
            // If the entity is the player this will be ignored in favour of PlayerStats.GetInstance().GetSpeed()
            this._speed = speed;
        }

        public bool MoveEntity(int xVel, int yVel, Room room)
        {
            // Method name is self explanatory
            // Takes an X velocity and a Y velocity, each are either 0 or pos/negative 1, and moves in that direction
            this.Left += xVel * (this._isPlayer ? PlayerStats.GetInstance().GetSpeed() : this._speed);
            var xDoorFound = CheckForDoor(room);
            if (xDoorFound) return true;
            var validMoveX = CheckX(xVel, room.GetBlockers());

            this.Top += yVel * (this._isPlayer ? PlayerStats.GetInstance().GetSpeed() : this._speed);
            var yDoorFound = CheckForDoor(room);
            if (yDoorFound) return true;
            var validMoveY = CheckY(yVel, room.GetBlockers());
            RoomView.GetInstance().GetRoom().Invalidate();
            return validMoveX && validMoveY;
        }

        private bool CheckForDoor(Room room)
        {
            var doors = room.GetDoors();
            foreach (var door in doors)
            {
                if (!room.Controls.Contains(door) || !this.Bounds.IntersectsWith(door.Bounds)) continue;
                door.Open();
                return true;
            }

            return false;
        }

        private bool CheckX(int xVel, List<Blocker> blockers)
        {
            // Check for collisions in the X direction
            if (xVel == 0) return true;
            var bounceBackDistance = 0;
            foreach (var blocker in blockers)
            {
                // If the player is not touching the blocker then there is no need to do anything
                if (!blocker.Bounds.IntersectsWith(this.Bounds)) continue;
                // Find out how far into the blocker the player is
                int difference;
                if (xVel > 0)
                {
                    difference = this.Right - blocker.Left;
                }
                else
                {
                    difference = blocker.Right - this.Left;
                }
                // If the player is moving into multiple blockers, track the largest difference
                bounceBackDistance = difference > bounceBackDistance ? difference : bounceBackDistance;
            }
            
            if (bounceBackDistance > 0)
            {
                // Push the player in the opposite direction to where they are going
                if (xVel > 0)
                {
                    this.Left -= bounceBackDistance;
                }
                else
                {
                    this.Left += bounceBackDistance;
                }
            }
            // Return true if there was no collision
            return bounceBackDistance == 0;
        }

        private bool CheckY(int yVel, List<Blocker> blockers)
        {
            // Check for collisions in the Y direction
            if (yVel == 0) return true;
            var bounceBackAmount = 0;
            foreach (var blocker in blockers)
            {
                // If the player is not touching the blocker then there is no need to do anything
                if (!blocker.Bounds.IntersectsWith(this.Bounds)) continue;
                // Find out how far into the blocker the player is
                int difference;
                if (yVel > 0)
                {
                    difference = this.Bottom - blocker.Top;
                }
                else
                {
                    difference = blocker.Bottom - this.Top;
                }

                bounceBackAmount = difference > bounceBackAmount ? difference : bounceBackAmount;
            }

            if (bounceBackAmount > 0)
            {
                // Push the player in the opposite direction to where they are going
                if (yVel > 0)
                {
                    this.Top -= bounceBackAmount;
                }
                else
                {
                    this.Top += bounceBackAmount;
                }
            }
            // Return true if there was no collision
            return bounceBackAmount == 0;
        }
    }
}