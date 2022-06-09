using System;
using System.Drawing;
using Project.Dungeon;

namespace Project.Menu
{
    public class Main : View
    {
        private static readonly Main Instance = new Main();
        private bool _initialised = false;
        private MenuButton _newGameButton;
        private MenuButton _loadGameButton;
        private MenuButton _settingsButton;
        private MenuButton _quitButton;
        
        private Main()
        {
            // Constructor for the Main Menu
        }

        public static Main GetInstance()
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
            // Create and setup the components of the Main menu
            
            // Using the MenuButton class, I can pass in the methods to be ran when clicked
            this._newGameButton = new MenuButton("_newGameButton", "Start New Game", NewGame);
            this._loadGameButton = new MenuButton("_loadGameButton", "Load Game", LoadGame);
            this._settingsButton = new MenuButton("_settingsButton", "Settings", Settings);
            this._quitButton = new MenuButton("_quitButton", "Quit Game", Quit);
            
            // Add the components to the Main menu and set their sizes
            this.Controls.Add(_newGameButton);
            this.Controls.Add(_loadGameButton);
            this.Controls.Add(_settingsButton);
            this.Controls.Add(_quitButton);

            this._initialised = true;

            this.SetComponents();
        }

        protected override void SetComponents()
        {
            // Set the sizes and locations of the components of the Main menu
            
            // If the components have not been created, return
            if (!this._initialised) return;
            var bounds = this.Bounds;

            this._newGameButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._newGameButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 6);
            
            this._loadGameButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._loadGameButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 9);
            
            this._settingsButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._settingsButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 12);
            
            this._quitButton.Size = new Size(bounds.Width / 3, bounds.Height / 10);
            this._quitButton.Location = new Point(bounds.Width / 3, (bounds.Height / 20) * 15);
        }

        private void NewGame(object sender, EventArgs e)
        {
            // Start a new game
            DungeonManager.GetInstance().NewGame();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            // Load a saved game
            throw new NotImplementedException();
        }

        private void Settings(object sender, EventArgs e)
        {
            // View and change game settings
            throw new NotImplementedException();
        }

        private void Quit(object sender, EventArgs e)
        {
            // Close the form, which in turn Exits the program
            this.FindForm()?.Close();
        }
    }
}