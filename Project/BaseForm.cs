using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using Project.Combat;
using Project.Dungeon;
using Project.Dungeon.Entities;
using Project.Dungeon.Map;
using Project.Menu;
using Project.Util;
using static System.Windows.Forms.Application;

namespace Project
{
    public partial class BaseForm : Form
    {
        private static BaseForm _instance;
        private static readonly List<View> ViewList = new List<View>();
        private static readonly Main MainMenuInstance = Main.GetInstance();
        private static readonly ModeSelect ModeSelectInstance = ModeSelect.GetInstance();
        private static readonly Pause PauseMenuInstance = Pause.GetInstance();
        private static readonly RoomView RoomViewInstance = RoomView.GetInstance();
        private static readonly CombatView CombatViewInstance = CombatView.GetInstance();
        private static readonly Death DeathScreenInstance = Death.GetInstance();
        private static readonly GameTracker GameTrackerInstance = GameTracker.GetInstance();
        private static View _currentView;
        private static View _previousViewInstance;
        
        private BaseForm()
        {
            // The BaseForm constructor.
            // This does the initial setup for the form
            this.AddComponents();
            this.InitializeComponent();
            this.InitialiseComponents();
        }

        public static BaseForm GetInstance()
        {
            // If an instance of BaseForm exists, return it
            // Otherwise, create a new instance of BaseForm, save it to _instance, and return it
            return _instance ?? (_instance = new BaseForm());
        }

        private void AddComponents()
        {
            // Adds the controls to the form
            this.Controls.Add(MainMenuInstance);
            ViewList.Add(MainMenuInstance);
            this.Controls.Add(ModeSelectInstance);
            ViewList.Add(ModeSelectInstance);
            this.Controls.Add(PauseMenuInstance);
            ViewList.Add(PauseMenuInstance);
            this.Controls.Add(RoomViewInstance);
            ViewList.Add(RoomViewInstance);
            this.Controls.Add(CombatViewInstance);
            ViewList.Add(CombatViewInstance);
            this.Controls.Add(DeathScreenInstance);
            ViewList.Add(DeathScreenInstance);
            
            _currentView = MainMenuInstance;
        }

        private void InitialiseComponents()
        {
            // Setup the components on the form
            this.SetComponentSizes();
            
            MainMenuInstance.Initialise();
            ModeSelectInstance.Initialise();
            PauseMenuInstance.Initialise();
            DeathScreenInstance.Initialise();
        }

        private void SetComponentSizes()
        {
            // Set the components to the correct sizes
            MainMenuInstance.Size = this.Size;
            MainMenuInstance.ResizeComponents();
            ModeSelectInstance.Size = this.Size;
            ModeSelectInstance.ResizeComponents();
            PauseMenuInstance.Size = this.Size;
            PauseMenuInstance.ResizeComponents();
            RoomViewInstance.Size = this.Size;
            RoomViewInstance.ResizeComponents();
            CombatViewInstance.Size = this.Size;
            CombatViewInstance.ResizeComponents();
            DeathScreenInstance.Size = this.Size;
            DeathScreenInstance.ResizeComponents();
        }

        public void SwitchView(View view)
        {
            // Change the view which is currently displayed
            //
            // Set the previous view
            _previousViewInstance = _currentView;
            foreach (var v in ViewList)
            {
                // Show the required view
                if (v.GetType() != view.GetType())
                {
                    v.Hide();
                }
                else
                {
                    // Set the new current view
                    _currentView = v;
                    v.Show();
                }
            }
        }
        
        public View GetCurrentView()
        {
            return _currentView;
        }
        
        public View GetPreviousView()
        {
            return _previousViewInstance;
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            // Exit the program when the form is closed to prevent it running in the background
            Exit();
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            // If the form is resized, resize the components on it.
            this.SetComponentSizes();
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            // When a key is pressed while the BaseForm is the current window,
            // add it to the list of held keys
            GameTrackerInstance.AddHeldKey(e.KeyData);
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            // When a key is released while the BaseForm is the current window,
            // remove it from the list of held keys
            GameTrackerInstance.RemoveHeldKey(e.KeyData);
        }

        private void MovementTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Check to see if a movement key is being held, and if so, move the player
            //
            // The player should not move if the game is paused
            if (GameTrackerInstance.IsPaused()) return;
            // The player should not move if the map is open
            if (MapBackground.GetInstance().Visible) return;
            // The player should not move unless in a room
            if (_currentView.GetType() != typeof(RoomView)) return;
            var yVel = 0;
            var xVel = 0;
            
            // Modify velocity variables based on held keys to find the resulting directions required
            if (GameTrackerInstance.GetHeldKeys().Contains(Keys.W))
            {
                yVel--;
            }
            if (GameTrackerInstance.GetHeldKeys().Contains(Keys.A))
            {
                xVel--;
            }
            if (GameTrackerInstance.GetHeldKeys().Contains(Keys.S))
            {
                yVel++;
            }
            if (GameTrackerInstance.GetHeldKeys().Contains(Keys.D))
            {
                xVel++;
            }

            // Move in the direction of the resulting velocities
            Player.GetInstance().MoveEntity(xVel, yVel, BaseForm.RoomViewInstance.GetRoom());
        }

        private void MiscTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Checks if the user presses escape while not in the main menu
            if (_currentView.GetType() != typeof(Main) && _currentView.GetType() != typeof(ModeSelect) &&
                GameTrackerInstance.GetHeldKeys().Contains(Keys.Escape) && !GameTrackerInstance.PauseKeyHeld())
            {
                // If you are paused, resume the game, if you are not paused, pause the game
                if (GameTrackerInstance.IsPaused())
                {
                    GameTrackerInstance.SetPaused(false);
                    this.SwitchView(_previousViewInstance);
                }
                else
                {
                    GameTrackerInstance.SetPaused(true);
                    this.SwitchView(PauseMenuInstance);
                }
            }
            // Checks if the user presses escape while in the ModeSelect
            else if (_currentView is ModeSelect && GameTrackerInstance.GetHeldKeys().Contains(Keys.Escape) &&
                     !GameTrackerInstance.PauseKeyHeld())
            {
                // Return to the main menu
                GameTrackerInstance.SetPauseKeyHeld(true);
                this.SwitchView(MainMenuInstance);
            }
            // Checks if the user is holding C and is in the RoomView
            // If not and the map is visible, hide the map
            if (_currentView is RoomView && GameTrackerInstance.GetHeldKeys().Contains(Keys.C))
            {
                if (!MapBackground.GetInstance().Visible) MapBackground.GetInstance().Show();
            }
            else if (MapBackground.GetInstance().Visible)
            {
                MapBackground.GetInstance().Hide();
            }
        }
    }
}