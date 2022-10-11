using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project.Combat.Display.Grid;

namespace Project.Combat.Display
{
    public class CombatGrid : TableLayoutPanel
    {
        private static readonly CombatGrid Instance = new CombatGrid();
        private readonly int _rowCount = 7;
        private readonly int _colCount = 15;
        private int _squareSize;
        private readonly Dictionary<Point, Color> _gridHighlightedLocations = new Dictionary<Point, Color>();
        
        private CombatGrid()
        {
            // Create the CombatGrid
            this.RowCount = _rowCount;
            this.ColumnCount = _colCount;
        }

        public static CombatGrid GetInstance()
        {
            // Fetch the CombatGrid instance
            return Instance;
        }

        public void Initialise()
        {
            // Initialise the grid
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.CellPaint += PaintGridCell;
            this.MouseClick += CombatGridMouseClickDuringCombat;
            
            // Configure the rows and columns
            for (var row = 0; row < this._rowCount; row++)
            {
                this.RowStyles.Add(new RowStyle(SizeType.Absolute, this._squareSize));
            }

            for (var col = 0; col < this._colCount; col++)
            {
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, this._squareSize));
            }
            
            this.Margin = new Padding(0);
            this.Padding = new Padding(1);
        }

        public void SetComponents()
        {
            // Configure the rows and columns
            foreach (RowStyle rowStyle in this.RowStyles)
            {
                rowStyle.Height = this._squareSize;
            }
            foreach (ColumnStyle colStyle in this.ColumnStyles)
            {
                colStyle.Width = this._squareSize;
            }
            this.Margin = new Padding(0);
            this.Padding = new Padding(1);
        }

        public void SetSquareSize(int size)
        {
            // Set the size of each square
            this._squareSize = size;
        }
        
        public bool IsGridSquareOccupied(Point location)
        {
            // Determine if a square is occupied
            return (this.GetControlFromPosition(location.X, location.Y) != null);
        }
        
        public GridEntity GetEntityAtGridSquare(Point square)
        {
            // Get the entity from a specific square
            return (GridEntity) this.GetControlFromPosition(square.X, square.Y);
        }

        public void SetGridPlayer()
        {
            // Initialise the GridPlayer
            GridPlayer.GetInstance().SetCoordinates(new Point(2, 3));
            if (this.Controls.Contains(GridPlayer.GetInstance())) return;
            GridPlayer.GetInstance().InitialiseEntity();
            GridPlayer.GetInstance().Size = new Size(this._squareSize / 3 * 2, this._squareSize / 3 * 2);
            GridPlayer.GetInstance().MouseClick += GridPlayerClickDuringCombat;
            this.Controls.Add(GridPlayer.GetInstance(), 2, 3);
        }

        public void AddEnemies(List<GridEnemy> enemies)
        {
            // Initialise and position the enemies
            foreach (var enemy in enemies)
            {
                enemy.InitialiseEntity();
                enemy.Size = new Size(this._squareSize / 3 * 2, this._squareSize / 3 * 2);
                Point location;
                do
                {
                    location = new Point(CombatManager.GetInstance().GetRandom().Next(1, 5) + 8,
                        CombatManager.GetInstance().GetRandom().Next(1, 5));
                } while (IsGridSquareOccupied(location));
                enemy.SetCoordinates(location);
                enemy.MouseClick += GridEnemyClickDuringCombat;
                this.Controls.Add(enemy, location.X, location.Y);
            }
        }

        public List<GridEnemy> GetGridEnemies()
        {
            // Fetch all enemies on the grid
            var gridEnemies = new List<GridEnemy>();
            foreach (var control in this.Controls)
            {
                if (control is GridEnemy enemy) gridEnemies.Add(enemy);
            }
            return gridEnemies;
        }

        public int GetTotalEnemyMaxHealth()
        {
            // Find the total max health of all enemies
            return GetGridEnemies().Sum(enemy => enemy.GetMaxHealth());
        }

        public int GetTotalEnemyHealth()
        {
            // Find the total health of all enemies
            return GetGridEnemies().Sum(enemy => enemy.GetHealth());
        }
        
        public int GetDistanceBetweenLocations(Point location1, Point location2)
        {
            // Determine the distance between 2 squares
            var xDifference = Math.Abs(location1.X - location2.X);
            var yDifference = Math.Abs(location1.Y - location2.Y);
            return xDifference + yDifference;
        }
        
        public void HighlightGridLocations(List<Point> toHighlight, Color colour)
        {
            // Highlight a list of squares on the grid
            foreach (var location in toHighlight)
            {
                this._gridHighlightedLocations.Add(new Point(location.X, location.Y), colour);
            }
            this.Refresh();
        }
        
        public void RemoveAllGridHighlighting()
        {
            // Remove all highlighting from the grid
            this._gridHighlightedLocations.Clear();
            this.Refresh();
        }
        
        public void UpdateGridEntitiesPositions()
        {
            // Reposition GridEntities according to their stored coordinates
            foreach (GridEntity gridEntity in this.Controls)
            {
                var location = this.GetPositionFromControl(gridEntity);
                var gridEntityLocation = gridEntity.GetCoordinates();
                if (!(location.Column == gridEntityLocation.X &&
                      location.Row == gridEntityLocation.Y))
                {
                    this.SetCellPosition(gridEntity, new TableLayoutPanelCellPosition(gridEntityLocation.X, gridEntityLocation.Y));
                }
            }
        }

        public void RemoveKilledEnemy(GridEnemy enemy)
        {
            // Remove a GridEnemy from the grid
            this.Controls.Remove(enemy);
        }
        
        public bool CheckIfAllEnemiesAreDead()
        {
            // Check if there are any enemies alive
            return GetGridEnemies().Count == 0;
        }

        public void ResetGrid()
        {
            // Remove all entities from the grid
            this.Controls.Clear();
            // Position the GridPlayer
            this.Controls.Add(GridPlayer.GetInstance(), 2, 3);
        }
        
        private void PaintGridCell(object sender, TableLayoutCellPaintEventArgs e)
        {
            // Highlight grid cells as needed
            var current = new Point(e.Column, e.Row);
            using (var b = new SolidBrush(this._gridHighlightedLocations.Keys.Contains(current) ? this._gridHighlightedLocations[current] : SystemColors.Control))
            {
                e.Graphics.FillRectangle(b , e.CellBounds);
            }
        }
        
        private void GridEnemyClickDuringCombat(object sender, EventArgs e)
        {
            // Get the coordinates of the enemy
            var me = (MouseEventArgs) e;
            var cellPosition = this.GetPositionFromControl((GridEntity) sender);
            var location = new Point(cellPosition.Column, cellPosition.Row);
            if (location.X == -1 || location.Y == -1) return;
            if (me.Button.Equals(MouseButtons.Left))
            {
                // If attacking and this enemy is a valid target, finish the attack
                if (CombatManager.GetInstance().IsAttacking() &&
                    this._gridHighlightedLocations.Keys.Contains(location))
                {
                    GridPlayer.GetInstance().FinishAttack(location, CardManager.GetSelectedCard());
                }
                // If healing and this enemy is a valid target, finish the action
                else if (CombatManager.GetInstance().IsHealing() &&
                         this._gridHighlightedLocations.Keys.Contains(location))
                {
                    GridPlayer.GetInstance().FinishHeal(location, CardManager.GetSelectedCard());
                }
            }
        }
        
        private void GridPlayerClickDuringCombat(object sender, EventArgs e)
        {
            // Get the coordinates of the GridPlayer
            var me = (MouseEventArgs) e;
            var location = GridPlayer.GetInstance().GetCoordinates();
            if (location.X == -1 || location.Y == -1) return;
            if (me.Button.Equals(MouseButtons.Left))
            {
                // If healing and the player is a valid target, finish the action
                if (CombatManager.GetInstance().IsHealing() &&
                    this._gridHighlightedLocations.Keys.Contains(location))
                {
                    GridPlayer.GetInstance().FinishHeal(location, CardManager.GetSelectedCard());
                }
            }
        }
        
        private void CombatGridMouseClickDuringCombat(object sender, EventArgs e)
        {
            // Determine which cell was clicked
            var me = (MouseEventArgs) e;
            var location = GetRowColIndex(this.PointToClient(Cursor.Position));
            if (location.X == -1 || location.Y == -1) return;
            if (me.Button.Equals(MouseButtons.Left))
            {
                // If in a move action and the clicked cell is a viable target, finish the move
                if (CombatManager.GetInstance().IsMoving() && this._gridHighlightedLocations.Keys.Contains(location))
                {
                    GridPlayer.GetInstance().FinishMove(location, CardManager.GetSelectedCard());
                }
                // If attacking and the clicked cell is a viable target, finish the attack
                else if (CombatManager.GetInstance().IsAttacking() &&
                         this._gridHighlightedLocations.Keys.Contains(location))
                {
                    var enemy = GetEntityAtGridSquare(location);
                    if (enemy == null) return;
                    GridPlayer.GetInstance().FinishAttack(location, CardManager.GetSelectedCard());
                }
                // If in a heal action and the clicked cell is a viable target, finish the action
                else if (CombatManager.GetInstance().IsHealing() &&
                         this._gridHighlightedLocations.Keys.Contains(location))
                {
                    var entity = GetEntityAtGridSquare(location);
                    if (entity == null) return;
                    GridPlayer.GetInstance().FinishHeal(location, CardManager.GetSelectedCard());
                }
            }
        }
        
        private Point GetRowColIndex(Point point)
        {
            // Find a grid cell from a position
            if (point.X > this.Width || point.Y > this.Height) return new Point(-1, -1);

            var w = this.Width;
            var h = this.Height;
            var widths = this.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            var col = i + 1;

            var heights = this.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];

            var row = i + 1;

            return new Point(col, row);
        }
    }
}