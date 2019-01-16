using System;
using Console = System.Console;
using ASRMenuFactory;
using Utilities;
using View;

namespace Controller
{
    /* Main Program of Appointment Scheduling Reservation Application 
     * ASR_App clas is the controller for the ASR application.
     */
    public class ASRController
    {
        private ASR_DataController Data;
        private MenuFactory factoryMenu;
        private StaffView StaffManagement;
        private StudentView StudentManagement;
        private MainView MainManagement;

        // Constractor
        public ASRController()
        {
            // Instantiate the ASR_App controller to run this application
            Data = ASR_DataController.GetStart;
            factoryMenu = new MenuFactory();
       
            StaffManagement = new StaffView(Data.GetStaffs());
            StudentManagement = new StudentView(Data.GetStudents());
            MainManagement = new MainView();
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
                    factoryMenu.GetMenu("MAIN").ViewMenu();
                    int mainOpt = Utilities.Console.AskInt("Enter option: ");

                    switch (mainOpt)
                    {
                        case 1:
                            MainManagement.ListRooms(Data.GetRooms());
                            MainMenu();
                            break;
                        case 2:
                            MainManagement.ListSlots(Data.GetSlots());
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
                factoryMenu.GetMenu("STAFF").ViewMenu();
                int staffOpt = Utilities.Console.AskInt("Enter option: ");

                switch (staffOpt)
                {
                    case 1:
                        StaffManagement.ListStaffs();
                        StaffMenu(); 
                        break;
                    case 2:
                        StaffManagement.ListRoomAvailability(Data.GetRooms(),Data.GetSlots());
                        StaffMenu(); 
                        break;
                    case 3:
                        StaffManagement.CreateSlot(Data.GetSlots(),Data.GetRooms());
                        Data.RefreshDatabase();
                        StaffMenu();
                        break;
                    case 4:
                        StaffManagement.RemoveSlot(Data.GetSlots(), Data.GetRooms());
                        Data.RefreshDatabase();
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
                factoryMenu.GetMenu("STUDENT").ViewMenu();
                int studentOpt = Utilities.Console.AskInt("Enter option: ");

                switch (studentOpt)
                {
                    case 1:
                        StudentManagement.ListStudents();
                        StudentMenu(); 
                        break;
                    case 2:
                        StaffManagement.StaffAvailability(Data.GetSlots());
                        StudentMenu(); 
                        break;
                    case 3:
                        StudentManagement.MakeBooking(Data.GetSlots(),Data.GetRooms());
                        Data.RefreshDatabase();
                        StudentMenu();
                        break;
                    case 4:
                        StudentManagement.CancelBooking(Data.GetSlots());
                        Data.RefreshDatabase();
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
