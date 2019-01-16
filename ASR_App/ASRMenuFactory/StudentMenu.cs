using System;

namespace ASRMenuFactory
{   
    /* Student menu implement interface IMenu to dispaly student menu
    */
    class StudentMenu : IMenu
    {
        public void ViewMenu()
        {
            Console.WriteLine(@"
 -------------------------------------------------------------
                     Student Menu
 ------------------------------------------------------------- 
    Main Menu: 
    1. List student
    2. Staff availability
    3. Make booking
    4. Cancel booking
    5. Exit
 -------------------------------------------------------------");
   
        }
    }
}
