using System;

namespace ASR_Model
{
    /* Slot class is a model class to create container for slot object from Slot table
     */

    public class Slot
    {
        public string RoomName { get; set; }
        public DateTime SlotDatetime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StaffID { get; set; }
        public string StudentBookingID { get; set; }

        public Slot(string room,DateTime datetime,string staffId, string bookingId)
        {
            RoomName = room;
            StartTime = datetime.ToShortTimeString();
            EndTime = DateTime.Parse(datetime.Hour + 1 + ":00").ToShortTimeString();
            SlotDatetime = datetime.Date;
            StaffID = staffId;
            StudentBookingID = bookingId;              
        }

        public override string ToString()
        {
            return $"    {RoomName,-12} {StartTime,-15} {EndTime,-15} {StaffID,-15} {StudentBookingID}";
        }

    }
}
