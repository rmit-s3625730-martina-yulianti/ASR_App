using ASR_App;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    

    public class Staff : User, IObserver
    {
        const int MAXSLOTS = 4;
        public int SlotCounter { get; set; } = 0;
        public Staff(string id, string name, string email) : base(id, name, email) { }

        // Add counter everytime staff creates new slot
        public void AddSchedule(string staffId, string start)
        {
            //bool success = false;
            if (SlotCounter <= MAXSLOTS)
            {
                SlotCounter++;
                //success = true;
            }
            else
            {
                throw new SlotException("Unable to create slot. Maximum 4 slots");
            }

            //return success;
        }

        // Reduce counter everytime staff deletes new slot
        public void DeleteSchedule()
        {
            if (SlotCounter > 0 && SlotCounter <= MAXSLOTS)
            {
                SlotCounter--;
            }
            else
            {
                throw new SlotException("Maximum 4 slots");
            }

        }

    }

    
}
