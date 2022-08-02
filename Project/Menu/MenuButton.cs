using System;
using System.Windows.Forms;

namespace Project.Menu
{
    public class MenuButton : Button
    {
        public MenuButton(string name, string displayText, Action<object, EventArgs> onClickMethod)
        {
            // Construct the MenuButton
            this.Name = name;
            this.SetText(displayText);
            this.Click += new EventHandler(onClickMethod);
            this.TabStop = false;
        }

        private void SetText(string text)
        {
            // Setting a virtual member in constructor is bad so it's set here instead
            this.Text = text;
        }
    }
}