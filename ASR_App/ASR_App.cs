using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    /* Main Program of Appointment Scheduling Reservation Application */
    
    class ASR_App
    {
        
        List<Room> Rooms = new List<Room>();
        List<User> Users = new List<User>();
        List<Slot> Slots = new List<Slot>();

        // Constractor
        public ASR_App()
        {
            // Initiate the rooms in the ASR App
            // Later this should be read from the ASRdb (table Room)
            Rooms.Add(new Room("A"));
            Rooms.Add(new Room("B"));
            Rooms.Add(new Room("C"));
            Rooms.Add(new Room("D"));

            // Initiate Users from ASPdb
            // dummy
            Users.Add(new Student("s1234567", "John Doe", "s1234567.student.rmit.edu.au"));
            Users.Add(new Student("s2345678", "Jimmy Kim", "s2345678.student.rmit.edu.au"));
            Users.Add(new Staff("e12345", "William Smiths", "e12345.rmit.edu.au"));
            Users.Add(new Staff("e23456", "Annie Lim", "e23456.rmit.edu.au"));


            // Initiate Slots from ASPdb 


        }

        // import all the nescessary files including database
        private void Imports()
        {

        }

        public void Start()
        {
            MainMenu();

            Console.ReadLine();
        }

        // Main Menu of ASR
        private void MainMenu()
        {
            bool mainMenu = true;

            while (mainMenu)
            {
                try
                {
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Welcome to Appointment Scheduling and Reservation System");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("    1. List rooms");
                    Console.WriteLine("    2. List slots");
                    Console.WriteLine("    3. Staff menu");
                    Console.WriteLine("    4. Student menu");
                    Console.WriteLine("    5. Exit");
                    Console.WriteLine("-----------------------------");
                    int mainOpt = Util.Console.AskInt("Enter option: ");

                    switch (mainOpt)
                    {
                        case 1:
                            ListRooms();
                            break;
                        case 2:
                            ListSlots();
                            break;

                        case 3:
                            StaffMenu();
                            break;

                        case 4:
                            StudentMenu();
                            break;

                        case 5:
                            Console.WriteLine("Terminating ASR App ...");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Choose between 1 - 5, try again");
                            break;

                    }
                }
                catch (FormatException err)
                {
                    Console.WriteLine(err.Message);
                    MainMenu();
                }
                catch (Exception)
                {
                    Console.WriteLine("Application error");
                }
                mainMenu = false;
            }

        }

        // List the rooms that available in the school
        private void ListRooms()
        {
            Console.WriteLine("------List Room------");
            Console.WriteLine("      Room Name");
            if (Rooms.Count > 0)
            {
                foreach (Room room in Rooms)
                    Console.WriteLine($"      {room.RoomName}");
            }
            Console.WriteLine();
            MainMenu();

        }

        // List the slots that available in the system
        private void ListSlots()
        {
            Console.WriteLine("Show slots availabilitis"); // dummy function
        }

        // Display menu for staff
        private void StaffMenu()
        {
            bool staffMenu = true;

            while (staffMenu)
            {
                try
                {
                    Console.WriteLine("Entering staff menu");
                    Console.WriteLine("\n---------------------------------------");
                    Console.WriteLine("Staff menu:");
                    Console.WriteLine("\t 1. List staff");
                    Console.WriteLine("\t 2. Room availability");
                    Console.WriteLine("\t 3. Create slot");
                    Console.WriteLine("\t 4. Remove slot");
                    Console.WriteLine("\t 5. Exit");
                    Console.WriteLine("-----------------------------");
                    int staffOpt = Util.Console.AskInt("Enter option: ");

                    switch (staffOpt)
                    {
                        case 1:
                            // List all staffs
                            Console.WriteLine("--- List Staffs ---");
                            Console.WriteLine("    IDName \t\t Name                      "+$"\t Email");
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
                            StaffMenu(); // back to staff menu option
                            break;
                        case 2:
                            // List all room availabilities
                            int found = 0;
                            Console.WriteLine("--- Room Availability ---");
                            var date = Util.Console.Ask("Enter date for room availability (dd-mm-yyyy): ");

                            Console.WriteLine($"\nRooms available on {date}:\n");
                            Console.WriteLine("\tRoom name \t Availability (slots)");
                            
                           
                            foreach(Room room in Rooms)
                            {
                                // First check the RoomSlots (max = 2)
                                if (room.RoomSlots > 0 && room.RoomSlots <=2) 
                                {
                                    // Check the date
                                    foreach(Schedule roomDate in room.Schedules)
                                    {
                                        // the room still empty
                                        if (roomDate.Date.Equals("-") || roomDate.Date.Equals(date))
                                        {
                                            Console.WriteLine($"\t{room.RoomName} \t\t\t {room.RoomSlots}");
                                            found++;
                                            break;
                                        }else
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
                            StaffMenu(); // back to staff menu option
                            break;

                        case 3:
                            // create slot
                            Console.WriteLine("\n--- Create slot ---\n");
                            var slotRoom = Util.Console.Ask("Enter room name: ");
                            var slotDate = Util.Console.Ask("Enter date for slot (dd-mm-yyyy): ");
                            var slotTime = Util.Console.Ask("Enter time for slot (hh:mm): ");
                            var staffID = Util.Console.Ask("Enter staff ID: ");

                            // Check whether staff ID valid
                            foreach(User user in Users)
                            {
                                if( staffID.StartsWith("e") && user.UserID.Equals(staffID))
                                {
                                    // Check whether staff still can create slot
                                    try {
                                        // Check whether the room is available
                                        foreach (Room room in Rooms)
                                        {
                                            if (room.RoomName.Equals(slotRoom) && room.RoomSlots < 2)
                                            {
                                                
                                                ((Staff)user).AddSlot();
                                                room.AddSchedule(slotDate,slotTime);
                                                Slots.Add(new Slot(staffID, room));
                                                Console.WriteLine("Slot created successfully");
                                                break;
                                            }
                                            else { continue; }
                                        }
                                    }
                                    catch (SlotException err)
                                    {
                                        Console.WriteLine(err.Message);
                                    }

                                }
                                StaffMenu();
                            } 

                            break;

                        case 4:
                            // remove slot
                            break;

                        case 5:
                            Console.WriteLine("Exit staff menu ..."); // back to main menu
                            MainMenu();
                            break;
                        default:
                            Console.WriteLine("Choose between 1 - 5, try again");
                            break;

                    }

                }
                catch(FormatException err)
                {
                    Console.WriteLine(err.Message);
                    StaffMenu();
                }
                catch (Exception)
                {
                    Console.WriteLine("Application error");
                }

                staffMenu = false;
            }

        }

        // Display menu for student
        private void StudentMenu()
        {
            Console.WriteLine("Show student's menu"); // dummy function
        }

     
    }
}
