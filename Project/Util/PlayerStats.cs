namespace Project.Util
{
    public class PlayerStats
    {
        private static readonly PlayerStats Instance = new PlayerStats();
        private int _speed = 6;

        private PlayerStats()
        {
            // Create the PlayerStats instance
        }

        public static PlayerStats GetInstance()
        {
            return Instance;
        }

        public int GetSpeed()
        {
            return this._speed;
        }
    }
}