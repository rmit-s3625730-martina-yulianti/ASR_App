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
    public class ASRController
    {
        private static ASRController instance = null;  // for Singleton pattern

        private List<Room> Rooms = new List<Room>();
        private IList<User> Users = new List<User>();
        private List<Slot> Slots = new List<Slot>();
      
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

        private ASRController()
        {
            // Initiate the application including reading the data from database


            try
            {
                //System.Console.WriteLine("Connect to SQL Server.");

                CreateRooms();
                CreateUsers();
                // Build connection string
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "wdt2019.australiasoutheast.cloudapp.azure.com";
                //builder.UserID = "s3625730";
                //builder.Password = "abc123";
                //builder.InitialCatalog = "s3625730";

                // Connect to SQL Server
                /*
                System.Console.WriteLine("\nConecting to SQL server ....");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {               
                    connection.Open();
             
                    // Initiate the rooms in the ASR App
                    query = "Select * from Room";
                    command = new SqlCommand(query, connection);

                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Rooms.Add(new Room($"{reader["RoomID"]}"));
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Room table is empty.");
                    }
                    reader.Close();     

                    // Initiate the Users in the ASR App
                    query = "Select * from [User]";
                    command = new SqlCommand(query, connection);

                    reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                if ($"{reader["UserID"]}".StartsWith("s"))
                                {
                                    int IndAt = $"{reader["Email"]}".IndexOf("@");
                                    string atEmail = $"{reader["Email"]}".Substring(IndAt+1);
                                    if (atEmail.Equals("student.rmit.edu.au"))
                                    {
                                        Users.Add(new Student($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid student email");
                                        continue;
                                    }

                                }
                                else if($"{reader["UserID"]}".StartsWith("e"))
                                {
                                    int IndAt = $"{reader["Email"]}".IndexOf("@");
                                    string atEmail = $"{reader["Email"]}".Substring(IndAt+1);
                                    if (atEmail.Equals("rmit.edu.au"))
                                    {
                                        Users.Add(new Staff($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid student email");
                                        continue;
                                    }

                                }

                            }
               
                }
                */
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err.ToString());
            }
 
        } // End of Constractor

       

        // Read the Rooms list that available in database
        private void CreateRooms()
        {
            using(var connection = Program.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Room";

                Rooms = command.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                System.Console.WriteLine("Rooms list created!\n");
            }
        } // End of CreateRooms()

        // Create the Users list that available in database
        private void CreateUsers()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from [User] where (SUBSTRING(UserID,1,1)='e' OR SUBSTRING(UserID,1,1)='s')";

                var reader = command.GetDataTable().CreateDataReader();
                while (reader.Read())
                {
                    if ($"{reader["UserID"]}".StartsWith("s"))
                    {
                        int IndAt = $"{reader["Email"]}".IndexOf("@");
                        string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                        if (atEmail.Equals("student.rmit.edu.au"))
                        {
                            Users.Add(new Student($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid student email");
                            continue;
                        }

                    }
                    else if ($"{reader["UserID"]}".StartsWith("e"))
                    {
                        int IndAt = $"{reader["Email"]}".IndexOf("@");
                        string atEmail = $"{reader["Email"]}".Substring(IndAt + 1);
                        if (atEmail.Equals("rmit.edu.au"))
                        {
                            Users.Add(new Staff($"{reader["UserID"]}", $"{reader["Name"]}", $"{reader["Email"]}"));
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid student email");
                            continue;
                        }

                    }
                }
                System.Console.WriteLine("Users list created!\n");
            }

        } // End of CreateUsers()

        // Check the room is valid or not
        private bool CheckRoom(string roomName)
        {
            bool check = false;
            foreach(Room room in Rooms)
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
            if (Rooms.Count > 0)
            {
                foreach (Room room in Rooms)
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
            var query = Util.Console.Ask("Enter date for slots (dd-mm-yyyy) ");
            System.Console.WriteLine($"\nSlots on {query}");
            System.Console.WriteLine("    Room name \t Start time \t End time \t Staff ID \t Bookings");
            System.Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach (Slot slot in Slots)
            {
                if (slot.SlotDate.Equals(query))
                {
                    System.Console.WriteLine(slot.ToString());
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
            System.Console.WriteLine("    IDName \t\t Name                      " + $"\t Email");
            foreach (User user in Users)
            {
                if (user.UserID.StartsWith("e"))
                {
                    System.Console.WriteLine(user.ToString());
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
            DateTime date;
            if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for room availability (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)))
            {
                System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                ListRoomAvailability();
            }

            System.Console.WriteLine($"\nRooms available on {date}:\n");
            System.Console.WriteLine("\tRoom name \t Availability (slots)");

            foreach (Room room in Rooms)
            {
                // First check the RoomSlots (max = 2)
                if (room.RoomSlots > 0 && room.RoomSlots <= 2)
                {
                    // Check the date
                    foreach (Schedule roomDate in room.Schedules)
                    {
                        // the room still empty
                        if (roomDate.Date.Equals("01-01-2000") || roomDate.Date.Equals(date))
                        {
                            System.Console.WriteLine($"\t{room.RoomName} \t\t\t {room.RoomSlots}");
                            found++;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
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



            }catch(FormatException e)
            {
                System.Console.WriteLine(e.Message);
            }
           

            // Checking date input from user
            DateTime slotDate;
            if(!(DateTime.TryParseExact(Util.Console.Ask("Enter date for slot (dd-mm-yyyy): "),"dd-MM-yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
            {
                System.Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                CreateSlot();
            }
            var slotTime = Util.Console.Ask("Enter time for slot (hh:mm): ");
            var staffID = Util.Console.Ask("Enter staff ID: ");

            // Check whether staff ID valid
            foreach(User user in Users)
            {
                if (user.UserID.Equals(staffID) && staffID.StartsWith("e"))
                {
                    // Check whether staff still can create slot
                    
                        // Check whether the room is available
                        foreach (Room room in Rooms)
                        {
                            if (room.RoomName.Equals(slotRoom) && room.RoomAvailability())
                            {
                                // If all valid, add schedule in room and slot
                                
                                try
                                {
                                    ((Staff)user).AddSlot();
                                    room.AddSchedule(slotDate, slotTime); // fix this later
                                    Slots.Add(new Slot(staffID, slotRoom, slotDate, slotTime));
                                    System.Console.WriteLine("\nSlot created successfully!");
                                }
                                catch (SlotException err)
                                {
                                    System.Console.WriteLine(err.Message);
                                }
                                catch (Exception err)
                                {
                                    System.Console.WriteLine(err.Message);
                                }
                                
                                break;
                            }
                            else { continue; }
                        }

                }
                else { continue; }

            } // end foreach
            
        } // end of CreateSlot()

    } // End of ASRController

}
