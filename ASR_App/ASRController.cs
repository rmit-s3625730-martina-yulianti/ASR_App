using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ASR_App;

namespace Controller
{
    public class ASRController
    {
        private List<Room> Rooms = new List<Room>();
        private List<User> Users = new List<User>();
        private List<Slot> Slots = new List<Slot>();
        private SqlCommand command;
        private SqlDataReader reader;
        private string query;
        private SqlConnection connection;

        public ASRController()
        {
            // Initiate the application including reading the data from database

            try
            {
                Console.WriteLine("Connect to SQL Server.");

                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "wdt2019.australiasoutheast.cloudapp.azure.com";
                builder.UserID = "s3625730";
                builder.Password = "abc123";
                builder.InitialCatalog = "s3625730";

                // Connect to SQL Server
                Console.WriteLine("\nConecting to SQL server ....");
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
                        Console.WriteLine("Room table is empty.");
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

            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }
 

        }

        // List the rooms that available in the school
        public void ListRooms()
        {
            Console.WriteLine("------List Room------");
            Console.WriteLine("      Room Name");
            if (Rooms.Count > 0)
            {
                foreach (Room room in Rooms)
                    Console.WriteLine($"      {room.RoomName}");
            }
            Console.WriteLine();

        }

        // List the slots that available in the system
        public void ListSlots()
        {
            int found = 0;

            Console.WriteLine("--- List Slots ---");
            var query = Util.Console.Ask("Enter date for slots (dd-mm-yyyy) ");
            Console.WriteLine($"\nSlots on {query}");
            Console.WriteLine("    Room name \t Start time \t End time \t Staff ID \t Bookings");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach (Slot slot in Slots)
            {
                if (slot.SlotDate.Equals(query))
                {
                    Console.WriteLine(slot.ToString());
                    found++;
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine((found == 0) ? "    No slots available at this date." : "--------------------------------------------------------------------------------");
        }

        // List all staffs
        public void ListStaffs()
        {
            Console.WriteLine("--- List Staffs ---");
            Console.WriteLine("    IDName \t\t Name                      " + $"\t Email");
            foreach (User user in Users)
            {
                if (user.UserID.StartsWith("e"))
                {
                    Console.WriteLine(user.ToString());
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("----------------------------------------------------------------------------");

        }

        // List all room availabilities
        public void ListRoomAvailability()
        {
            int found = 0;
            Console.WriteLine("--- Room Availability ---");
            var date = Util.Console.Ask("Enter date for room availability (dd-mm-yyyy): ");

            Console.WriteLine($"\nRooms available on {date}:\n");
            Console.WriteLine("\tRoom name \t Availability (slots)");


            foreach (Room room in Rooms)
            {
                // First check the RoomSlots (max = 2)
                if (room.RoomSlots > 0 && room.RoomSlots <= 2)
                {
                    // Check the date
                    foreach (Schedule roomDate in room.Schedules)
                    {
                        // the room still empty
                        if (roomDate.Date.Equals("-") || roomDate.Date.Equals(date))
                        {
                            Console.WriteLine($"\t{room.RoomName} \t\t\t {room.RoomSlots}");
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

            Console.WriteLine((found == 0) ? "No room available." : "-----------------------------------------------");
        }

        public void CreateSlot()
        {
            // create slot
           
            Console.WriteLine("\n--- Create slot ---\n");
            var slotRoom = Util.Console.Ask("Enter room name: ");
            var slotDate = Util.Console.Ask("Enter date for slot (dd-mm-yyyy): ");
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
                                ((Staff)user).AddSlot();
                                try
                                {
                                    room.AddSchedule(slotDate, slotTime); // fix this later
                                }
                                catch (SlotException err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                Slots.Add(new Slot(staffID,slotRoom,slotDate,slotTime));
                                Console.WriteLine("\nSlot created successfully!");
                                break;
                            }
                            else { continue; }
                        }

                    
                   

                }
                else { continue; }

            }
            
        }

    }

}
