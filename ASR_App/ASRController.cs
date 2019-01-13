using System;
using System.Collections.Generic;
using Util;
using ASR_Model;
using ASR_App;
using System.Linq;
using System.Globalization;

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
            
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.ToString());
            }
 
        } // End of Constractor

        // Read the Rooms list that available in database
        public static List<Room> CreateRooms()
        {
            List<Room> tempRooms = new List<Room>();
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Room";

                tempRooms = command.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                System.Console.WriteLine("Rooms list created!\n"); // Delete this if already finish
            }
            return tempRooms;

        } // End of CreateRooms()

        // Create the Staffs list that available in database
        public static List<Student> CreateStudents()
        {
            List<Student> tempStudents = new List<Student>();
            using (var connection = Program.ConnectionString.CreateConnection())
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
            using (var connection = Program.ConnectionString.CreateConnection())
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
            int found = 0;

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

            System.Console.WriteLine($"\nSlots on {slotDate}");
            System.Console.WriteLine("    Room name \t Start time \t End time \t Staff ID \t Bookings");
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            foreach (Slot slot in Slots)
            {
                if (slot.SlotDate == slotDate)
                {
                    foreach(Room room in slot.GetRooms())
                    {
                        if (room.Schedules.Count > 0)
                        {
                            foreach (Schedule sch in room.Schedules)
                            {
                                System.Console.WriteLine($"    {room.RoomName} \t\t {sch.ToString()}");
                            }
                        }                   
                    }
                    found++;
                }
                else
                {
                    continue;
                }
            }
            System.Console.WriteLine((found == 0) ? "    No slots available at this date." : "--------------------------------------------------------------------------------");
        }

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
            int found = 0;
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
            
            if (Slots.Count == 0)
            {
                try
                {
                    TempRooms = CreateRooms();
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
            else
            {
                foreach(Slot slot in Slots)
                {
                    if(slot.SlotDate == slotDate)
                    {
                        TempRooms = slot.GetRooms();
                    }
                    else
                    {
                        TempRooms = CreateRooms();
                    }
                }
            }
            
            System.Console.WriteLine($"\nRooms available on {slotDate}:\n");
            System.Console.WriteLine("\tRoom name \t Availability (slots)");

            foreach (Room room in TempRooms)
            {
                // First check the RoomSlots (max = 2)
                if (room.RoomAvailability())
                {
                    System.Console.WriteLine($"\t{room.RoomName} \t\t\t {room.RoomSlots}");
                    found++;

                }
                else
                {
                    continue;
                }
            }

            System.Console.WriteLine((found == 0) ? "No room available." : "-----------------------------------------------");

        } // End of ListRoomAvailability() 

        public void CreateSlot()
        {
            // create slot  
            System.Console.WriteLine("\n--- Create slot ---\n");
            var slotRoom = "";
            try
            {
                slotRoom = Util.Console.AskChar("Enter room name: ");
                if (!CheckRoom(slotRoom))
                {
                    System.Console.WriteLine("Room name is invalid. Try again");
                    CreateSlot();
                }

                // Checking date input from user   
                var check = false;
                while (!check)
                {
                    if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for slot (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                    {
                        System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                    }
                    else
                    {
                        check = true;
                    }
                }

                var slotTime = Util.Console.Ask("Enter time for slot (hh:mm): ");  // Business rule hasn't been implemented

                // Check whether staffId valid
                var staffID = Util.Console.Ask("Enter staff ID: ");
                var valid = true;
                foreach(Staff staff in TempStaffs)
                {
                    if (staff.UserID == staffID)
                    {
                        if (Slots.Count == 0)
                        {
                            Slots.Add(new Slot(staffID, slotRoom.ToUpper(), slotDate, slotTime));
                        }
                        else
                        {   // Slot date already exists
                            foreach (Slot slot in Slots)
                            {
                                if (slot.SlotDate == slotDate)
                                {
                                    // Check whether staff create the same schedule in the other rooms
                                    var foundTheSameSch = 0;
                                    foreach (Room room in slot.GetRooms())
                                    {
                                        if (room.RoomAvailability())
                                        {
                                            foreach (Schedule sch in room.Schedules)
                                            {
                                                if (sch.StaffID == staffID && sch.StartTime == slotTime)
                                                {
                                                    foundTheSameSch++;
                                                }

                                            } // End of foreach room.Schedule
                                        } // End of if else room.Availability

                                    } // End of foreach slot.GetRooms()
                                    if (foundTheSameSch > 0)
                                    {
                                        System.Console.WriteLine("Unable to create slot.Double schedule in same or different room.");
                                        return;
                                    }
                                    else if (foundTheSameSch == 0)
                                    {
                                        try
                                        {
                                            slot.AddSchedule(slotRoom.ToUpper(), staffID, slotTime);
                                        }
                                        catch(SlotException err)
                                        {
                                            System.Console.WriteLine(err.Message);
                                        }
                                        catch(Exception e)
                                        {
                                            System.Console.WriteLine(e.Message);
                                        }
                                    }
                                } // End of if(slot.SlotDate == slotDate)

                            } // End of foreach slots
                        }
                        break;
                    } // End of if staff.UserID == staffID
                    valid = false;
                }

                if (!valid) { System.Console.WriteLine("Invalid User."); }

            }
            catch (SlotException err)
            {
                System.Console.WriteLine(err.Message);
            }
            catch (FormatException e)
            {
                System.Console.WriteLine(e.Message);
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.Message);
            }

        } // end of CreateSlot()

    } // End of ASRController

}
