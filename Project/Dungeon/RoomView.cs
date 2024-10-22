﻿using Project.Dungeon.Rooms;

namespace Project.Dungeon
{
    public class RoomView : View
    {
        private static readonly RoomView Instance = new RoomView();
        private Room _room;

        private RoomView()
        {
            // Create a RoomView. This will be the class which is displayed on the BaseForm,
            // and holds whichever room the player is currently located in
        }

        public static RoomView GetInstance()
        {
            // Get the RoomView
            return Instance;
        }

        public Room GetRoom()
        {
            // Fetch the currently held room
            return this._room;
        }

        public void SetRoom(Room room)
        {
            // Change the room currently displayed by the RoomView
            
            // If there is already a room on the view, it should be removed
            if (this.Controls.Count > 0) this.Controls.Clear();
            // Add and display the new room
            this._room = room;
            if (this._room is null) return;
            this.Controls.Add(this._room);
            this._room.Show();
            this.SetComponents();
        }

        protected override void SetComponents()
        {
            // If there is a room on the RoomView, scale it to the screen size
            if (this._room != null) {
                this._room.Size = this.Size;
                this._room.SetComponents();
            }
        }
    }
}