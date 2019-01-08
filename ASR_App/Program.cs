using System;

namespace ASR_App
{
    /* Main Program of Appointment Scheduling Reservation Application
     */


    class Program
    {
        static void Main(string[] args)
        {

            MainMenu();

            Console.ReadLine();
        }

        // Main Menu of ASR
        static void MainMenu()
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
        static void ListRooms()
        {
            Console.WriteLine("Show rooms availabilitis"); // dummy function

        }

        // List the slots that available in the system
        static void ListSlots()
        {
            Console.WriteLine("Show slots availabilitis"); // dummy function
        }

        // Display menu for staff
        static void StaffMenu()
        {
            Console.WriteLine("Show staff's menu"); // dummy function
        }

        // Display menu for student
        static void StudentMenu()
        {
            Console.WriteLine("Show student's menu"); // dummy function
        }
    }
}
