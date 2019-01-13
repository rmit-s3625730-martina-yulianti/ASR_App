using System;
using System.Collections.Generic;
using Controller;
using ASR_App.Model;

namespace ASR_Model
{
    
    class Slot : AbstractSlot
    {
        //public string RoomNm { get; set; }
        public DateTime SlotDate { get; set; }
        public string StartTime { get; set; }
        //public string StaffID { get; set; }
       
        private List<Room> RoomsSlot = new List<Room>();
        private List<Staff> Staffs = new List<Staff>();

        public Slot(String staffId,String roomNm,DateTime date, string start)
        {
            // Initial Rooms 
            RoomsSlot = ASRController.CreateRooms();

            // Initial Users
            Staffs = ASRController.CreateStaffs();

            //StaffID = staffId;
            //RoomNm = roomNm;
            SlotDate = date;
            StartTime = start;
           
            /*
            // Cut the hour from startTime, and convert to int
            string hourStr = start.Substring(0, 2);
            // Add one hour from EndTime (1 hour consultation) 
            int EndInt = int.Parse(hourStr) + 1;

            // Set the End time
            string endTime = (EndInt < 10) ? "0" + EndInt.ToString() : EndInt.ToString();
            EndTime = endTime + ":00";
            */

            AddSchedule(roomNm, staffId, start);

        }

        // Return Rooms in this slot
        public List<Room> GetRooms()
        {
            return RoomsSlot;
        }

        // Return Staffs in this slot
        public List<Staff> GetStaffs()
        {
            return Staffs;
        }
        
        // Notify Observer for adding new schedule
        public void AddSchedule(string roomNm, string staffId, string startTime )
        {
            // register observers
            foreach (Room room in RoomsSlot)
            {
                if (room.RoomName == roomNm)
                {
                    AddRoom(room);
                    break;
                }
                else { continue; }
            }

            foreach (Staff staff in Staffs)
            {
                if (staff.UserID == staffId)
                {
                    AddStaff(staff);
                    break;
                }
                else { continue; }
            }

            NotifyNewSchedule(roomNm,staffId,startTime);
        }

        // Notify Observer for removing a schedule
        public void RemoveSchedule()
        {
            NotifyRemoveSchedule();
        }

                        
    }
}
