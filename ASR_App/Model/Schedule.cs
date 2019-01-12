using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    class Schedule
    {
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Available { get; set; }
        
        public Schedule()
        {
            Date = Convert.ToDateTime("01-01-2000");
            StartTime = "-";
            EndTime = "-";
            Available = true;
        }

       

    }
}
