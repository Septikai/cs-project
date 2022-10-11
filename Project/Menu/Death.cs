using System;
using System.Drawing;
using System.Windows.Forms;
using Project.Combat;
using Project.Dungeon;
using Project.Dungeon.Dungeons;

namespace Project.Menu
{
    public class Death : View
    {
        private static readonly Death Instance = new Death();
        private bool _initialised;
        private MenuButton _loadGameButton;
        private MenuButton _returnToMenuButton;
        private Label _deathLabel;
        
        private Death()
        {
            // Constructor for the Death Screen
        }

        public static Death GetInstance()
        {
            // Get the Main menu
            return Instance;
        }

        public void Initialise()
        {
            // Initial setup for the Main menu
            this.InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            // Create and setup the components of the Death screen
            //
            // Using the MenuButton class, I can pass in the methods to be ran when clicked
            this._loadGameButton = new MenuButton("_loadGameButton", "Load Game", LoadGame);
            this._returnToMenuButton = new MenuButton("_quitButton", "Quit Game", ReturnToMenu);

            // Create the label to tell the player they died
            this._deathLabel = new Label();
            this._deathLabel.Text = @"You Died";
            this._deathLabel.Font = new Font(this._deathLabel.Font.Name, this._deathLabel.Font.Size * 8);
            
            // Add the components to the Death screen and set their sizes
            this.Controls.Add(this._loadGameButton);
            this.Controls.Add(this._returnToMenuButton);
            this.Controls.Add(this._deathLabel);

            this._initialised = true;

            this.SetComponents();
        }

        protected override void SetComponents()
        {
            // Set the sizes and locations of the components of the Main menu
            
            // If the components have not been created, return
            if (!this._initialised) return;
            var bounds = this.Bounds;
            
            this._loadGameButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._loadGameButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 8);
            
            this._returnToMenuButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._returnToMenuButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 12);
            
            var textSize = TextRenderer.MeasureText(this._deathLabel.Text, this._deathLabel.Font);
            this._deathLabel.Size = new Size(textSize.Width, textSize.Height);
            this._deathLabel.Location = new Point((bounds.Width - this._deathLabel.Width) / 2, (bounds.Height / 20) * 4);
        }

        private void LoadGame(object sender, EventArgs e)
        {
            // Check that the Death screen is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Load a saved game
            throw new NotImplementedException();
        }

        private void ReturnToMenu(object sender, EventArgs e)
        {
            // Check that the Death screen is visible
            if (BaseForm.GetInstance().GetCurrentView() != Instance) return;
            // Return to the Main menu
            DungeonManager.GetInstance().SelectDungeon(DungeonId.NullDungeon);
            if (CombatManager.GetInstance().IsInCombat()) CombatManager.GetInstance().EndCombat();
            BaseForm.GetInstance().SwitchView(Main.GetInstance());
        }
    }
}