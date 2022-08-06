using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project.Menu
{
    public class ModePanel : Panel
    {
        private readonly Label _modeName;

        public ModePanel(string modeName, Action<object, EventArgs> onClickMethod)
        {
            // Create the label for the mode
            this._modeName = new Label();
            this._modeName.Text = modeName;
            this._modeName.Name = modeName;
            this._modeName.Font = new Font(this._modeName.Font.Name, this._modeName.Font.Size * 5);
            this.Controls.Add(this._modeName);
            
            // Add the method to be ran when the panel is clicked
            this.Click += new EventHandler(onClickMethod);
        }

        public void SetComponents()
        {
            // Resize and reposition the label
            var nameSize = TextRenderer.MeasureText(this._modeName.Text, this._modeName.Font);
            this._modeName.Size = new Size(nameSize.Width, nameSize.Height);
            this._modeName.Location = new Point(this.Width / 2 - this._modeName.Width / 2, this.Height * 9 / 10);
        }
    }
}