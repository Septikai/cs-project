using System;
using System.Drawing;
using Project.Dungeon;

namespace Project.Menu
{
    public class ModeSelect : View
    {
        private static readonly ModeSelect Instance = new ModeSelect();
        private bool _initialised;
        private ModePanel _storyModePanel;
        private ModePanel _endlessModePanel;

        private ModeSelect()
        {
            // Create the ModeSelect View
        }

        public static ModeSelect GetInstance()
        {
            // Get the ModeSelect
            return Instance;
        }
        
        public void Initialise()
        {
            // Initial setup for the ModeSelect
            this.InitialiseComponents();
        }
        
        private void InitialiseComponents()
        {
            // Create and setup the components of the ModeSelect view
            //
            // Using the ModePanel class, I can pass in the methods to be ran when clicked
            this._storyModePanel = new ModePanel("Story Mode", SelectStoryMode);
            this._endlessModePanel = new ModePanel("Endless Mode", SelectEndlessMode);
            this._storyModePanel.BackColor = Color.RoyalBlue;
            this._endlessModePanel.BackColor = Color.Firebrick;
            
            // Add the components to the view and set their sizes
            this.Controls.Add(this._storyModePanel);
            this.Controls.Add(this._endlessModePanel);
            
            this._initialised = true;

            this.SetComponents();
        }
        
        protected override void SetComponents()
        {
            // Set the sizes and locations of the components of the ModeSelect view
            
            // If the components have not been created, return
            if (!this._initialised) return;
            
            this._storyModePanel.Size = new Size(this.Width / 2, this.Height);
            this._storyModePanel.Location = new Point(0, 0);
            this._storyModePanel.SetComponents();
            
            this._endlessModePanel.Size = new Size(this.Width / 2, this.Height);
            this._endlessModePanel.Location = new Point(this.Width / 2, 0);
            this._endlessModePanel.SetComponents();
        }

        private void SelectStoryMode(object sender, EventArgs e)
        {
            // Check that the mode selector is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Start a new game with story mode
            DungeonManager.GetInstance().NewGame();
        }

        private void SelectEndlessMode(object sender, EventArgs e)
        {
            // Check that the mode selector is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Start a new game with endless mode
            DungeonManager.GetInstance().NewGame(false);
        }
    }
}