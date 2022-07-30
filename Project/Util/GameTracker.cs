using System.Collections.Generic;
using System.Windows.Forms;

namespace Project.Util
{
    public class GameTracker
    {
        private static readonly GameTracker Instance = new GameTracker();
        private bool _paused = true;
        private bool _pauseKeyHeld;
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

        public void SetPaused(bool isPaused, bool triggeredByKeyPress = true)
        {
            // Pause or unpause the game
            this._paused = isPaused;
            if (triggeredByKeyPress) this._pauseKeyHeld = true;
        }

        public bool IsPaused()
        {
            // Check if the game is paused
            return this._paused;
        }
        
        public bool PauseKeyHeld()
        {
            // Check if the game was paused and escape has not been released
            return this._pauseKeyHeld;
        }

        public void AddHeldKey(Keys key)
        {
            // Add a key to the list of held keys
            this._heldKeys.Add(key);
        }

        public void RemoveHeldKey(Keys key)
        {
            // Remove all of a key from the list of held keys
            // Removes all in case of a key appearing multiple times
            this._heldKeys.RemoveAll(k => k == key);
            if (key == Keys.Escape) this._pauseKeyHeld = false;
        }

        public List<Keys> GetHeldKeys()
        {
            // Fetch the list of held keys
            return this._heldKeys;
        }
    }
}