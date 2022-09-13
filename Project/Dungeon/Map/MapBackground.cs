using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Project.Properties;
using Project.Util;

namespace Project.Dungeon.Map
{
    public sealed class MapBackground : Panel
    {
        private static readonly MapBackground Instance = new MapBackground();
        private int _availableArea;
        private int _xBorder;
        private int _yBorder;
        private readonly MapRoom _nullRoom;
        private readonly List<List<MapRoom>> _mapRooms = new List<List<MapRoom>>();
        private MapMarker _playerMarker;
        private readonly List<MapMarker> _otherMarkers = new List<MapMarker>();
        private readonly Label _floorLabel;

        private MapBackground()
        {
            // Create the MapBackground
            this.BackColor = Color.Khaki;
            // Create the label for the floor
            this._floorLabel = new Label();
            this._floorLabel.Text = DungeonManager.GetInstance().GetCurrentFloorNumber().ToString();
            this._floorLabel.Name = "FloorLabel";
            this._floorLabel.Font = new Font(this._floorLabel.Font.Name, this._floorLabel.Font.Size * 5);
            // Initialise the map with empty rooms
            this._nullRoom = new MapRoom(new RoomData(null));
            InitialiseMapRooms();
            // Initialise the MapBackground and player position marker
            InitialiseComponents();
        }

        public static MapBackground GetInstance()
        {
            // Fetch the MapBackground
            return Instance;
        }

        private void InitialiseMapRooms()
        {
            this.Controls.Add(this._floorLabel);
            // Fill the list of MapRooms with empty MapRooms
            for (var col = 0; col < 9; col++)
            {
                this._mapRooms.Add(new List<MapRoom>());
                for (var row = 0; row < 9; row++)
                {
                    this._mapRooms[col].Add(_nullRoom);
                }
            }
        }

        private void InitialiseComponents()
        {
            // Determine the area in which the map itself should be displayed
            this._availableArea =
                this.Height * 9 / 10 < this.Width * 9 / 10 ? this.Height * 9 / 10 : this.Width * 9 / 10;
            this._xBorder = (this.Width - this._availableArea) / 2;
            this._yBorder = (this.Height - this._availableArea) / 2;

            // Create the player position marker
            this._playerMarker = new MapMarker();
            this._playerMarker.BackColor = Color.Blue;
            this._playerMarker.Size = new Size(this._availableArea / 9 / 3, this._availableArea / 9 / 3);
            this._playerMarker.Location = new Point(
                this._availableArea / 9 / 2 - this._playerMarker.Width / 2,
                this._availableArea / 9 / 2 - this._playerMarker.Height / 2
            );
            
            // Resize and reposition the label
            var numberSize = TextRenderer.MeasureText(this._floorLabel.Text, this._floorLabel.Font);
            this._floorLabel.Size = new Size(numberSize.Width, numberSize.Height);
            this._floorLabel.Location = new Point(this.Width * 19 / 20 - this._floorLabel.Width / 2, this.Height * 1 / 20);

            UpdateMap();
        }
        
        public void UpdateMap()
        {
            var currentFloor = DungeonManager.GetInstance().GetCurrentFloor();
            if (currentFloor is null) return;
            for (var col = 0; col < 9; col++)
            {
                for (var row = 0; row < 9; row++)
                {
                    if (currentFloor[col][row].Explored &&
                        this._mapRooms[col][row].GetRoomData() == this._nullRoom.GetRoomData())
                    {
                        var room = new MapRoom(currentFloor[col][row]);

                        room.Size = new Size(this._availableArea / 9, this._availableArea / 9);
                        room.Location = new Point(
                            this._xBorder + this._availableArea * col / 9,
                            this._yBorder + this._availableArea * row / 9
                        );

                        if (currentFloor[col][row].StaircaseDirection != Direction.NullDirection)
                        {
                            var staircaseMarker = new MapMarker();
                            var img = Resources.stairs_marker;
                            if (currentFloor[col][row].StaircaseDirection == Direction.Up)
                                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            staircaseMarker.Image = img;
                            staircaseMarker.Size = new Size(this._availableArea / 9 / 2, this._availableArea / 9 / 2);
                            staircaseMarker.Location = new Point(
                                this._availableArea / 9 / 2 - staircaseMarker.Width / 2,
                                this._availableArea / 9 / 2 - staircaseMarker.Height / 2
                            );
                            this._otherMarkers.Add(staircaseMarker);
                            room.AddMarker(staircaseMarker);
                        }
                        
                        this._mapRooms[col][row] = room;
                        this.Controls.Add(room);
                    }
                }
            }

            var coordinate = DungeonManager.GetInstance().GetCurrentCoordinate();
            this._mapRooms[coordinate.GetX()][coordinate.GetY()].AddMarker(this._playerMarker);
            this._playerMarker.BringToFront();
            
            this._floorLabel.Text = DungeonManager.GetInstance().GetCurrentFloorNumber().ToString();
            
            // Resize and reposition the label
            var numberSize = TextRenderer.MeasureText(this._floorLabel.Text, this._floorLabel.Font);
            this._floorLabel.Size = new Size(numberSize.Width, numberSize.Height);
            this._floorLabel.Location = new Point(this.Width * 19 / 20 - this._floorLabel.Width / 2, this.Height * 1 / 20);
            this._floorLabel.BringToFront();
        }

        public void SetComponents()
        {
            // Update the area in which the map should be displayed
            this._availableArea =
                this.Height * 9 / 10 < this.Width * 9 / 10 ? this.Height * 9 / 10 : this.Width * 9 / 10;
            this._xBorder = (this.Width - this._availableArea) / 2;
            this._yBorder = (this.Height - this._availableArea) / 2;
            
            // Resize and reposition all rooms on the map
            for (var col = 0; col < 9; col++)
            {
                for (var row = 0; row < 9; row++)
                {
                    if (this._mapRooms[col][row].GetRoomData() != this._nullRoom.GetRoomData())
                    {
                        var room = this._mapRooms[col][row];
                        room.Size = new Size(this._availableArea / 9, this._availableArea / 9);
                        room.Location = new Point(
                            this._xBorder + this._availableArea * col / 9,
                            this._yBorder + this._availableArea * row / 9
                        );
                    }
                }
            }

            foreach (var marker in this._otherMarkers)
            {
                marker.Size = new Size(this._availableArea / 9 / 2, this._availableArea / 9 / 2);
                marker.Location = new Point(
                    this._availableArea / 9 / 2 - marker.Width / 2,
                    this._availableArea / 9 / 2 - marker.Height / 2
                );
            }
            
            // Resize and reposition the player position marker
            this._playerMarker.Size = new Size(this._availableArea / 9 / 3, this._availableArea / 9 / 3);
            this._playerMarker.Location = new Point(
                this._availableArea / 9 / 2 - this._playerMarker.Width / 2,
                this._availableArea / 9 / 2 - this._playerMarker.Height / 2
            );
            
            // Resize and reposition the label
            var numberSize = TextRenderer.MeasureText(this._floorLabel.Text, this._floorLabel.Font);
            this._floorLabel.Size = new Size(numberSize.Width, numberSize.Height);
            this._floorLabel.Location = new Point(this.Width * 19 / 20 - this._floorLabel.Width / 2, this.Height * 1 / 20);
        }

        public void ClearMap()
        {
            this.Controls.Clear();
            this._mapRooms.Clear();
            this.InitialiseMapRooms();
        }
    }
}