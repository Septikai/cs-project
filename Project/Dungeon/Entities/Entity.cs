using System.Windows.Forms;
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

        public bool MoveEntity(int xVel, int yVel)
        {
            this.Left += xVel * (this._isPlayer ? PlayerStats.GetInstance().GetSpeed() : this._speed);
            var validMoveX = CheckX(xVel);

            this.Top += yVel * (this._isPlayer ? PlayerStats.GetInstance().GetSpeed() : this._speed);
            var validMoveY = CheckY(yVel);
            return validMoveX && validMoveY;
        }

        private bool CheckX(int xVel)
        {
            if (xVel == 0) return true;
            
            return true;
        }

        private bool CheckY(int yVel)
        {
            if (yVel == 0) return true;
            
            return true;
        }
    }
}