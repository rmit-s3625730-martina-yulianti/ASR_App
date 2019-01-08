using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class Room
    {
        public Schedule[] Schedules = new Schedule[6];
        public string RoomName { get; set; }
        public int RoomSlots { get; set; }

        // Constrator
        public Room(string roomName)
        {
            RoomName = roomName;
            RoomSlots = 0;

            for (int i = 0; i < Schedules.Length; i++)
            {
                Schedules[i] = new Schedule();
            }
        }

        // Add counter to RoomSlots
        public void RoomsCounter()
        {
            if (RoomSlots < 2)
            {
                RoomSlots++;
            }
            else
            {
                throw new SlotException("Unable to create slot.");
            }
        }
    }
}
