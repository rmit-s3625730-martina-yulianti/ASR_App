using System;

namespace ASR_Model
{
    /* Student class is a model class to create container for student object from User table
     */ 

    public class Student : User
    {   
        public Student(string id, string name, string email) : base(id, name, email) { }
    }
}
