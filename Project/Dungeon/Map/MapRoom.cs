using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Project.Properties;
using Project.Util;

namespace Project.Dungeon.Map
{
    public class MapRoom : PictureBox
    {
        private readonly RoomData _roomData;
        private readonly List<MapMarker> _markers = new List<MapMarker>();

        public MapRoom(RoomData roomData)
        {
            // Create a MapRoom from a RoomData object
            this._roomData = roomData;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            if (!_roomData.Generated) return;
            this.Image = GetRoomImage(roomData);
        }

        public RoomData GetRoomData()
        {
            // Fetch the RoomData of the MapRoom
            return this._roomData;
        }

        private Image GetRoomImage(RoomData roomData)
        {
            Image img;
            // Check how many doors there should be
            switch (roomData.DoorLocations.Count)
            {
                case 1:
                    img = Resources.room_one;
                    // Rotate to face the correct direction
                    switch (roomData.DoorLocations[0])
                    {
                        case Direction.North:
                            break;
                        case Direction.East:
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case Direction.South:
                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case Direction.West:
                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }

                    break;
                case 2:
                    // Check which room type should be used
                    if (roomData.NorthDoor && roomData.SouthDoor || roomData.EastDoor && roomData.WestDoor)
                    {
                        img = Resources.room_two_line;
                        // Rotate to face the correct direction
                        if (roomData.EastDoor) img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    else
                    {
                        img = Resources.room_two_corner;
                        // Rotate to face the correct direction
                        if (roomData.EastDoor && roomData.SouthDoor) img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else if (roomData.SouthDoor && roomData.WestDoor) img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        else if (roomData.WestDoor && roomData.NorthDoor) img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }

                    break;
                case 3:
                    img = Resources.room_three;
                    // Rotate to face the correct direction
                    if (!roomData.NorthDoor) img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    else if (!roomData.EastDoor) img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    else if (!roomData.SouthDoor) img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 4:
                    img = Resources.room_four;
                    break;
                default:
                    img = Resources.room_zero;
                    break;
            }

            return img;
        }

        public List<MapMarker> GetMapMarkers()
        {
            return this._markers;
        }

        public void AddMarker(MapMarker marker)
        {
            // Remove the marker from it's previous MapRoom
            marker.RemoveFromParent();
            // Add the marker to this MapRoom
            this._markers.Add(marker);
            this.Controls.Add(marker);
        }

        public void RemoveMarker(MapMarker marker)
        {
            // If the marker is already on this room, do nothing
            if (!this._markers.Contains(marker)) return;
            // Add the marker to this MapRoom
            this.Controls.Remove(marker);
            this._markers.Remove(marker);
        }
    }
}