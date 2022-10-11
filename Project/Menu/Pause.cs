using System;
using System.Drawing;
using Project.Combat;
using Project.Dungeon;
using Project.Dungeon.Dungeons;
using Project.Util;

namespace Project.Menu
{
    public class Pause : View
    {
        private static readonly Pause Instance = new Pause();
        private bool _initialised;
        private MenuButton _resumeGameButton;
        private MenuButton _saveGameButton;
        private MenuButton _returnToMenuButton;

        private Pause()
        {
            // Constructor for Pause menu
        }

        public static Pause GetInstance()
        {
            // Get the Pause menu instance
            return Instance;
        }
        
        public void Initialise()
        {
            // Initial setup for the Pause menu
            this.InitialiseComponents();
        }
        
        private void InitialiseComponents()
        {
            // Create and setup the components of the Main menu
            //
            // Using the MenuButton class, I can pass in the methods to be ran when clicked
            this._resumeGameButton = new MenuButton("_resumeGameButton", "Resume Game", ResumeGame);
            this._saveGameButton = new MenuButton("_saveGameButton", "Save Game", SaveGame);
            this._returnToMenuButton = new MenuButton("_returnToMenuButton", "Return To Menu", ReturnToMenu);

            // Add the components to the Main menu and set their sizes
            this.Controls.Add(this._resumeGameButton);
            this.Controls.Add(this._saveGameButton);
            this.Controls.Add(this._returnToMenuButton);

            this._initialised = true;

            this.SetComponents();
        }
        
        protected override void SetComponents()
        {
            // Set the sizes and locations of the components of the Main menu
            
            // If the components have not been created, return
            if (!this._initialised) return;
            var bounds = this.Bounds;

            this._resumeGameButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._resumeGameButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 6);
            
            this._saveGameButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._saveGameButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 9);
            
            this._returnToMenuButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._returnToMenuButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 12);
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            // Check that the pause menu is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Unpause the game
            GameTracker.GetInstance().SetPaused(false, false);
            BaseForm.GetInstance().SwitchView(BaseForm.GetInstance().GetPreviousView());
        }

        private void SaveGame(object sender, EventArgs e)
        {
            // Check that the pause menu is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Save the game
            throw new NotImplementedException();
        }

        private void ReturnToMenu(object sender, EventArgs e)
        {
            // Check that the pause menu is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Return to the Main menu
            DungeonManager.GetInstance().SelectDungeon(DungeonId.NullDungeon);
            if (CombatManager.GetInstance().IsInCombat()) CombatManager.GetInstance().EndCombat();
            BaseForm.GetInstance().SwitchView(Main.GetInstance());
        }
    }
}