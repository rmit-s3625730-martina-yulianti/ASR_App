using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    class Room
    {
        public const int ROOMSLOTS = 2;    // Maximum slots for each room
        public Schedule[] Schedules = new Schedule[ROOMSLOTS]; 
        public string RoomName { get; }
        public int RoomSlots { get; set; }  

        // Constractor
        public Room(string roomName)
        {
            RoomName = roomName;
            RoomSlots = ROOMSLOTS;

            for(int i=0; i < RoomSlots; i++)
            {
                Schedules[i] = new Schedule();
            }

        }

        // Delete schedule and decrease counter to RoomSlots when staff schedule created
        public void DeleteSchedule()
        {
            if (RoomSlots >= 0 && RoomSlots < 2)
            {
                RoomSlots++;
            }
            else
            {
                throw new SlotException("Unable to create slot.");
            }
        }

        // Add schedule and increase counter to RoomSlots when staff schedule created (pending)
        public void AddSchedule(string date, string startTime)
        {
            if (RoomSlots > 0 && RoomSlots <= ROOMSLOTS)
            {
                for(int i = 0; i < ROOMSLOTS; i++)
                {
                    // Check if the schedule already exist in the Schecules
                    if (Schedules[i].Date.Equals(date) && Schedules[i].StartTime.Equals(startTime))
                    {
                        throw new SlotException("Slot already exist. Choose other date or time");
                    }
                    else
                    {
                        continue;
                    }
                }
                // Cut the hour from startTime, and convert to int
                string hourStr = startTime.Substring(0, 2);
                // Add one hour from EndTime (1 hour consultation) 
                int EndInt = int.Parse(hourStr) + 1;

                // Add new schedule to the room
                Schedules[ROOMSLOTS-RoomSlots].Date = date;
                Schedules[ROOMSLOTS-RoomSlots].StartTime = startTime;
                string endTime = (EndInt < 10) ? "0" + EndInt.ToString() : EndInt.ToString();
                Schedules[ROOMSLOTS-RoomSlots].EndTime = endTime + ":00";
                RoomSlots--;

            }
            else 
            {
                throw new SlotException("Maximum 2 slots only.");
            }
        }

        // Check Room Availability
        public bool RoomAvailability() => (RoomSlots > 0 && RoomSlots <= 2) ? true : false;
        
    }
}
