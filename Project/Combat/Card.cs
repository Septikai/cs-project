using System.Drawing;
using System.Windows.Forms;
using Project.Combat.Display.Grid;

namespace Project.Combat
{
    public class Card : PictureBox
    {
        private readonly string _cardId;
        private readonly int _range;
        private readonly int _damage;
        private readonly int _rechargeTime;
        private int _remainingRechargeTime;

        public Card(string cardId, Image image, int damage, int range, int rechargeTime)
        {
            // Create a card
            this._cardId = cardId;
            this.Image = image;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this._damage = damage;
            this._range = range;
            this._rechargeTime = rechargeTime;
            this._remainingRechargeTime = 0;
            // Add the event to colour the card based on recharge time
            this.Paint += PaintRechargingCard;
        }

        private Color? GetRechargeColour()
        {
            // Select which colour should be painted over the card
            switch (this._remainingRechargeTime)
            {
                case 1: return Color.Green;
                case 2: return Color.Yellow;
                case var n when (n >= 3): return Color.Red;
                default: return null;
            }
        }

        private void PaintRechargingCard(object sender, PaintEventArgs e)
        {
            // Paint a colour over the card
            var colour = GetRechargeColour();
            if (colour == null) return;
            using (var b = new SolidBrush(Color.FromArgb(128, (Color) colour)))
            {
                e.Graphics.FillRectangle(b, 0, 0, this.Width, this.Height);
            }
            // Display the turns until recharged on the card
            var font = new Font(FontFamily.GenericMonospace, 50);
            var stringSize = TextRenderer.MeasureText(this._remainingRechargeTime.ToString(), font);
            using (var b = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(this._remainingRechargeTime.ToString(), font, b,
                    this.Width / 2f - stringSize.Width / 2f, this.Height / 2f - stringSize.Height / 2f);
            }
        }

        public string GetCardId()
        {
            // Get the card Id
            return this._cardId;
        }

        public int GetRange()
        {
            // Get the card range
            return this._range;
        }

        public int GetDamage()
        {
            // Get the card damage
            return this._damage;
        }

        public int GetRemainingRechargeTime()
        {
            // Get the remaining time until the card is recharged
            return this._remainingRechargeTime;
        }

        public void UseCard()
        {
            // Use the card - this is what makes the card need to recharge
            var originalRemainingRechargeTime = this._remainingRechargeTime;
            this._remainingRechargeTime = this._rechargeTime;
            if (originalRemainingRechargeTime == 0) this.Refresh();
        }

        public void Recharge()
        {
            // Lower the remaining time until recharged by 1
            if (this._remainingRechargeTime <= 0) return;
            this._remainingRechargeTime--;
            this.Refresh();
        }

        public void SetRemainingRechargeTime(int time)
        {
            // Set the remaining recharge time
            this._remainingRechargeTime = time;
        }
        
        public void Play(int choice)
        {
            // Play the card
            if (choice == 0)
            {
                GridPlayer.GetInstance().StartMove(this.GetRange());
            }
            else
            {
                if (this.GetDamage() >= 0 && !CombatManager.GetInstance().IsAttacking())
                {
                    GridPlayer.GetInstance().StartAttack(this.GetRange());
                }
                else if (this.GetDamage() < 0 && !CombatManager.GetInstance().IsHealing())
                {
                    GridPlayer.GetInstance().StartHeal(this.GetRange());
                }
            }
        }
    }
}