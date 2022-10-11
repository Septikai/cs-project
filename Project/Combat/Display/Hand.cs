using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project.Properties;

namespace Project.Combat.Display
{
    public class Hand
    {
        private static readonly Hand Instance = new Hand(); 
        private readonly List<Card> _hand = new List<Card>();
        private const int HandSize = 5;
        private PictureBox _deckBox;
        private PictureBox _discardBox;
        private int _maxHeight;
        private int _sideBorder;
        private Size _containerSize;

        private Hand()
        {
            // Initialise the Hand
            Initialise();
        }

        public static Hand GetInstance()
        {
            // Get the Hand instance
            return Instance;
        }

        private void Initialise()
        {
            // Initialise the deck and discard
            this._deckBox = new PictureBox();
            this._deckBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this._deckBox.Image = Resources.deck_3;

            this._discardBox = new PictureBox();
            this._discardBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this._discardBox.Image = Resources.empty_card_pile;
        }

        public void ClearHand()
        {
            // Empty the hand of cards
            this._hand.Clear();
        }

        public void AddToHand(Card card)
        {
            // Add a card to the hand
            this._hand.Add(card);
        }

        public void RemoveFromHand(Card card)
        {
            // Remove a card from the hand
            this._hand.Remove(card);
        }
        
        public List<Card> GetHand()
        {
            // Get the hand
            return this._hand;
        }

        public static int GetHandSize()
        {
            // Get the hand size
            return HandSize;
        }
        
        public void ResetCooldowns()
        {
            // Reset the cooldowns of all cards in the hand
            foreach (var card in this._hand)
            {
                card.SetRemainingRechargeTime(0);
            }
        }

        private void SetDeckImage()
        {
            // Set the image to be displayed on the deck
            if (CardManager.GetInstance().GetDeck().Count / 5 >= 3)
            {
                this._deckBox.Image = Resources.deck_3;
            }
            else if (CardManager.GetInstance().GetDeck().Count / 5 == 2)
            {
                this._deckBox.Image = Resources.deck_2;
            }
            else if (CardManager.GetInstance().GetDeck().Count / 5 == 1)
            {
                this._deckBox.Image = Resources.deck_1;
            }
            else
            {
                this._deckBox.Image = Resources.empty_card_pile;
            }
        }

        private void SetDiscardImage()
        {
            // Set the image to be displayed on the discard
            if (CardManager.GetInstance().GetDiscard().Count > 0)
            {
                var card = CardManager.GetCard(CardManager.GetInstance().GetDiscard().Last());
                this._discardBox.Image = card.Image;
            }
            else this._discardBox.Image = Resources.empty_card_pile;
        }

        public void DisplayCards()
        {
            // Display the deck
            DisplayDeck();
            // Display the discard pile
            DisplayDiscard();
            // Display the hand
            DisplayHand();
        }

        private void DisplayDeck()
        {
            // Set the deck image, size and location
            SetDeckImage();
            var sf = (float) this._maxHeight / this._deckBox.Image.Height;
            this._deckBox.Size = new Size((int) (this._deckBox.Image.Width * sf), (int) (this._deckBox.Image.Height * sf));
            this._deckBox.Location = new Point(0, this._containerSize.Height - this._deckBox.Height);
        }

        private void DisplayDiscard()
        {
            // Set the discard image, size and location
            SetDiscardImage();
            var sf = (float) this._maxHeight / this._discardBox.Image.Height;
            this._discardBox.Size = new Size((int) (this._discardBox.Image.Width * sf), (int) (this._discardBox.Image.Height * sf));
            this._discardBox.Location = new Point(
                this._containerSize.Width - this._discardBox.Width,
                this._containerSize.Height - this._discardBox.Height);
        }

        private void DisplayHand()
        {
            // Display all of the cards in the hand
            var sf = (float) this._maxHeight / this._hand.First().Image.Height;
            var cardWidth = (int) (this._hand.First().Image.Width * sf);
            var maxWidth = this._containerSize.Width - this._sideBorder * 2;
            var xPadding = (maxWidth - cardWidth * this._hand.Count) / this._hand.Count;
            for (var i = 0; i < this._hand.Count; i++)
            {
                var locationX = (int) ((this._hand[i].Image.Width * sf) * i);
                this._hand[i].Size = new Size(
                    (int) (this._hand[i].Image.Width * sf),
                    (int) (this._hand[i].Image.Height * sf));
                this._hand[i].Location = new Point(
                    this._sideBorder + locationX + (xPadding * i),
                    (int) (this._containerSize.Height - (this._maxHeight / 80d) - this._hand[i].Height));
                this._hand[i].BackColor = Color.Transparent;
            }
        }

        public void SetHandHeight(int height)
        {
            // Set the maximum height of the cards in the hand
            this._maxHeight = height;
        }

        public void SetSideBorder(int size)
        {
            // Set the size of the border either side of the hand
            this._sideBorder = size;
        }

        public PictureBox GetDeckBox()
        {
            // Fetch the deck PictureBox
            return this._deckBox;
        }

        public PictureBox GetDiscardBox()
        {
            // Fetch the discard PictureBox
            return this._discardBox;
        }

        public void SetContainerSize(Size size)
        {
            // Set the size of the parent control
            this._containerSize = size;
        }
    }
}