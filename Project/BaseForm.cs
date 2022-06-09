using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Project.Dungeon;
using Project.Menu;
using static System.Windows.Forms.Application;

namespace Project
{
    public partial class BaseForm : Form
    {
        private static BaseForm _instance;
        private static readonly List<PictureBox> ViewList = new List<PictureBox>();
        public static readonly Main MainMenuInstance = Main.GetInstance();
        public static readonly RoomView RoomViewInstance = RoomView.GetInstance();
        
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

        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Exit the program when the form is closed to prevent it running in the background
            Exit();
        }

        private void BaseForm_Resize(object sender, EventArgs e)
        {
            // If the form is resized, resize the components on it.
            this.SetComponentSizes();
        }
    }
}