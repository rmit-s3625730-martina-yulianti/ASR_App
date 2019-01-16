using System;

namespace ASR_Model
{
    /* Room class is a model class to create container for room object from Room table
     */

    public class Room 
    {
        private const int ROOMSLOTS = 2;    // Maximum slots for each room
        public string RoomName { get; }
        
        // Constractor
        public Room(string roomName)
        {
            RoomName = roomName;
        }

        public string RoomAvailability(int used) => $"\t{RoomName,-16} {ROOMSLOTS - used,-5}";      
        
    }
}
