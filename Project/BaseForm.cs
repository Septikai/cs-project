using System;
using System.Drawing;
using System.Windows.Forms;
using Project.Menu;
using static System.Windows.Forms.Application;

namespace Project
{
    public partial class BaseForm : Form
    {
        private static readonly Main MainMenuInstance = Main.GetInstance();
        
        public BaseForm()
        {
            // The BaseForm constructor.
            // This does the initial setup for the form
            this.AddComponents();
            this.InitializeComponent();
            this.InitialiseComponents();
        }

        private void AddComponents()
        {
            // Adds the controls to the form
            this.Controls.Add(MainMenuInstance);
        }

        private void InitialiseComponents()
        {
            // Setup the components on the form
            this.SetComponentSizes();
            
            MainMenuInstance.Initialise();
        }

        private  void SetComponentSizes()
        {
            // Set the components to the correct sizes
            MainMenuInstance.Size = this.Size;
            MainMenuInstance.ResizeComponents();
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