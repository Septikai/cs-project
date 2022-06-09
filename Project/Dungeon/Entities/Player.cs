using System.Drawing;

namespace Project.Dungeon.Entities
{
    public class Player : Entity
    {
        private static readonly Player Instance = new Player();

        private Player()
        {
            // Create the player
            SetBackColour();
        }

        public static Player GetInstance()
        {
            // Get the player instance
            return Instance;
        }

        private void SetBackColour()
        {
            // Setting the player colour to blue
            // Done here because it is a virtual member
            this.BackColor = Color.Blue;
        }

        public void SetDimensions(Size availableSpace)
        {
            // Set the player size
            // Player will be a square that is 1/20th of the smaller screen dimension
            var smaller = availableSpace.Height < availableSpace.Width ? availableSpace.Height : availableSpace.Width;
            this.Size = new Size(smaller / 20, smaller / 20);
        }
    }
}