using System;

namespace ASRMenuFactory
{   
    /* Concreat class Menu Factory which extend abstract class menu factory
     * This class implement logic for calling type of menu from GetMenu method
    */
    class MenuFactory : AbstractMenuFactory
    {
        public override IMenu GetMenu(string menuName)
        {
            if (menuName == null) { return null; }

            if (menuName == "MAIN")
            {
                return new MainMenu();
            }
            else if (menuName == "STAFF")
            {
                return new StaffMenu();
            }
            else if (menuName == "STUDENT")
            {
                return new StudentMenu();
            }
            else
            {
                return null;
            }
        }
    }
}
