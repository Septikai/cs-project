namespace Project.Util
{
    public class PlayerStats
    {
        private static readonly PlayerStats Instance = new PlayerStats();
        private int _speed = 12;

        private PlayerStats()
        {
            // Create the PlayerStats instance
        }

        public static PlayerStats GetInstance()
        {
            // Get the PlayerStats instance
            return Instance;
        }

        public int GetSpeed()
        {
            // Get the player speed
            return this._speed;
        }
    }
}