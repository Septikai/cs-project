namespace Project.Dungeon
{
    public class FloorCoordinate
    {
        private readonly int _xCoordinate;
        private readonly int _yCoordinate;

        public FloorCoordinate(int xCoordinate, int yCoordinate)
        {
            // Set the X and Y values of the coordinate
            this._xCoordinate = xCoordinate;
            this._yCoordinate = yCoordinate;
        }

        public int GetX()
        {
            // Fetch the X coordinate
            return this._xCoordinate;
        }

        public int GetY()
        {
            // Fetch the Y coordinate
            return this._yCoordinate;
        }
    }
}