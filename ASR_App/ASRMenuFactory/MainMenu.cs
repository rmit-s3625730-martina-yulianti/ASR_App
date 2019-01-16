using System;

namespace ASRMenuFactory
{
    public class MainMenu : IMenu
    {
        public void ViewMenu()
        {
            Console.WriteLine(@"
 -------------------------------------------------------------
 Welcome to Appointment Scheduling and Reservation System 
 ------------------------------------------------------------- 
    Main Menu: 
    1. List rooms
    2. List slots
    3. Staff menu
    4. Student menu
    5. Exit
 -------------------------------------------------------------");
           
        }
    }
}
