using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class Room
    {
        public const int ROOMSLOTS = 2;    // Maximum slots for each room
        public Schedule[] Schedules = new Schedule[ROOMSLOTS]; 
        public string RoomName { get; }
        public int RoomSlots { get; set; }  

        // Constrator
        public Room(string roomName)
        {
            RoomName = roomName;
            RoomSlots = ROOMSLOTS;

            for (int i = 0; i < Schedules.Length; i++)
            {
                Schedules[i] = new Schedule();
            }
        }

        // Add counter to RoomSlots
        public void RoomsCounter()
        {
            if (RoomSlots > 0 && RoomSlots <=2)
            {
                RoomSlots--;
            }
            else
            {
                throw new SlotException("Unable to create slot.");
            }
        }
    }
}
