using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project.Combat.Display;
using Project.Combat.Display.Grid;
using Project.Combat.Display.Grid.Enemy;
using Project.Dungeon;
using Project.Util;

namespace Project.Combat
{
    public class CombatManager
    {
        private static readonly CombatManager Instance = new CombatManager();
        private bool _isPlayerTurn = true;
        private bool _actionOptionsVisible;
        private bool _inCombat;
        private bool _isMoving;
        private bool _isAttacking;
        private bool _isHealing;
        private readonly Random _random = new Random();

        private CombatManager()
        {
            // Create the CombatDisplay instance
        }

        public static CombatManager GetInstance()
        {
            // Fetch the CombatDisplay instance
            return Instance;
        }

        public void CheckForEncounter()
        {
            // Check to see if there should be a combat
            if (!(this._random.Next(100) < 20)) return;
            StartCombat();
        }

        private List<GridEnemy> GenerateEnemies()
        {
            // Generate the enemies for the battle
            var enemies = new List<GridEnemy>();
            var enemyCount = GenerateEnemyCount();
            for (var i = 0; i < enemyCount; i++)
            {
                enemies.Add(new GridSkeleton());
            }
            return enemies;
        }

        private int GenerateEnemyCount()
        {
            // Decide how many enemies should be generated
            var randNum = this._random.Next(10);
            return randNum < 7 ? 1 : 2;
        }

        private void StartCombat()
        {
            // Start the combat
            this._inCombat = true;
            GameTracker.GetInstance().ClearHeldKeys();
            // Set up the grid
            CombatGrid.GetInstance().SetGridPlayer();
            CombatGrid.GetInstance().AddEnemies(GenerateEnemies());
            CombatView.GetInstance().SetEnemyHealth(CombatGrid.GetInstance().GetTotalEnemyMaxHealth());
            // Display the CombatView
            BaseForm.GetInstance().SwitchView(CombatView.GetInstance());
        }

        public void EndCombat()
        {
            // End the combat
            this._inCombat = false;
            GameTracker.GetInstance().ClearHeldKeys();
            // Switch back to displaying the RoomView
            BaseForm.GetInstance().SwitchView(RoomView.GetInstance());
            // Reset for next combat
            Hand.GetInstance().ResetCooldowns();
            ResetActionStates();
            CardManager.GetInstance().DrawNewHand();
            CombatGrid.GetInstance().ResetGrid();
            // Heal the player as a reward for winning the combat
            PlayerStats.GetInstance().HealPlayer(3);
            CombatView.GetInstance().UpdateHealthBars();
            CombatView.GetInstance().DisplayCards();
        }

        private void ResetActionStates()
        {
            // No actions in progress
            SetIsAttacking(false);
            SetIsHealing(false);
            SetIsMoving(false);
        }

        public void EndPlayerTurn()
        {
            // End the player's turn
            this._isPlayerTurn = false;
            // The enemy should then take their turns
            TakeEnemyTurn();
        }

        private async void TakeEnemyTurn()
        {
            // Get all enemies on the grid
            var enemies = new List<GridEnemy>(CombatGrid.GetInstance().GetGridEnemies());
            foreach (var enemy in enemies)
            {
                // Add a short delay between turns
                Application.DoEvents();
                await Task.Delay(1000 / enemies.Count);
                enemy.TakeTurn();
                CombatGrid.GetInstance().UpdateGridEntitiesPositions();
            }
            // End of round
            CardManager.GetInstance().RechargeCards();
            CardManager.GetInstance().UseCardsInQueue();
            // Player turn again
            this._isPlayerTurn = true;
        }

        public bool IsPlayerTurn()
        {
            // Check if it is the player's turn
            return this._isPlayerTurn;
        }

        public Random GetRandom()
        {
            // Fetch the random instance
            return this._random;
        }
        
        public bool IsInCombat()
        {
            // Check if the player is in combat
            return this._inCombat;
        }

        public void SetActionOptionsVisible(bool visible)
        {
            // Set if the action options are shown or not
            this._actionOptionsVisible = visible;
        }

        public bool IsActionOptionsVisible()
        {
            // Are the action options visible
            return this._actionOptionsVisible;
        }

        public bool IsMoving()
        {
            // Is a move action in progress
            return this._isMoving;
        }
        
        public void SetIsMoving(bool value)
        {
            // Set whether a move action is in progress
            this._isMoving = value;
        }

        public bool IsAttacking()
        {
            // Is an attack action in progress
            return this._isAttacking;
        }
        
        public void SetIsAttacking(bool value)
        {
            // Set whether an attack action is in progress
            this._isAttacking = value;
        }

        public bool IsHealing()
        {
            // Is a heal action in progress
            return this._isHealing;
        }
        
        public void SetIsHealing(bool value)
        {
            // Set whether a heal action is in progress
            this._isHealing = value;
        }
    }
}