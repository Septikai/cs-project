using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using Project.Dungeon;
using Project.Dungeon.Entities;
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
        public static readonly RoomView RoomViewInstance = RoomView.GetInstance();
        private static readonly GameTracker GameTrackerInstance = GameTracker.GetInstance();
        private static View _currentView;
        
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
            this.Controls.Add(RoomViewInstance);
            ViewList.Add(RoomViewInstance);
            
            _currentView = MainMenuInstance;
        }

        private void InitialiseComponents()
        {
            // Setup the components on the form
            this.SetComponentSizes();
            
            MainMenuInstance.Initialise();
            // RoomViewInstance.Initialise();
        }

        private  void SetComponentSizes()
        {
            // Set the components to the correct sizes
            MainMenuInstance.Size = this.Size;
            MainMenuInstance.ResizeComponents();
            RoomViewInstance.Size = this.Size;
            RoomViewInstance.ResizeComponents();
        }

        public void SwitchView(View view)
        {
            // Change the view which is currently displayed
            foreach (var v in ViewList)
            {
                if (v.GetType() != view.GetType())
                {
                    v.Hide();
                }
                else
                {
                    v.Show();
                }
            }
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
            Player.GetInstance().MoveEntity(xVel, yVel, BaseForm.RoomViewInstance.GetRoom().GetBlockers());
        }
    }
}