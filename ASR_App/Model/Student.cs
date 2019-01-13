using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_Model
{
    public class Student : User
    {
        public bool BookingStatus { get; set; }

        public Student(string id, string name, string email) : base(id, name, email) { }

    }
}
