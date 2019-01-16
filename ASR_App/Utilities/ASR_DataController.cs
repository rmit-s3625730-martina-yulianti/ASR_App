using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using ASR_Model;


namespace Utilities
{
    /* This call to call data from database and create the list of obejct in the ASR system.
     */ 
    class ASR_DataController
    {
        private List<Room> TempRooms = new List<Room>();
        private List<Student> TempStudents = new List<Student>();
        private List<Staff> TempStaffs = new List<Staff>();
        private List<Slot> TempSlots = new List<Slot>();

        private static ASR_DataController instance = null;  // for Singleton pattern
        private DateTime slotDate;

        // Lock the object to create singleton pattern
        private static readonly object lockThis = new object();
        public static ASR_DataController GetStart
        {
            get
            {
                lock (lockThis)
                {
                    if (ASR_DataController.instance == null)
                        instance = new ASR_DataController();
                    return instance;
                }
            }
        }

        // ASR_DataController constractor 
        public ASR_DataController()
        {
            // Initiate the application including reading the data from database     
            try
            {
                TempRooms = CreateRooms();
                TempSlots = CreateSlots();
                TempStaffs = CreateStaffs();
                TempStudents = CreateStudents();
            }
            catch (SqlException err)
            {
                if (err.Number == 53)
                {
                    System.Console.WriteLine("SORRY, ASR Database is Offline now. The system is using offline mode");
                    System.Console.WriteLine("The ASR functionality cannot work properly. Try again later.");
                    System.Console.WriteLine("********************************************************************\n");
                }
                else
                {
                    System.Console.WriteLine(err.Message);
                }
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.ToString());
            }
        } // End of Constractor

        // Create the Staffs list that available in database
        private List<Student> CreateStudents()
        {
            List<Student> tempStudents = new List<Student>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where SUBSTRING(UserID,1,1)='s'";

                var reader = command.GetDataTable().CreateDataReader();
                while (reader.Read())
                {
                    // Student validation (start with "s" and followed by 7 numbers)
                    if ($"{reader["UserID"]}".StartsWith("s"))
                    {
                        // remove extra space from database reading
                        string studentId = $"{reader["UserID"]}".Replace(" ", String.Empty);
                        // check if it's followed by 7 numbers
                        if (int.TryParse(studentId.Substring(1, studentId.Length - 1), out int stdInt))
                        {
                            if (studentId.Substring(1, studentId.Length - 1).Length == 7)
                            {
                                // validation of student's email address
                                int IndAt = $"{reader["Email"]}".IndexOf("@");
                                string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                                if (atEmail.Equals("student.rmit.edu.au"))
                                {
                                    // create student object for valid student
                                    tempStudents.Add(new Student($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                                }
                            }
                        }
                    }
                } // End While

            }
            return tempStudents;

        } // End of CreateStudents()

        // Create the Students list that available in database
        private List<Staff> CreateStaffs()
        {
            List<Staff> tempStaffs = new List<Staff>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where SUBSTRING(UserID,1,1)='e'";

                var reader = command.GetDataTable().CreateDataReader();
                while (reader.Read())
                {
                    // Staff validation (start with "e" and followed by 5 numbers)
                    if ($"{reader["UserID"]}".StartsWith("e"))
                    {
                        // remove extra space from database reading
                        string staffId = $"{reader["UserID"]}".Replace(" ", String.Empty);
                        // check if it's followed by 5 numbers
                        if (int.TryParse(staffId.Substring(1, staffId.Length - 1), out int staffInt))
                        {
                            if (staffId.Substring(1, staffId.Length - 1).Length == 5)
                            {
                                // Validation of Staff's email address
                                int IndAt = $"{reader["Email"]}".IndexOf("@");
                                string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                                if (atEmail.Equals("rmit.edu.au"))
                                {
                                    // Create staff object for valid staff
                                    tempStaffs.Add(new Staff($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                                }
                            }
                        }

                    }
                }
            }

            return tempStaffs;
        } // End of CreateStaffs()

        // Read the Rooms list that available in database
        private List<Room> CreateRooms()
        {
            List<Room> tempRooms = new List<Room>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Room";

                tempRooms = command.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();

            }
            return tempRooms;
        } // End of CreateRooms()

        // Read the Rooms list that available in database
        private List<Slot> CreateSlots()
        {
            List<Slot> Slots = new List<Slot>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Slot";

                Slots = command.GetDataTable().Select().
                    Select(x => new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                        (x["BookedInStudentID"] == DBNull.Value) ? "-" : (string)x["BookedInStudentID"])).ToList();

            }
            return Slots;

        } // End of CreateRooms()

        // Refresh the slots list everytime there is new slot inserted of updated
        public List<Slot> RefreshDatabase() => CreateSlots();
        
        // Return Staff List
        public List<Staff> GetStaffs() => TempStaffs;

        // Return Student List
        public List<Student> GetStudents() => TempStudents;

        // Return Room List
        public List<Room> GetRooms() => TempRooms;

        // Return Slot list
        public List<Slot> GetSlots() => TempSlots;
    }
}
