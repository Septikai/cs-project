using System.Drawing;

namespace Project.Combat.Display.Grid.Enemy
{
    public class GridSkeleton : GridEnemy
    {
        public GridSkeleton(int health = 10, int damage = 2, int speed = 2) : base(health, damage, speed)
        {
            // Create a basic Skeleton enemy
        }

        public override void InitialiseEntity()
        {
            // Initialise the enemy
            base.InitialiseEntity();
            this.BackColor = Color.Moccasin;
        }
    }
}