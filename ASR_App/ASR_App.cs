using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Controller;

namespace ASR_App
{
    /* Main Program of Appointment Scheduling Reservation Application */
    
    class ASR_App
    {
        
        ASRController Driver; 

        // Constractor
        public ASR_App()
        {
            
            // Instantiate the ASR_App controller
            Driver = new ASRController();
            
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
                    Console.WriteLine("\n-------------------------------------------------------------");
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
                            Driver.ListRooms();
                            MainMenu();
                            break;
                        case 2:
                            Driver.ListSlots();
                            MainMenu();
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
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                mainMenu = false;
            }

        }

        /*
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
        */

        /*
        // List the slots that available in the system
        private void ListSlots()
        {
            int found = 0;

            Console.WriteLine("--- List Slots ---");
            var query = Util.Console.Ask("Enter date for slots (dd-mm-yyyy) ");
            Console.WriteLine($"\nSlots on {query}");
            Console.WriteLine("    Room name \t Start time \t End time \t Staff ID \t Bookings");
            Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach(Slot slot in Slots)
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
            Console.WriteLine((found==0)? "    No slots available at this date." : "--------------------------------------------------------------------------------");
        }

        */

        // Display menu for staff
        private void StaffMenu()
        {
            bool staffMenu = true;

            while (staffMenu)
            {
                //try
                //{
                    Console.WriteLine("\nEntering staff menu");
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
                            /*
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
                            */
                            Driver.ListStaffs();
                            StaffMenu(); // back to staff menu option
                            break;
                        case 2:
                            // List all room availabilities
                            /*
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
                            */
                            Driver.ListRoomAvailability();
                            StaffMenu(); // back to staff menu option
                            break;

                        case 3:
                            // create slot
                            /*
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
                                    try
                                    {
                                        // Check whether the room is available
                                        foreach (Room room in Rooms)
                                        {
                                            if (room.RoomName.Equals(slotRoom) && room.RoomAvailability())
                                            {
                                                // If all valid, add schedule in room and slot
                                                ((Staff)user).AddSlot();
                                                room.AddSchedule(slotDate, slotTime); // fix this later
                                                Slots.Add(new Slot(staffID,slotRoom,slotDate,slotTime));
                                                Console.WriteLine("\nSlot created successfully!");
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
                                else { continue; }

                            }
                            */
                            Driver.CreateSlot();
                            StaffMenu();
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

                //}
                //catch(FormatException err)
                //{
                //    Console.WriteLine(err.Message);
                //    StaffMenu();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Application error");
                //}

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
