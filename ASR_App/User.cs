using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    abstract class User
    {
        //protected string userID;

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // default constractor
        public User()
        {

        }

        // overload constractor
        public User(string id, string name, string email)
        {
            UserID = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return ($"    {UserID.ToString()} \t\t {Name.ToString()}               "+$"\t {Email.ToString()}");
        }
    }
}
