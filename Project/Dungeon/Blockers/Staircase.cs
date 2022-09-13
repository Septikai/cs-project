using System.Drawing;
using Project.Util;

namespace Project.Dungeon.Blockers
{
    public sealed class Staircase : Blocker
    {
        public Staircase()
        {
            // Create a staircase
            this.BackColor = Color.SaddleBrown;
        }

        public void Climb(Direction origin)
        {
            // Use the staircase to move between floors
            DungeonManager.GetInstance().MoveFloor();
            
            var room = DungeonManager.GetInstance().GetCurrentRoom();
            if (room is null) return;
            
            // Set up the new room
            var newRoomData = 
                DungeonManager.GetInstance().GetRoomData(
                    DungeonManager.GetInstance().GetCurrentFloor(),
                    DungeonManager.GetInstance().GetCurrentCoordinate());
            room.LoadRoomData(newRoomData);
            
            room.EnterRoom(Direction.Centre, origin);
        }

        public void SetImage(Direction staircaseDirection)
        {
            // TODO
        }
    }
}