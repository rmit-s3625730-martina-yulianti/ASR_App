using System;

namespace ASR_Model
{
   
    class Room 
    {
        public const int ROOMSLOTS = 2;    // Maximum slots for each room
        public string RoomName { get; }
        public int RoomSlots { get; set; }  

        // Constractor
        public Room(string roomName)
        {
            RoomName = roomName;
     
        }

        public string RoomAvailability(int used) => $"\t{RoomName,-16} {RoomSlots - used,-5}";
        
        
    }
}
