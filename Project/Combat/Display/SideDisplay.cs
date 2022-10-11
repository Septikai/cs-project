using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project.Properties;

namespace Project.Combat.Display
{
    public sealed class SideDisplay : PictureBox
    {
        private readonly int _location;
        private int _sideBorder;
        private Size _containerSize;

        public SideDisplay(string name, int location)
        {
            // Create the SideDisplay
            this.Name = name;
            this._location = location;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.MouseUp += SideDisplayMouseUpDuringCombat;
        }
        
        private void SideDisplayMouseUpDuringCombat(object sender, EventArgs e)
        {
            // If in combat with a selected card, play the card
            if (!CombatManager.GetInstance().IsInCombat()) return;
            if (CardManager.GetSelectedCard() is null) return;
            var me = (MouseEventArgs) e;
            if (me.Button.Equals(MouseButtons.Left))
            {
                CardManager.GetSelectedCard().Play(_location);
            }
        }

        public void DisplayAction(Card card)
        {
            // Display the appropriate action image
            this.Image = this._location == 0 ? Resources.move_selector :
                card.GetDamage() >= 0 ? Resources.attack_selector : Resources.heal_selector;
            ResizeDisplay();
        }

        public void HideAction(Card card)
        {
            // Remove the image from the display
            this.Image = null;
        }

        public void DisplayCard(string id)
        {
            // Set the appropriate image to reflect the given card
            switch (id)
            {
                case null:
                    this.Image = null;
                    return;
                case "DISCARD":
                    this.Image = CardManager.GetCard(CardManager.GetInstance().GetDiscard().Last()).Image;
                    break;
                default:
                    this.Image = (Image) CardManager.GetCardDictionary()[id][1];
                    break;
            }
            ResizeDisplay();
        }

        private void ResizeDisplay()
        {
            // Resize and reposition the display to fit the image
            var scaleFactor = (this._sideBorder - ((this._sideBorder / 40d) * 2)) / this.Image.Width;
            this.Size = new Size(
                (int) (this.Image.Width * scaleFactor),
                (int) (this.Image.Height * scaleFactor));
            // _location determined which side of the screen the display is on
            if (this._location == 0)
            {
                this.Location = new Point(
                    (this._sideBorder - this.Width) / 2,
                    (this._containerSize.Height / 2) - (this.Height / 2));
            }
            else
            {
                this.Location = new Point(
                    this._containerSize.Width - this.Width - (this._sideBorder - this.Width) / 2,
                    (this._containerSize.Height / 2) - (this.Height / 2));
            }
        }

        public void SetSideBorder(int size)
        {
            // Set the available width
            this._sideBorder = size;
        }

        public void SetContainerSize(Size size)
        {
            // Set the size of the CombatView
            this._containerSize = size;
        }
    }
}