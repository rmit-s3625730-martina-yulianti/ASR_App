using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    /* Main Program of Appointment Scheduling Reservation Application */

    class ASR_App
    {
        List<Room> Rooms = new List<Room>();

        // Constractor
        public ASR_App()
        {
            // initiate the rooms in the ASR App
            Rooms.Add(new Room("A"));
            Rooms.Add(new Room("B"));
            Rooms.Add(new Room("C"));
            Rooms.Add(new Room("D"));

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
                    int option = Util.Console.AskInt("Enter option: ");

                    switch (option)
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
            Console.WriteLine("Show staff's menu"); // dummy function
        }

        // Display menu for student
        private void StudentMenu()
        {
            Console.WriteLine("Show student's menu"); // dummy function
        }
    }
}
