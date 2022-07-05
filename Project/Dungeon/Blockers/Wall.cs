using System.Drawing;

namespace Project.Dungeon.Blockers
{
    public class Wall : Blocker
    {
        public Wall()
        {
            // Create the Wall
            SetBackColour();
        }
        
        private void SetBackColour()
        {
            // Setting the wall colour to dark red
            // Done here because it is a virtual member
            this.BackColor = Color.DarkRed;
        }
    }
}