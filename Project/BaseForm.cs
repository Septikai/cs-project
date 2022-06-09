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

        public void SwitchView(PictureBox view)
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
            GameTrackerInstance.AddHeldKey(e.KeyData);
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            GameTrackerInstance.RemoveHeldKey(e.KeyData);
        }

        private void MovementTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (GameTrackerInstance.IsPaused()) return;
            var yVel = 0;
            var xVel = 0;
            
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

            Player.GetInstance().MoveEntity(xVel, yVel);
        }
    }
}