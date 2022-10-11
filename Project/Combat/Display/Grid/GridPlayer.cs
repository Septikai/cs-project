using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Project.Util;

namespace Project.Combat.Display.Grid
{
    public sealed class GridPlayer : GridEntity
    {
        private static readonly GridPlayer Instance = new GridPlayer();
        
        private GridPlayer()
        {
            // Create the GridPlayer instance
            this.BackColor = Color.Blue;
            this.Name = "PLAYER";
        }

        public static GridPlayer GetInstance()
        {
            // Fetch the GridPlayer
            return Instance;
        }

        public void StartMove(int distance)
        {
            // If already moving, return
            if (CombatManager.GetInstance().IsMoving()) return;
            // If taking a different action, cancel the other action
            if (CombatManager.GetInstance().IsAttacking()) CancelAttack();
            if (CombatManager.GetInstance().IsHealing()) CancelHeal();
            // Start the movement action
            CombatManager.GetInstance().SetIsMoving(true);
            var possibleCoordinates = new List<Point>();
            var current = new Dictionary<Point, int> {{this.GetCoordinates(), -1}};
            var toCheck = new Dictionary<Point, int>();
            var checkedLocations = new Dictionary<Point, int>();
            var distanceFromPlayer = 0;
            
            while (current.First().Key != new Point(-1, -1))
            {
                // If the square is occupied, you cannot move through it
                if ((!CombatGrid.GetInstance().IsGridSquareOccupied(current.First().Key) ||
                     current.First().Key == this.GetCoordinates()) &&
                    current.First().Key.X >= 0 && current.First().Key.Y >= 0)
                {
                    // Set the distance from the player
                    distanceFromPlayer = current.First().Value + 1;
                    // If the square isn't close enough to reach it can be ignored
                    if (distanceFromPlayer <= distance)
                    {
                        // If the square isn't the starting square then add it to possible locations
                        if (distanceFromPlayer > 0)
                        {
                            possibleCoordinates.Add(new Point(current.First().Key.X, current.First().Key.Y));
                        }
                        
                        // Add all adjacent squares unless they have already been checked
                        if (current.First().Key.X != 0 &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X - 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (current.First().Key.X != 0 &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (current.First().Key.X != CombatGrid.GetInstance().ColumnCount &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X + 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (current.First().Key.X != CombatGrid.GetInstance().ColumnCount &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (current.First().Key.Y != 0 &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y - 1), distanceFromPlayer);
                        }
                        else if (current.First().Key.Y != 0 &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)] = distanceFromPlayer;
                        }
                        
                        if (current.First().Key.Y != CombatGrid.GetInstance().RowCount &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y + 1), distanceFromPlayer);
                        }
                        else if (current.First().Key.Y != CombatGrid.GetInstance().RowCount &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)] = distanceFromPlayer;
                        }
                    }
                }
                // The square has been checked and does not need checking again
                toCheck.Remove(current.First().Key);
                checkedLocations.Add(current.First().Key, distanceFromPlayer);
                if (toCheck.Count > 0)
                {
                    // If there are more locations to check, find the closest
                    var min = toCheck.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    current = new Dictionary<Point, int> {{min, toCheck[min]}};
                }
                else
                {
                    current = new Dictionary<Point, int> {{new Point(-1, -1), -1}};
                }
            }

            // Highlight all locations the player can move to
            CombatGrid.GetInstance().HighlightGridLocations(possibleCoordinates, Color.Khaki);
        }

        public void FinishMove(Point newLocation, Card usedCard)
        {
            // Move the player to the new location
            this.SetCoordinates(newLocation);
            CombatGrid.GetInstance().UpdateGridEntitiesPositions();
            // Remove the highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // The card has been used
            CardManager.SetSelectedCard(null);
            CardManager.GetInstance().QueueUsedCard(usedCard);
            // End the action and the turn
            CombatManager.GetInstance().SetIsMoving(false);
            CombatManager.GetInstance().EndPlayerTurn();
        }

        public void CancelMove()
        {
            // Remove all highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // End the action
            CombatManager.GetInstance().SetIsMoving(false);
        }

        public void StartAttack(int range)
        {
            // If already attacking, return
            if (CombatManager.GetInstance().IsAttacking()) return;
            // If taking a different action, cancel the other action
            if (CombatManager.GetInstance().IsMoving()) CancelMove();
            if (CombatManager.GetInstance().IsHealing()) CancelHeal();
            // Start the attack action
            CombatManager.GetInstance().SetIsAttacking(true);
            var possibleCoordinates = new List<Point>();
            var current = new Dictionary<Point, int> {{this.GetCoordinates(), -1}};
            var toCheck = new Dictionary<Point, int>();
            var checkedLocations = new Dictionary<Point, int>();
            var distanceFromPlayer = 0;
            
            while (current.First().Key != new Point(-1, -1))
            {
                if (current.First().Key.X >= 0 && current.First().Key.Y >= 0)
                {
                    // Set the distance from the player
                    distanceFromPlayer = current.First().Value + 1;
                    // If the square isn't close enough to reach it can be ignored
                    if (distanceFromPlayer <= range)
                    {
                        // If the square isn't the starting square then add it to possible locations
                        if (distanceFromPlayer > 0)
                        {
                            possibleCoordinates.Add(new Point(current.First().Key.X, current.First().Key.Y));
                        }
                        
                        // Add all adjacent squares unless they have already been checked
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X - 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X + 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y - 1), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y + 1), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)] = distanceFromPlayer;
                        }
                    }
                }
                // The square has been checked and does not need checking again
                toCheck.Remove(current.First().Key);
                checkedLocations.Add(current.First().Key, distanceFromPlayer);
                if (toCheck.Count > 0)
                {
                    // If there are more locations to check, find the closest
                    var min = toCheck.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    current = new Dictionary<Point, int> {{min, toCheck[min]}};
                }
                else
                {
                    current = new Dictionary<Point, int> {{new Point(-1, -1), -1}};
                }
            }

            // Highlight all locations the player can reach to attack
            CombatGrid.GetInstance().HighlightGridLocations(possibleCoordinates, Color.LightCoral);
        }

        public void FinishAttack(Point target, Card usedCard)
        {
            // Attack the enemy
            var allEnemiesDead = MakeAttack(target);
            // Remove the highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // The card has been used
            CardManager.SetSelectedCard(null);
            CardManager.GetInstance().QueueUsedCard(usedCard);
            // End the attack action
            CombatManager.GetInstance().SetIsAttacking(false);
            // End the turn, unless all enemies are dead, in which case end the combat
            if (!allEnemiesDead) CombatManager.GetInstance().EndPlayerTurn();
            else CombatManager.GetInstance().EndCombat();
        }

        public void CancelAttack()
        {
            // Remove all highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // End the action
            CombatManager.GetInstance().SetIsAttacking(false);
        }

        private bool MakeAttack(Point target)
        {
            // Get the enemy at the target location
            var enemy = (GridEnemy) CombatGrid.GetInstance().GetEntityAtGridSquare(target);
            // Deal damage to the enemy
            var killed = enemy.DamageEntity(CardManager.GetSelectedCard().GetDamage());
            if (!killed) return false;
            // If the enemy died, remove it from the grid
            CombatGrid.GetInstance().RemoveKilledEnemy(enemy);
            // return true if all enemies are dead
            return CombatGrid.GetInstance().CheckIfAllEnemiesAreDead();
        }

        public void StartHeal(int range)
        {
            // If already healing, return
            if (CombatManager.GetInstance().IsHealing()) return;
            // If taking a different action, cancel the other action
            if (CombatManager.GetInstance().IsMoving()) CancelMove();
            if (CombatManager.GetInstance().IsAttacking()) CancelAttack();
            // Start the heal action
            CombatManager.GetInstance().SetIsHealing(true);
            var possibleCoordinates = new List<Point>();
            var current = new Dictionary<Point, int> {{this.GetCoordinates(), -1}};
            var toCheck = new Dictionary<Point, int>();
            var checkedLocations = new Dictionary<Point, int>();
            var distanceFromPlayer = 0;
            
            while (current.First().Key != new Point(-1, -1))
            {
                if (current.First().Key.X >= 0 && current.First().Key.Y >= 0)
                {
                    // Set the distance from the player
                    distanceFromPlayer = current.First().Value + 1;
                    // If the square isn't close enough to reach it can be ignored
                    if (distanceFromPlayer <= range)
                    {
                        // If the square isn't the starting square then add it to possible locations
                        if (distanceFromPlayer >= 0)
                        {
                            possibleCoordinates.Add(new Point(current.First().Key.X, current.First().Key.Y));
                        }
                        
                        // Add all adjacent squares unless they have already been checked
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X - 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X + 1, current.First().Key.Y), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y - 1), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)] = distanceFromPlayer;
                        }
                        
                        if (!checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y + 1), distanceFromPlayer);
                        }
                        else if (toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                                 distanceFromPlayer < toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)] = distanceFromPlayer;
                        }
                    }
                }
                // The square has been checked and does not need checking again
                toCheck.Remove(current.First().Key);
                checkedLocations.Add(current.First().Key, distanceFromPlayer);
                if (toCheck.Count > 0)
                {
                    // If there are more locations to check, find the closest
                    var min = toCheck.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    current = new Dictionary<Point, int> {{min, toCheck[min]}};
                }
                else
                {
                    current = new Dictionary<Point, int> {{new Point(-1, -1), -1}};
                }
            }

            // Highlight all locations the player can reach to heal
            CombatGrid.GetInstance().HighlightGridLocations(possibleCoordinates, Color.LightGreen);
        }

        public void FinishHeal(Point target, Card usedCard)
        {
            // Heal the target
            Heal(target, -usedCard.GetDamage());
            CombatView.GetInstance().UpdateHealthBars();
            // Remove the highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // The card has been used
            CardManager.SetSelectedCard(null);
            CardManager.GetInstance().QueueUsedCard(usedCard);
            // End the action and the turn
            CombatManager.GetInstance().SetIsHealing(false);
            CombatManager.GetInstance().EndPlayerTurn();
        }

        public void CancelHeal()
        {
            // Remove the highlighting from the grid
            CombatGrid.GetInstance().RemoveAllGridHighlighting();
            // End the action
            CombatManager.GetInstance().SetIsHealing(false);
        }

        private void Heal(Point target, int health)
        {
            // Find the target entity
            var entity = CombatGrid.GetInstance().GetEntityAtGridSquare(target);
            // Heal the target entity
            if (entity.Name == "PLAYER") PlayerStats.GetInstance().HealPlayer(health);
            else ((GridEnemy) entity).HealEnemy(health);
        }
    }
}