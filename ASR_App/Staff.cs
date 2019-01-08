using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class Staff : User
    {
        public Staff() : base() { }
        public Staff(string id, string name, string email) : base(id, name, email) { }

    }
}
