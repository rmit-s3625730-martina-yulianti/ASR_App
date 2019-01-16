using System;

namespace ASRMenuFactory
{   
    /* Staff menu implement interface IMenu to dispaly staff menu
    */

    class StaffMenu : IMenu
    {
        public void ViewMenu()
        {
            Console.WriteLine(@"
 -------------------------------------------------------------
                    Staff Menu
 ------------------------------------------------------------- 
    Main Menu: 
    1. List staff
    2. List staff
    3. Create slot
    4. Remove slot
    5. Exit
 -------------------------------------------------------------");
 
        }
    }
}
