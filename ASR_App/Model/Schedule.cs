using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    class Schedule
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BookingID { get; set; }
        public string StaffID { get; set; }

        public Schedule(string staffId, string start, string id="-")
        {
            StartTime = start;
            StaffID = staffId;
            BookingID = id;

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
            return $"{StartTime} \t\t {EndTime} \t\t {StaffID} \t\t {BookingID}";
        }


    }
}
