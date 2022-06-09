using System.Collections.Generic;
using System.Windows.Forms;

namespace Project.Util
{
    public class GameTracker
    {
        private static readonly GameTracker Instance = new GameTracker();
        private bool _paused = true;
        private readonly List<Keys> _heldKeys = new List<Keys>();

        private GameTracker()
        {
            // The GameTracker will keep track of anything which does not fit nicely into another class
        }

        public static GameTracker GetInstance()
        {
            // Get the GameTracker instance
            return Instance;
        }

        public void SetPaused(bool isPaused)
        {
            this._paused = isPaused;
        }

        public bool IsPaused()
        {
            return this._paused;
        }

        public void AddHeldKey(Keys key)
        {
            this._heldKeys.Add(key);
        }

        public void RemoveHeldKey(Keys key)
        {
            this._heldKeys.RemoveAll(k => k == key);
        }

        public List<Keys> GetHeldKeys()
        {
            return this._heldKeys;
        }
    }
}