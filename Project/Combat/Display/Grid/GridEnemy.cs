using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Project.Util;

namespace Project.Combat.Display.Grid
{
    public class GridEnemy : GridEntity
    {
        private int _health;
        private readonly int _maxHealth;
        private readonly int _damage;
        private readonly int _speed;
     
        protected GridEnemy(int health, int damage = 0, int speed = 1)
        {
            // Create a GridEnemy
            this._health = health;
            this._maxHealth = health;
            this._damage = damage;
            this._speed = speed;
        }
        
        public int GetHealth()
        {
            return this._health;
        }

        public int GetMaxHealth()
        {
            // Get the enemy's max health
            return this._maxHealth;
        }
        
        private void ChangeHealth(int amount)
        {
            // Edit the enemy's health and update health bars
            this._health += amount;
            CombatView.GetInstance().UpdateHealthBars();
        }

        public bool DamageEntity(int damage)
        {
            // Damage the enemy
            if (this._health < damage) damage = this._health;
            ChangeHealth(-damage);
            // Return true if the enemy is dead
            return this._health == 0;
        }

        public void HealEnemy(int health)
        {
            // Regenerate health up to maximum health
            if (this._health + health >= this._maxHealth) this._health = this._maxHealth;
            else this._health += health;
            CombatView.GetInstance().UpdateHealthBars();
        }

        private void BasicAttack()
        {
            // A basic attack that is always available to an entity.
            // Can be overridden to remove the attack, make it stronger,
            // or entirely change how it works
            var killed = PlayerStats.GetInstance().DamagePlayer(this._damage);
            if (killed) PlayerStats.GetInstance().PlayerDeath();
        }

        public void TakeTurn()
        {
            // The enemy's turn
            // Move the enemy, and see if the player is close
            // enough to attack
            var attack = FindAndMakeMove();
            if (!attack) return;
            // If the player is in range, attack them and update
            // the health bars
            BasicAttack();
            CombatView.GetInstance().UpdateHealthBars();
        }

        private bool FindAndMakeMove()
        {
            // A basic movement algorithm
            // Will move towards the player until touching
            // Returns true when touching player, false otherwise
            var playerCoordinates = GridPlayer.GetInstance().GetCoordinates();
            // If already adjacent to the player, return true
            if (CombatGrid.GetInstance().GetDistanceBetweenLocations(this.GetCoordinates(), playerCoordinates) == 1)
            {
                return true;
            }
            var possibleCoordinates = new Dictionary<Point, List<int>>();
            var preferredCoordinates = new Dictionary<Point, List<int>>();

            var rand = CombatManager.GetInstance().GetRandom();
            var current = new Dictionary<Point, int> {{this.GetCoordinates(), -1}};
            var toCheck = new Dictionary<Point, int>();
            var checkedLocations = new Dictionary<Point, List<int>>();
            var distanceFromEntity = 0;
            var distanceFromPlayer = 0;
            
            while (current.First().Key != new Point(-1, -1))
            {
                // If the square is occupied, you cannot move through it
                if ((!CombatGrid.GetInstance().IsGridSquareOccupied(current.First().Key) ||
                     current.First().Key == this.GetCoordinates()) &&
                    current.First().Key.X >= 0 && current.First().Key.Y >= 0)
                {
                    // Set the distance from the entity and player
                    distanceFromEntity = current.First().Value + 1;
                    distanceFromPlayer = CombatGrid.GetInstance().GetDistanceBetweenLocations(current.First().Key, playerCoordinates);
                    // If the square isn't close enough to reach it can be ignored
                    if (distanceFromEntity <= this._speed)
                    {
                        // If the square isn't the starting square then add it to possible locations
                        if (distanceFromEntity > 0)
                        {
                            possibleCoordinates.Add(
                                new Point(current.First().Key.X, current.First().Key.Y),
                                new List<int>() {distanceFromPlayer, distanceFromEntity});
                        }
                        
                        // Add all adjacent squares unless they have already been checked
                        if (current.First().Key.X != 0 &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X - 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X - 1, current.First().Key.Y), distanceFromEntity);
                        }
                        else if (current.First().Key.X != 0 &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X - 1, current.First().Key.Y)) &&
                                 distanceFromEntity < toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X - 1, current.First().Key.Y)] = distanceFromEntity;
                        }
                        
                        if (current.First().Key.X != CombatGrid.GetInstance().ColumnCount &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X + 1, current.First().Key.Y)))
                        {
                            toCheck.Add(new Point(current.First().Key.X + 1, current.First().Key.Y), distanceFromEntity);
                        }
                        else if (current.First().Key.X != CombatGrid.GetInstance().ColumnCount &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X + 1, current.First().Key.Y)) &&
                                 distanceFromEntity < toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)])
                        {
                            toCheck[new Point(current.First().Key.X + 1, current.First().Key.Y)] = distanceFromEntity;
                        }
                        
                        if (current.First().Key.Y != 0 &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y - 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y - 1), distanceFromEntity);
                        }
                        else if (current.First().Key.Y != 0 &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y - 1)) &&
                                 distanceFromEntity < toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y - 1)] = distanceFromEntity;
                        }
                        
                        if (current.First().Key.Y != CombatGrid.GetInstance().RowCount &&
                            !checkedLocations.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                            !toCheck.Keys.Contains(new Point(current.First().Key.X, current.First().Key.Y + 1)))
                        {
                            toCheck.Add(new Point(current.First().Key.X, current.First().Key.Y + 1), distanceFromEntity);
                        }
                        else if (current.First().Key.Y != CombatGrid.GetInstance().RowCount &&
                                 toCheck.ContainsKey(new Point(current.First().Key.X, current.First().Key.Y + 1)) &&
                                 distanceFromEntity < toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)])
                        {
                            toCheck[new Point(current.First().Key.X, current.First().Key.Y + 1)] = distanceFromEntity;
                        }
                    }
                }
                // The square has been checked and does not need checking again
                toCheck.Remove(current.First().Key);
                checkedLocations.Add(current.First().Key, new List<int>() {distanceFromPlayer, distanceFromEntity});
                if (toCheck.Count > 0)
                {
                    // If there are more squares to check, find the closest
                    var min = toCheck.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    current = new Dictionary<Point, int> {{min, toCheck[min]}};
                }
                else
                {
                    current = new Dictionary<Point, int> {{new Point(-1, -1), -1}};
                }
            }
            
            // Only include the closest locations to the player
            var closestToPlayer = 999;
            foreach (var location in possibleCoordinates)
            {
                if (location.Value[0] < closestToPlayer)
                {
                    closestToPlayer = location.Value[0];
                    preferredCoordinates.Clear();
                    preferredCoordinates.Add(location.Key, location.Value);
                }
                else if (location.Value[0] == closestToPlayer)
                {
                    preferredCoordinates.Add(location.Key, location.Value);
                }
            }

            if (possibleCoordinates.Count == 1)
            {
                // If there is a single point closest to the
                // player, move there
                MakeMove(possibleCoordinates.First().Key);
                return closestToPlayer == 1;
            }

            
            // Find the location that prioritises movement in
            // whichever direction is farther away
            var smallestDifference = 999;
            var idealCoordinates = new Dictionary<Point, List<int>>();
            foreach (var location in preferredCoordinates)
            {
                var yDifferenceFromLocation = Math.Abs(this.GetCoordinates().Y - playerCoordinates.Y);
                var xDifferenceFromLocation = Math.Abs(this.GetCoordinates().X - playerCoordinates.X);

                var difference = Math.Abs(xDifferenceFromLocation - yDifferenceFromLocation);
                
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    idealCoordinates.Clear();
                    idealCoordinates.Add(location.Key, location.Value);
                }
                else if (difference == smallestDifference)
                {
                    idealCoordinates.Add(location.Key, location.Value);
                }
            }

            if (idealCoordinates.Count == 1)
            {
                // If there is a single ideal location, move there
                MakeMove(idealCoordinates.First().Key);
                return closestToPlayer == 1;
            }
            
            // Find the ideal coordinate closest to the enemy
            var smallestDistanceToSelf = 999;
            var bestCoordinates = new Dictionary<Point, List<int>>();
            foreach (var location in idealCoordinates)
            {
                if (location.Value[1] < smallestDistanceToSelf)
                {
                    smallestDistanceToSelf = location.Value[1];
                    bestCoordinates.Clear();
                    bestCoordinates.Add(location.Key, location.Value);
                }
                else if (location.Value[1] == smallestDistanceToSelf)
                {
                    bestCoordinates.Add(location.Key, location.Value);
                }
            }
            
            if (bestCoordinates.Count == 1)
            {
                // If there is a single coordinate left, move there
                MakeMove(bestCoordinates.First().Key);
                return closestToPlayer == 1;
            }
            
            // If there are still multiple best locations, pick one at random
            MakeMove(bestCoordinates.ElementAt(rand.Next(0, bestCoordinates.Count)).Key);
            return closestToPlayer == 1;
        }

        private void MakeMove(Point newLocation)
        {
            // Does not update entity positions as that will be done after all enemies have moved
            this.SetCoordinates(newLocation);
        }
    }
}