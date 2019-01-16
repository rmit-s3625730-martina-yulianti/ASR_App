using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Utilities;
using ASR_Model;
using Console = System.Console;

namespace Controller
{
    /* Implement design pattern of Singleton
     * The ASRController is the main driver to run the ASR functionalities in ASR application,
     * together with the StaffController and StudentController.
     */

    public class ASRController
    {
        private static ASRController instance = null;  // for Singleton pattern

        private List<Room> TempRooms = new List<Room>();
        private List<Slot> Slots = new List<Slot>();
       
        private StaffController StaffManagement;
        private StudentController StudentManagement;
        private DateTime slotDate;

        // Lock the object to create singleton pattern
        private static readonly object lockThis = new object();
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

        // ASRController constractor 
        public ASRController()
        {
            // Initiate the application including reading the data from database     
            try
            {
                TempRooms = CreateRooms();
                Slots = CreateSlots();
                StudentManagement = new StudentController(CreateStudents());
                StaffManagement = new StaffController(CreateStaffs());

            }
            catch(SqlException err)
            {
                if (err.Number == 53)
                {                  
                    Console.WriteLine("SORRY, ASR Database is Offline now. The system is using offline mode");
                    Console.WriteLine("The ASR functionality cannot work properly. Try again later.");
                    Console.WriteLine("********************************************************************\n");
                }
                else
                {
                    Console.WriteLine(err.Message);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        } // End of Constractor

        // Refresh the slots list everytime there is new slot inserted of updated
        private void RefreshDatabase()
        {
            Slots = CreateSlots();
        }

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
                    Select(x => new Slot((string)x["RoomID"],(DateTime)x["StartTime"],(string)x["StaffID"],
                        (x["BookedInStudentID"] == DBNull.Value)? "-" : (string)x["BookedInStudentID"])).ToList();
               
            }
            return Slots;

        } // End of CreateRooms()

        // List the rooms that available in the database
        public void ListRooms()
        {
            Console.WriteLine("\n------List Room------");
            Console.WriteLine("\n      Room Name");
            if (TempRooms.Count > 0)
            {
                foreach (Room room in TempRooms)
                    Console.WriteLine($"      {room.RoomName}");
            }
            else
            {
                Console.WriteLine("\t Room has not been created yet.");
            }
            Console.WriteLine();

        } // End of ListRooms()

        // List the slots that available in the system
        public void ListSlots()
        {
            Console.WriteLine("\n--- List Slots ---");

            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter date for slots (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else {check = true;}
            }

            if (Slots.Count==0)
            {
                Console.WriteLine("\nThere is no slot that available now.");
            }
            else
            {
                Console.WriteLine($"\nSlots on {slotDate.ToShortDateString()}");
                Console.WriteLine("\n    Room name \t Start time \t End time \t Staff ID \t Bookings");
                Console.WriteLine("--------------------------------------------------------------------------------");

                var slotQuery = Slots.Where(x => x.SlotDatetime == slotDate).ToList();
                if (slotQuery.Count==0)
                {
                    Console.WriteLine($"No slot available at {slotDate.ToShortDateString()}");
                }
                else
                {
                    foreach (Slot slot in slotQuery)
                        Console.WriteLine(slot);
                }
            }
        } // End of listSlots

        // List all room availabilities
        public void ListRoomAvailability()
        {
            Console.WriteLine("\n--- Room Availability ---");

            // Checking date input 
            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter date for room availability (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else {check = true;}
            }

            Console.WriteLine($"\nRooms available on {slotDate.ToShortDateString()}:\n");
            Console.WriteLine("\tRoom name \tAvailability (slots)");

            foreach (Room room in TempRooms)
            {
                int countRoom = Slots.Where(x => x.SlotDatetime == slotDate && x.RoomName == room.RoomName).Count();
                Console.WriteLine(room.RoomAvailability(countRoom));
            }

            Console.WriteLine("----------------------------------------------------------");

        } // End of ListRoomAvailability() 
        
        // List staff availabilities
        public void StaffAvailability()
        {
            StaffManagement.StaffAvailability(Slots);

        } // End of StaffAvailability()

        // List all students
        public void ListStudents()
        {
            StudentManagement.ListStudents();

        } // End of ListStudent()

        // List all staffs
        public void ListStaffs()
        {
            StaffManagement.ListStaffs();

        } // End of ListStaffs()

        // Staff create a slot
        public void CreateSlot()
        {
            if (StaffManagement.CreateSlot(Slots, TempRooms))
                RefreshDatabase();
        }

        // Staff remove a slot
        public void RemoveSlot()
        {
            if (StaffManagement.RemoveSlot(Slots, TempRooms))
                RefreshDatabase();
        }

        // Student make a booking slot
        public void MakeBooking()
        {
            if (StudentManagement.MakeBooking(Slots, TempRooms))
                RefreshDatabase();
        }

        // Student cancel a booking slot
        public void CancelBooking()
        {
            if (StudentManagement.CancelBooking(Slots))
                RefreshDatabase();
        }

    } // End of ASRController

}
