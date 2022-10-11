using System.Drawing;
using System.Windows.Forms;

namespace Project.Combat.Display.Grid
{
    public class GridEntity : PictureBox
    {
        private Point _coordinates;
        
        protected GridEntity()
        {
            // Create a new GridEntity
            this._coordinates = new Point();
        }

        public void SetCoordinates(Point coords)
        {
            // Store the location of the entity
            this._coordinates = coords;
        }

        public Point GetCoordinates()
        {
            // Get the location of the entity
            return this._coordinates;
        }

        public virtual void InitialiseEntity()
        {
            // Initial setup
            this.Anchor = AnchorStyles.None;
        }
    }
}