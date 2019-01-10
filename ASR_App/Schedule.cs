using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    class Schedule
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Available { get; set; }
        
        public Schedule()
        {
            Date = "-";
            StartTime = "-";
            EndTime = "-";
            Available = true;
        }

       

    }
}
