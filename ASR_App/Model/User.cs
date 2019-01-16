using System;
using System.Collections.Generic;

namespace ASR_Model
{   /* Abstract User for create Student and Staff class
    */


    public abstract class User 
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // overload constractor
        public User(string id, string name, string email)
        {
            UserID = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return ($"    {UserID} \t\t {Name}               "+$"\t {Email}");
        }

    }
}
