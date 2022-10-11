using System.Drawing;
using System.Windows.Forms;

namespace Project.Combat.Display
{
    public class MainHealthBar : PictureBox
    {
        private readonly int _direction;
        private Label _nameLabel;
        private Label _healthLabel;
        private PictureBox _remainingHealthBox;
        private PictureBox _lostHealthBox;
        private int _maxHealth;
        private int _health;
        
        public MainHealthBar(string name, int maxHealth, int direction = 0)
        {
            // Create a MainHealthBar
            this._maxHealth = maxHealth;
            this._health = maxHealth;
            this._direction = direction;
            InitialiseHealthBar();
            this._nameLabel.Text = name;
        }

        private void InitialiseHealthBar()
        {
            // This label says who the health bar is for
            this._nameLabel = new Label();
            this._nameLabel.Font = new Font(this._nameLabel.Font.Name, this._nameLabel.Font.Size * 3);
            this.Controls.Add(this._nameLabel);
            
            // This label displays the health as a value
            this._healthLabel = new Label();
            this._healthLabel.Font = new Font(this._healthLabel.Font.Name, this._healthLabel.Font.Size * 2);
            this._healthLabel.Text = this._health + @" / " + this._maxHealth;
            this.Controls.Add(this._healthLabel);
            
            // The green PictureBox to display remaining health
            this._remainingHealthBox = new PictureBox();
            this._remainingHealthBox.BackColor = Color.Green;
            this.Controls.Add(this._remainingHealthBox);
            
            // The red PictureBox to display lost health
            this._lostHealthBox = new PictureBox();
            this._lostHealthBox.BackColor = Color.Red;
            this.Controls.Add(this._lostHealthBox);
        }

        public void UpdateBar()
        {
            // Update size and location of each element of the MainHealthBar
            var textSize = TextRenderer.MeasureText(this._nameLabel.Text, this._nameLabel.Font);
            this._nameLabel.Size = new Size(textSize.Width, textSize.Height);
            this._nameLabel.Location = new Point(this._direction == 0 ? 0 : this.Width - this._nameLabel.Width, 0);
            
            textSize = TextRenderer.MeasureText(this._healthLabel.Text, this._healthLabel.Font);
            this._healthLabel.Size = new Size(textSize.Width, textSize.Height);
            this._healthLabel.Location = new Point(this._direction == 0 ? this.Width - this._healthLabel.Width : 0, 
                this._remainingHealthBox.Top - this._healthLabel.Height);
            
            this._remainingHealthBox.Size = new Size(
                (int) (((float) this.Width / this._maxHealth) * this._health),
                (this.Height - this._nameLabel.Height) / 5);
            this._remainingHealthBox.Top = this._nameLabel.Bottom;
            
            this._lostHealthBox.Size = new Size(
                this.Width - this._remainingHealthBox.Width,
                (this.Height - this._nameLabel.Height) / 5);
            this._lostHealthBox.Top = this._nameLabel.Bottom;
            
            // Set the x position so that a health bar on the right is vertically flipped
            if (this._direction == 0)
            {
                this._lostHealthBox.Left = this._remainingHealthBox.Right;
            }
            else
            {
                this._lostHealthBox.Left = 0;
                this._remainingHealthBox.Left = this._lostHealthBox.Right;
            }
        }

        public void SetHealth(int amount)
        {
            // Change the health value of the bar
            this._health = amount;
            this._healthLabel.Text = this._health + @" / " + this._maxHealth;
            UpdateBar();
        }

        public void SetMaxHealth(int amount)
        {
            // Set the health bar's max health
            this._maxHealth = amount;
            this._healthLabel.Text = this._health + @" / " + this._maxHealth;
            UpdateBar();
        }

        public void AddMouseUpEvent(MouseEventHandler clickHandler)
        {
            // Add a click handler to all elements of the health bar
            this._nameLabel.MouseUp += clickHandler;
            this._healthLabel.MouseUp += clickHandler;
            this._remainingHealthBox.MouseUp += clickHandler;
            this._lostHealthBox.MouseUp += clickHandler;
        }
    }
}