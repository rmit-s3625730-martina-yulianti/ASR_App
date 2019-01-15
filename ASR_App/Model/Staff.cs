using System;

namespace ASR_Model
{
    /* Staff class is a model class to create container for staff object from User table
     */

    public class Staff : User
    {      
        public Staff(string id, string name, string email) : base(id, name, email) { }

    }

}
