using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project.Combat.Display;
using Project.Combat.Display.Grid;
using Project.Util;

namespace Project.Combat
{
    public class CombatView : View
    {
        private static readonly CombatView Instance = new CombatView();
        private readonly CombatGrid _gridInstance = CombatGrid.GetInstance();
        private readonly MainHealthBar _playerHealthBar = new MainHealthBar("Player", PlayerStats.GetInstance().GetMaxHealth());
        private readonly MainHealthBar _enemyHealthBar = new MainHealthBar("Enemy", -1, 1);
        private readonly SideDisplay _leftSideDisplay = new SideDisplay("LeftSideDisplay", 0);
        private readonly SideDisplay _rightSideDisplay = new SideDisplay("RightSideDisplay", 1);

        private CombatView()
        {
            // Create the CombatDisplay instance
            this.ControlAdded += CombatDisplayControlAdded;
            this.ControlRemoved += CombatDisplayControlRemoved;
            this.MouseUp += CombatDisplayMouseUpDuringCombat;
            InitialiseComponents();
        }

        public static CombatView GetInstance()
        {
            // Get the CombatView
            return Instance;
        }

        private void CombatDisplayControlAdded(object sender, ControlEventArgs e)
        {
            // Add the required method to the MouseUp event of all added controls
            e.Control.MouseUp += CombatDisplayMouseUpDuringCombat;
        }
        
        private void CombatDisplayControlRemoved(object sender, ControlEventArgs e)
        {
            // Remove the required method to the MouseUp event of all added controls
            e.Control.MouseUp -= CombatDisplayMouseUpDuringCombat;
        }
        
        private void CombatDisplayMouseUpDuringCombat(object sender, EventArgs e)
        {
            // Control clicks during combat
            if (!CombatManager.GetInstance().IsInCombat()) return;
            var me = (MouseEventArgs) e;
            if (me.Button.Equals(MouseButtons.Right))
            {
                if (CombatManager.GetInstance().IsMoving())
                {
                    GridPlayer.GetInstance().CancelMove();
                }
                else if (CombatManager.GetInstance().IsAttacking())
                {
                    GridPlayer.GetInstance().CancelAttack();
                }
                else if (CombatManager.GetInstance().IsHealing())
                {
                    GridPlayer.GetInstance().CancelHeal();
                }
                else if (CardManager.GetSelectedCard() != null)
                {
                    CardManager.SetSelectedCard(null);
                }
            }
        }

        private void InitialiseComponents()
        {
            // Initialise and position all components of the CombatView
            SetDimensions();
            this._gridInstance.ControlAdded += CombatDisplayControlAdded;
            this._gridInstance.ControlRemoved += CombatDisplayControlRemoved;
            this.Controls.Add(this._gridInstance);
            this._gridInstance.Initialise();
            
            this.Controls.Add(this._playerHealthBar);
            this._playerHealthBar.UpdateBar();
            this._playerHealthBar.AddMouseUpEvent(CombatDisplayMouseUpDuringCombat);

            this.Controls.Add(this._enemyHealthBar);
            this._enemyHealthBar.UpdateBar();
            this._enemyHealthBar.AddMouseUpEvent(CombatDisplayMouseUpDuringCombat);
            
            DisplayCards();
            this.Controls.Add(Hand.GetInstance().GetDeckBox());
            this.Controls.Add(Hand.GetInstance().GetDiscardBox());
            
            this.Controls.Add(this._leftSideDisplay);
            this.Controls.Add(this._rightSideDisplay);
        }
        
        protected override void SetComponents()
        {
            // Position and update all components of the CombatView
            SetDimensions();
            this._gridInstance.SetComponents();
            this._playerHealthBar.UpdateBar();
            this._enemyHealthBar.UpdateBar();
            DisplayCards();
        }

        public void DisplayCards()
        {
            // Display the hand, deck and discard
            Hand.GetInstance().DisplayCards();
            foreach (var card in Hand.GetInstance().GetHand().Where(card => !this.Controls.Contains(card)))
            {
                this.Controls.Add(card);
            }
        }
        
        private void SetDimensions()
        {
            // Set the sizes and positions of all components of the view
            Hand.GetInstance().SetContainerSize(this.Size);
            this._leftSideDisplay.SetContainerSize(this.Size);
            this._rightSideDisplay.SetContainerSize(this.Size);
            
            var remainingHeight = this.Height;
            var squareSize = 0;
            
            var handHeight = this.Height / 4;
            Hand.GetInstance().SetHandHeight(handHeight);
            remainingHeight -= handHeight;
            
            var healthBarHeight = this.Height / 5;
            remainingHeight -= healthBarHeight;

            this._playerHealthBar.Size = new Size(this.Width / 5, healthBarHeight);
            this._playerHealthBar.Location = new Point((this.Width / 5) / 5, (healthBarHeight / 4) * 1);
            
            this._enemyHealthBar.Size = new Size(this.Width / 5, healthBarHeight);
            this._enemyHealthBar.Location = new Point(
                this.Width - this._enemyHealthBar.Width - (this.Width / 5) / 5, (healthBarHeight / 4) * 1);
            
            var gridHeight = remainingHeight;
            var minSideBorder = (this.Width / 20) * 3;
            Hand.GetInstance().SetSideBorder(minSideBorder);
            this._leftSideDisplay.SetSideBorder(minSideBorder);
            this._rightSideDisplay.SetSideBorder(minSideBorder);
            
            squareSize = gridHeight / this._gridInstance.RowCount < (this.Width - (minSideBorder * 2)) / this._gridInstance.ColumnCount ?
                gridHeight / this._gridInstance.RowCount :
                (this.Width - (minSideBorder * 2)) / this._gridInstance.ColumnCount;
            var sideBorder = (this.Width - (this._gridInstance.ColumnCount * squareSize)) / 2;
            
            this._gridInstance.Top = this.Height - handHeight - (this._gridInstance.RowCount * squareSize + this._gridInstance.RowCount + 2);
            this._gridInstance.Left = (this.Width - (squareSize * this._gridInstance.ColumnCount)) / 2;
            this._gridInstance.Size = new Size(
                this._gridInstance.ColumnCount * squareSize + this._gridInstance.ColumnCount + 1,
                this._gridInstance.RowCount * squareSize + this._gridInstance.RowCount + 1);
            this._gridInstance.SetSquareSize(squareSize);
        }

        public void UpdateHealthBars()
        {
            // Update the health values of the health bars
            this._playerHealthBar.SetHealth(PlayerStats.GetInstance().GetHealth());
            this._enemyHealthBar.SetHealth(CombatGrid.GetInstance().GetTotalEnemyHealth());
        }

        public void ShowActionOptions()
        {
            // Display the action options on the left and right displays
            CombatManager.GetInstance().SetActionOptionsVisible(true);
            this._leftSideDisplay.DisplayAction(CardManager.GetSelectedCard());
            this._rightSideDisplay.DisplayAction(CardManager.GetSelectedCard());
        }
        
        public void HideActionOptions()
        {
            // Hide the action options and display the hovered card on
            // the left display instead if necessary
            CombatManager.GetInstance().SetActionOptionsVisible(false);
            this._leftSideDisplay.HideAction(CardManager.GetSelectedCard());
            this._rightSideDisplay.HideAction(CardManager.GetSelectedCard());
            var hoveredCard = CardManager.GetInstance().GetHoveredCard();
            if (hoveredCard != null) this._leftSideDisplay.DisplayCard(hoveredCard);
        }

        public SideDisplay GetLeftSideDisplay()
        {
            // Fetch the left SideDisplay
            return this._leftSideDisplay;
        }
        
        public SideDisplay GetRightSideDisplay()
        {
            // Fetch the right SideDisplay
            return this._rightSideDisplay;
        }

        public void SetEnemyHealth(int health)
        {
            // Set the health and max health values for the enemy health bar
            this._enemyHealthBar.SetMaxHealth(health);
            this._enemyHealthBar.SetHealth(health);
        }
    }
}