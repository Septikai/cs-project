using System.Drawing;
using System.Windows.Forms;

namespace Project.Dungeon.Map
{
    public sealed class MapMarker : PictureBox
    {
        public MapMarker()
        {
            // Create a MapMarker
            this.BackColor = Color.Khaki;
        }

        public void RemoveFromParent()
        {
            // If the remove the MapMarker from the MapRoom
            ((MapRoom) this.Parent)?.RemoveMarker(this);
        }
    }
}