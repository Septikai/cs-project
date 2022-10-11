using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Project.Combat.Display;
using Project.Combat.Display.Grid;
using Project.Properties;

namespace Project.Combat
{
    public class CardManager
    {
        private static readonly CardManager Instance = new CardManager();
        private static Dictionary<string, List<object>> _cardDictionary;
        private List<string> _deck = new List<string>();
        private readonly List<string> _discard = new List<string>();
        private static Card _selectedCard;
        private Card _toUse;
        private string _hoveredCard;

        private CardManager()
        {
            // Create the CardManager
            InitializeCardDictionary();
            InitialiseDeck();
            this._deck.Shuffle();
            DrawStartingHand();
        }

        public static CardManager GetInstance()
        {
            // Fetch the CardManager instance
            return Instance;
        }

        private static void InitializeCardDictionary()
        {
            _cardDictionary = new Dictionary<string, List<object>>();
            /*
             --- Insert all cards to a dictionary of `string id : List<object>` ---
             The list contains all arguments to create a new Card object:
             - string cardId,
             - Image image,
             - int damage,
             - int range,
             - int rechargeTime
             */
            _cardDictionary["default"] = new List<object>() {"default", Resources.hand_to_hand_card, 2, 2, 0};
            _cardDictionary["clandestine_strike"] = new List<object>() {"clandestine_strike", Resources.clandestine_strike_card, 3, 6, 3};
            _cardDictionary["fireball"] = new List<object>() {"fireball", Resources.fireball_card, 7, 0, 5};
            _cardDictionary["fleeting_gift"] = new List<object>() {"fleeting_gift", Resources.fleeting_gift_card, -3, 4, 2};
            _cardDictionary["healing_potion"] = new List<object>() {"healing_potion", Resources.healing_potion_card, -6, 2, 6};
            _cardDictionary["marksman"] = new List<object>() {"marksman", Resources.marksman_card, 4, 2, 2};
            _cardDictionary["twin_shields"] = new List<object>() {"twin_shields", Resources.twin_shields_card, -8, 0, 4};
            _cardDictionary["syntax_error"] = new List<object>() {"syntax_error", Resources.syntax_error_card, 0, 3, 6};
            _cardDictionary["spleong_rrero"] = new List<object>() {"spleong_rrero", Resources.spleong_rrero_card, 0, 3, 6};
            _cardDictionary["void_orb"] = new List<object>() {"void_orb", Resources.void_orb_card, 9, 1, 5};
        }

        public static Dictionary<string, List<object>> GetCardDictionary()
        {
            // Fetch the card dictionary
            return _cardDictionary;
        }

        private void InitialiseDeck()
        {
            // Create the deck
            this._deck.Clear();
            // 20 cards
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("marksman");
            this._deck.Add("spleong_rrero");
            this._deck.Add("spleong_rrero");
            this._deck.Add("spleong_rrero");
            this._deck.Add("spleong_rrero");
            this._deck.Add("spleong_rrero");
            this._deck.Add("syntax_error");
            this._deck.Add("clandestine_strike");
            this._deck.Add("void_orb");
            this._deck.Add("void_orb");
            this._deck.Add("twin_shields");
            this._deck.Add("healing_potion");
            this._deck.Add("fleeting_gift");
        }

        private void DrawStartingHand()
        {
            // Initialise the starting hand
            InitialiseDeck();
            Hand.GetInstance().ClearHand();
            var card = CardManager.GetCard("default");
            card.MouseClick += CardClickedByMouse;
            card.MouseLeave += CardLeftByMouse;
            card.MouseClick += CardClickedByMouse;
            Hand.GetInstance().AddToHand(card);
            DrawHand();
        }
        
        public void DrawNewHand()
        {
            // Discard the current hand
            var toDiscard = new List<Card>();
            foreach (var card in Hand.GetInstance().GetHand().Where(card => card.GetCardId() != "default"))
            {
                toDiscard.Add(card);
            }
            toDiscard.ForEach(DiscardCard);
            // Draw a new hand
            DrawHand();
        }
        
        private void DrawHand()
        {
            // Draw cards up to the maximum hand size
            for (var i = 0; i < Hand.GetHandSize(); i++)
            {
                DrawCard();
            }
        }
        
        private void DrawCard()
        {
            // Draw and initialise a card
            if (this._deck.Count <= 0) ReShuffleDeck();
            var drawnCard = GetCard(this._deck[0]);
            this._deck.RemoveAt(0);
            drawnCard.MouseEnter += CardEnteredByMouse;
            drawnCard.MouseLeave += CardLeftByMouse;
            drawnCard.MouseClick += CardClickedByMouse;
            Hand.GetInstance().AddToHand(drawnCard);
        }
        
        private void DiscardCard(Card card)
        {
            // Remove a card from the hand
            card.MouseEnter -= CardEnteredByMouse;
            card.MouseLeave += CardLeftByMouse;
            card.MouseClick -= CardClickedByMouse;
            card.Parent = null;
            Hand.GetInstance().RemoveFromHand(card);
            this._discard.Add(card.GetCardId());
        }
        
        private void ReShuffleDeck()
        {
            // Shuffle the discard pile into the deck
            this._deck = new List<string>(this._discard);
            this._discard.Clear();
            this._deck.Shuffle();
        }
        
        private static List<object> GetCardDetails(string id)
        {
            // Fetch the CardDictionary entry for a card
            return _cardDictionary[id];
        }
        
        private static Card GetCard(IReadOnlyList<object> cardDetails)
        {
            // Create a card from the details provided
            return new Card((string) cardDetails[0], (Image) cardDetails[1], (int) cardDetails[2],
                (int) cardDetails[3], (int) cardDetails[4]);
        }

        public static Card GetCard(string id)
        {
            // Create a card from the id provided
            return GetCard(GetCardDetails(id));
        }
        
        private void CardEnteredByMouse(object sender, EventArgs e)
        {
            // Set this card as the hovered card
            SetHoveredCard(sender.GetType() == typeof(Card) ? ((Card) sender).GetCardId() : "discard");
        }
        
        private void CardLeftByMouse(object sender, EventArgs e)
        {
            // Remove the hovered card
            SetHoveredCard(null);
        }
        
        private void CardClickedByMouse(object sender, EventArgs e)
        {
            // Select this card
            if (!CombatManager.GetInstance().IsPlayerTurn()) return;
            var card = (Card) sender;
            SetSelectedCard(card);
        }
        
        public static void SetSelectedCard(Card card)
        {
            // If the card is still recharging, it cannot be selected
            if (card != null && card.GetRemainingRechargeTime() > 0) return;
            _selectedCard = card;
            if (card is null)
            {
                // If deselecting a card, hide the action options
                if (CombatManager.GetInstance().IsActionOptionsVisible()) CombatView.GetInstance().HideActionOptions();
                return;
            }
            // If selecting a card, show the action options
            if (!CombatManager.GetInstance().IsActionOptionsVisible()) CombatView.GetInstance().ShowActionOptions();
            else if (CombatManager.GetInstance().IsMoving())
            {
                // If moving, restart the move action with the new card
                GridPlayer.GetInstance().CancelMove();
                GridPlayer.GetInstance().StartMove(_selectedCard.GetRange());
            }
            else
            {
                CombatView.GetInstance().GetRightSideDisplay().DisplayAction(GetSelectedCard());
                
                // If no action is in progress, do nothing
                if (!CombatManager.GetInstance().IsAttacking() && !CombatManager.GetInstance().IsHealing()) return;
                
                // Cancel the current action
                if (CombatManager.GetInstance().IsAttacking()) GridPlayer.GetInstance().CancelAttack();
                else GridPlayer.GetInstance().CancelHeal();
                
                // Start the appropriate action with the new card
                if (_selectedCard.GetDamage() >= 0) GridPlayer.GetInstance().StartAttack(_selectedCard.GetRange());
                else GridPlayer.GetInstance().StartHeal(_selectedCard.GetRange());
            }
        }

        public static Card GetSelectedCard()
        {
            // Get the selected card
            return _selectedCard;
        }

        public void QueueUsedCard(Card card)
        {
            // Set the next card to be used
            this._toUse = card;
        }
        
        public void UseCardsInQueue()
        {
            // Use the appropriate card
            this._toUse.UseCard();
            this._toUse = null;
        }

        public void RechargeCards()
        {
            // Recharge each card in the hand
            foreach (var card in Hand.GetInstance().GetHand())
            {
                card.Recharge();
            }
        }

        private void SetHoveredCard(string id)
        {
            // Set the hovered card
            this._hoveredCard = id;
            // If the action options are hidden, show the hovered card on
            // the left SideDisplay
            if (CombatManager.GetInstance().IsActionOptionsVisible()) return;
            CombatView.GetInstance().GetLeftSideDisplay().DisplayCard(id);
        }

        public List<string> GetDeck()
        {
            // Fetch the deck
            return this._deck;
        }

        public List<string> GetDiscard()
        {
            // Fetch the discard
            return this._discard;
        }

        public string GetHoveredCard()
        {
            // Fetch the id for the hovered card
            return this._hoveredCard;
        }
    }
    
    public static class DeckExtensions
    {
        public static void Shuffle(this IList<string> list)
        {
            // Shuffle the deck by randomly swapping the positions of cards in the deck
            var rand = new Random();
            for(var i = list.Count - 1; i > 1; i--)
            {
                var randomIndex = rand.Next(i + 1);  

                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }
        }
    }
}