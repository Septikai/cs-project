namespace Project.Dungeon
{
    public class FloorCoordinate
    {
        private readonly int _xCoordinate;
        private readonly int _yCoordinate;
        private readonly int _floorNumber;

        public FloorCoordinate(int xCoordinate, int yCoordinate, int floorNumber = -1)
        {
            // Set the X and Y values of the coordinate
            this._xCoordinate = xCoordinate;
            this._yCoordinate = yCoordinate;
            // Set the floor number
            this._floorNumber = floorNumber;
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

        public int GetFloor()
        {
            // Fetch the floor number
            return this._floorNumber;
        }

        public override bool Equals(object obj)
        {
            // Check if two FloorCoordinates are the same
            if (obj == null) return false;
            if (obj.GetType() != typeof(FloorCoordinate)) return false;
            var c = (FloorCoordinate) obj;
            return c.GetFloor() == _floorNumber && c.GetX() == _xCoordinate && c.GetY() == _yCoordinate;
        }

        public override int GetHashCode()
        {
            // When overriding Equals, always override GetHashCode
            // Any two objects that are equal must have the same hashcode
            var result = _floorNumber.GetHashCode();
            result = 31 * result + _xCoordinate.GetHashCode();
            result = 31 * result + _yCoordinate.GetHashCode();
            return result;
        }
    }
}