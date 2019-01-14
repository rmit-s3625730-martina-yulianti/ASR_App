using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using ASR_Model;
using ASR_App;

//using System.Data.SqlClient;

namespace Controller
{
    /* Implement Singelton pattern
     * The ASRController is the main driver to run the functionality in ASR application.
     * Reference : Tutorial week 3
     */ 
    class ASRController
    {
        private static ASRController instance = null;  // for Singleton pattern

        //private IList<Schedule> Schedules = new List<Schedule>();
        private List<Room> TempRooms = new List<Room>();
        private List<Staff> TempStaffs = new List<Staff>();
        private List<Student> TempStudents = new List<Student>();
        private List<Slot> Slots = new List<Slot>();
        private DateTime slotDate;
        StaffController StaffManagement = new StaffController();
        StudentController StudentManagement = new StudentController();

        // Lock the object to create singleton pattern
        private static object lockThis = new object();
        public static ASRController GetStart
        {
            get
            {
                lock (lockThis)
                {
                    if (ASRController.instance == null)
                        instance = new ASRController();
                    return instance;
                }

            }

        }

        public ASRController()
        {
            // Initiate the application including reading the data from database
            try
            {
                //System.Console.WriteLine("Connect to SQL Server.");

                TempRooms = CreateRooms();
                TempStaffs = CreateStaffs();
                TempStudents = CreateStudents();
                Slots = CreateSlots();
            
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.ToString());
            }
 
        } // End of Constractor

        private void RefreshDatabase()
        {
            Slots = CreateSlots();
        }

        // Read the Rooms list that available in database
        public static List<Room> CreateRooms()
        {
            List<Room> tempRooms = new List<Room>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Room";

                tempRooms = command.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                System.Console.WriteLine("Rooms list created!\n"); // Delete this if already finish
            }
            return tempRooms;

        } // End of CreateRooms()

        // Read the Rooms list that available in database
        public static List<Slot> CreateSlots()
        {
            List<Slot> Slots = new List<Slot>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Slot";

                Slots = command.GetDataTable().Select().
                    Select(x => new Slot((string)x["RoomID"],(DateTime)x["StartTime"],(string)x["StaffID"],
                        (x["BookedInStudentID"] == DBNull.Value)? "-" : (string)x["BookedInStudentID"])).ToList();
                System.Console.WriteLine("Slot list created!\n"); // Delete this if already finish
            }
            return Slots;

        } // End of CreateRooms()

        // Create the Staffs list that available in database
        public static List<Student> CreateStudents()
        {
            List<Student> tempStudents = new List<Student>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where SUBSTRING(UserID,1,1)='s'";

                var reader = command.GetDataTable().CreateDataReader();
                while (reader.Read())
                {
                    if ($"{reader["UserID"]}".StartsWith("s"))
                    {
                        int IndAt = $"{reader["Email"]}".IndexOf("@");
                        string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                        if (atEmail.Equals("student.rmit.edu.au"))
                        {
                            tempStudents.Add(new Student($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid student email");
                            continue;
                        }
                    }
                }
                System.Console.WriteLine("Staffs list created!\n"); // Delete this if already finish
            }

            return tempStudents;

        } // End of CreateStaffs()

        // Create the Students list that available in database
        public static List<Staff> CreateStaffs()
        {
            List<Staff> tempStaffs = new List<Staff>();
            using (var connection = ASRDatabase.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where SUBSTRING(UserID,1,1)='e'";

                var reader = command.GetDataTable().CreateDataReader();
                while (reader.Read())
                {
                    if ($"{reader["UserID"]}".StartsWith("e"))
                    {
                        int IndAt = $"{reader["Email"]}".IndexOf("@");
                        string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                        if (atEmail.Equals("rmit.edu.au"))
                        {
                            tempStaffs.Add(new Staff($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid staff email");
                            continue;
                        }
                    }
                }
                System.Console.WriteLine("Users list created!\n"); // Delete this if already finish
            }

            return tempStaffs;

        } // End of CreateUsers()

        // Check the room is valid or not
        private bool CheckRoom(string roomName)
        {
            bool check = false;
            foreach(Room room in TempRooms)
            {
                if (room.RoomName == roomName.ToUpper())
                {
                    check = true;
                    break;
                }
                else continue;
            }
            return check;
        } // End of CheckRoom

        // List the rooms that available in the school
        public void ListRooms()
        {
            System.Console.WriteLine("------List Room------");
            System.Console.WriteLine("      Room Name");
            if (TempRooms.Count > 0)
            {
                foreach (Room room in TempRooms)
                    System.Console.WriteLine($"      {room.RoomName}");
            }
            else
            {
                System.Console.WriteLine("\t Rooms have not been created yet.");
            }
            System.Console.WriteLine();

        } // End of ListRooms()
       
        // List the slots that available in the system
        public void ListSlots()
        {
            //int found = 0;

            System.Console.WriteLine("--- List Slots ---");

            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for slots (dd-mm-yyyy) "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else
                {
                    check = true;
                }
            }

            if (Slots.Count==0)
            {
                System.Console.WriteLine("\nThere is no slot that available now.");
            }
            else
            {
                System.Console.WriteLine($"\nSlots on {slotDate}");
                System.Console.WriteLine("    Room name \t Start time \t End time \t Staff ID \t Bookings");
                System.Console.WriteLine("--------------------------------------------------------------------------------");

                var slotQuery = Slots.Where(x => x.SlotDatetime == slotDate).ToList();
                if (slotQuery.Count==0)
                {
                    System.Console.WriteLine($"No slot available at {slotDate.ToShortDateString()}");
                }
                else
                {
                    foreach (Slot slot in slotQuery)
                        System.Console.WriteLine(slot);
                }
            }

        } // End of listSlots

        // List all staffs
        public void ListStaffs()
        {
            System.Console.WriteLine("--- List Staffs ---");
            System.Console.WriteLine("    ID \t\t\t Name                  " + $"\t Email");
            foreach (Staff staff in TempStaffs)
            {
                if (staff.UserID.StartsWith("e"))
                {
                    System.Console.WriteLine(staff.ToString());
                }
                else
                {
                    continue;
                }
            }
            System.Console.WriteLine("----------------------------------------------------------------------------");

        } // End of ListStaffs()

        public void ListStudents()
        {
            System.Console.WriteLine("--- List Students ---");
            System.Console.WriteLine("    ID \t\t\t Name                  " + $"\t Email");
            foreach (Student student in TempStudents)
            {
                if (student.UserID.StartsWith("s"))
                {
                    System.Console.WriteLine(student.ToString());
                }
                else
                {
                    continue;
                }
            }
            System.Console.WriteLine("----------------------------------------------------------------------------");

        } // End of ListStaffs()

        // List all room availabilities
        public void ListRoomAvailability()
        {
            System.Console.WriteLine("--- Room Availability ---");

            // Checking date input 
            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for room availability (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else
                {
                    check = true;
                }
            }

            System.Console.WriteLine($"\nRooms available on {slotDate}:\n");
            System.Console.WriteLine("\tRoom name \tAvailability (slots)");

            foreach (Room room in TempRooms)
            {
                int countRoom = Slots.Where(x => x.SlotDatetime == slotDate && x.RoomName == room.RoomName).Count();
                System.Console.WriteLine(room.RoomAvailability(countRoom));
            }

            System.Console.WriteLine("----------------------------------------------------------");

        } // End of ListRoomAvailability() 
        
        // List all room availabilities
        public void StaffAvailability()
        {
            System.Console.WriteLine("--- Staff Availability ---");

            // Checking date input 
            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for staff availability (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else  { check = true; }
            }
  
            var staffID = Util.Console.Ask("Enter staff ID: ");

            // Check whether the staffID is valid or not
            if (TempStaffs.Where(x => x.UserID == staffID).Any())
            {
                var slotQuery = Slots.Where(x => x.SlotDatetime == slotDate && x.StaffID == staffID && x.StudentBookingID == "-").ToList();

                if (slotQuery.Count != 0)
                {
                    System.Console.WriteLine($"\nStaff {staffID} availability on {slotDate}:\n");
                    System.Console.WriteLine("\tRoom name \t Start time \t End time");
                    foreach(Slot slot in slotQuery)
                    {
                        System.Console.WriteLine($"\t{slot.RoomName,-17} {slot.StartTime,-15} {slot.EndTime}");
                    }
                    System.Console.WriteLine("----------------------------------------------------");
                }
                else
                {
                    System.Console.WriteLine($"\nStaff ID: {staffID} is not available at {slotDate.ToShortDateString(),-15:dd/MM/yyyy}");
                }
            }
            else
            {
                System.Console.WriteLine("Invalid staff id");
            }           

        } // End of StaffAvailability()
        
        // Staff create a slot
        public void CreateSlot()
        {
            if (StaffManagement.CreateSlot(TempStaffs, Slots))
                RefreshDatabase();
        }

        // Staff remove a slot
        public void RemoveSlot()
        {
            if (StaffManagement.RemoveSlot(Slots))
                RefreshDatabase();
        }

        // Student make a booking slot
        public void MakeBooking()
        {
            if (StudentManagement.MakeBooking(TempStudents, Slots))
                RefreshDatabase();
        }

        // Student cancel a booking slot
        public void CancelBooking()
        {
            if (StudentManagement.CancelBooking(TempStudents, Slots))
                RefreshDatabase();
        }


    } // End of ASRController

}
