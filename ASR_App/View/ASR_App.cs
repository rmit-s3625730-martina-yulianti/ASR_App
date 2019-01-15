using System;
using Controller;
using Console = System.Console;

namespace ASR_App
{
    /* Main Program of Appointment Scheduling Reservation Application 
     * ASR_App clas is the view for the ASR application which interact with user directly.
     * This class does not have any logic rules of the application, it needs ASRController 
     * as the brain in ASR system.
     */

    public class ASR_App
    {
        ASRController Driver;

        // Constractor
        public ASR_App()
        {
            // Instantiate the ASR_App controller to run this application
            Driver = ASRController.GetStart;
        }

        // This where the ASR application begins
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
                    Console.WriteLine("Main Menu: ");
                    Console.WriteLine("    1. List rooms");
                    Console.WriteLine("    2. List slots");
                    Console.WriteLine("    3. Staff menu");
                    Console.WriteLine("    4. Student menu");
                    Console.WriteLine("    5. Exit");
                    Console.WriteLine("-----------------------------\n");
                    int mainOpt = Utilities.Console.AskInt("Enter option: ");

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
                            Console.WriteLine("\nTerminating ASR App ...");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Choose between 1 - 5, try again.\n");
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
        } // End of MainMenu

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
                Console.WriteLine("-----------------------------\n");
                int staffOpt = Utilities.Console.AskInt("Enter option: ");

                switch (staffOpt)
                {
                    case 1:
                        Driver.ListStaffs();
                        StaffMenu(); 
                        break;
                    case 2:
                        Driver.ListRoomAvailability();
                        StaffMenu(); 
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
                        Console.WriteLine("\nExit staff menu ..."); 
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Choose between 1 - 5, try again.\n");
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
                Console.WriteLine("-----------------------------\n");
                int studentOpt = Utilities.Console.AskInt("Enter option: ");

                switch (studentOpt)
                {
                    case 1:
                        Driver.ListStudents();
                        StudentMenu(); 
                        break;
                    case 2:
                        Driver.StaffAvailability();
                        StudentMenu(); 
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
                        Console.WriteLine("\nExit student menu ..."); 
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Choose between 1 - 5, try again\n");
                        break;
                }

                studentMenu = false;
            }
        } // End of StudentMenu

    } // End of ASR_App class
}
