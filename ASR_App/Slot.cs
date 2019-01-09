using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class Slot
    {
        public string RoomName { get; set; }
        public string SlotDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StaffID { get; set; }
        public string BookingID { get; set; }

        public Slot(string staffID,string room,string date, string start)
        {
            StaffID = staffID;
            RoomName = room;
            SlotDate = date;
            StartTime = start;
            BookingID = "-";

            
            // Cut the hour from startTime, and convert to int
            string hourStr = start.Substring(0, 2);
            // Add one hour from EndTime (1 hour consultation) 
            int EndInt = int.Parse(hourStr) + 1;

            // Set the End time
            string endTime = (EndInt < 10) ? "0" + EndInt.ToString() : EndInt.ToString();
            EndTime = endTime + ":00";

        }

        public override string ToString()
        {
            return ($"    {RoomName} \t\t {StartTime} \t\t {EndTime} \t\t {StaffID} \t\t {BookingID}");
        }



    }
}
