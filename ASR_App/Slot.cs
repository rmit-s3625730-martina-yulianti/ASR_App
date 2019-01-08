using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class Slot
    {
        public Room SlotRoom { get; set; }
        public string StaffID { get; set; }
        public string BookingID { get; set; }

        public Slot(string staffID, Room room)
        {
            StaffID = staffID;
            SlotRoom = room;
            BookingID = "-";
        }

    }
}
