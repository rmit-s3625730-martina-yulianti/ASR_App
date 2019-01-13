using ASR_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App.Model
{
    abstract class AbstractSlot
    {
        IList<Room> RoomsList = new List<Room>();
        IList<Staff> StaffList = new List<Staff>();

        // Add room observer
        public void AddRoom(Room room)
        {
            RoomsList.Add(room);
        }

        // Add staff observer
        public void AddStaff(Staff staff)
        {
            StaffList.Add(staff);
        }

        // Remove room observer
        public void RemoveRoom(Room room)
        {
            RoomsList.Remove(room);
        }

        // Remove staff observer
        public void RemoveStaff(Staff staff)
        {
            StaffList.Remove(staff);
        }

        // Notify observers there is a new schedule added
        public void NotifyNewSchedule(string roomNm, string staffId, string startTime)
        {
            foreach(Room room in RoomsList)
            {
                if(room.RoomName == roomNm)
                    room.AddSchedule(staffId,startTime);
            }

            foreach(Staff staff in StaffList)
            {
                if(staff.UserID == staffId)
                    staff.AddSchedule(staffId, startTime);
            }
        }

        // Notify observers there is a schedule deleted
        public void NotifyRemoveSchedule()
        {
            foreach(Room room in RoomsList)
            {
                room.DeleteSchedule();
            }

            foreach (Staff staff in StaffList)
            {
                staff.DeleteSchedule();
            }
        }

    }
}
