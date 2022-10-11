using Project.Menu;

namespace Project.Util
{
    public class PlayerStats
    {
        private static readonly PlayerStats Instance = new PlayerStats();
        private const int Speed = 12;
        private const int MaxHealth = 20;
        private int _health = 20;

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
            return Speed;
        }

        public int GetMaxHealth()
        {
            // Get the maximum player health
            return MaxHealth;
        }

        public int GetHealth()
        {
            // Get the player health
            return this._health;
        }

        public bool DamagePlayer(int damage)
        {
            // Deal damage to the player
            if (this._health < damage) this._health = 0;
            else this._health -= damage;
            return this._health == 0;
        }

        public void HealPlayer(int health)
        {
            // Heal the player up to the maximum health
            if (this._health + health >= MaxHealth) this._health = MaxHealth;
            else this._health += health;
        }

        public void PlayerDeath()
        {
            // Display the Death View
            BaseForm.GetInstance().SwitchView(Death.GetInstance());
        }
    }
}