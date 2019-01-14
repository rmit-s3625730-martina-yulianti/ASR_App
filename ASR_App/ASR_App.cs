using System;
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
            Driver = ASRController.GetStart;

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

        // Display menu for staff
        private void StaffMenu()
        {
            bool staffMenu = true;
            while (staffMenu)
            {
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
                        Driver.ListStaffs();
                        StaffMenu(); // back to staff menu option
                        break;
                    case 2:
                        Driver.ListRoomAvailability();
                        StaffMenu(); // back to staff menu option
                        break;

                    case 3:
                        Driver.CreateSlot();
                        StaffMenu();
                        break;

                    case 4:
                        Driver.RemoveSlot();
                        StaffMenu();
                        break;

                    case 5:
                        Console.WriteLine("Exit staff menu ..."); // back to main menu
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Choose between 1 - 5, try again");
                        break;

                }

                staffMenu = false;
            }

        } // End of StaffMenu

        // Display menu for student
        private void StudentMenu()
        {
            bool studentMenu = true;
            while (studentMenu)
            {
                Console.WriteLine("\nEntering student menu");
                Console.WriteLine("\n---------------------------------------");
                Console.WriteLine("Student menu:");
                Console.WriteLine("\t 1. List student");
                Console.WriteLine("\t 2. Staff availability");
                Console.WriteLine("\t 3. Make booking");
                Console.WriteLine("\t 4. Cancel booking");
                Console.WriteLine("\t 5. Exit");
                Console.WriteLine("-----------------------------");
                int studentOpt = Util.Console.AskInt("Enter option: ");

                switch (studentOpt)
                {
                    case 1:
                        Driver.ListStudents();
                        StudentMenu(); // back to staff menu option
                        break;
                    case 2:
                        Driver.StaffAvailability();
                        StudentMenu(); // back to staff menu option
                        break;

                    case 3:
                        Driver.MakeBooking();
                        StudentMenu();
                        break;

                    case 4:
                        Driver.CancelBooking();
                        StudentMenu();
                        break;

                    case 5:
                        Console.WriteLine("Exit student menu ..."); // back to main menu
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Choose between 1 - 5, try again");
                        break;
                }

                studentMenu = false;
            }
        } // End of StudentMenu

    }
}
