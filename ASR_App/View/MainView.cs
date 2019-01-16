using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using ASR_Model;
using Console = System.Console;

namespace View
{
    /* This class to view the functionality of main menu ASR
     */

    public class MainView
    {     
        private List<Slot> Slots = new List<Slot>();
        private DateTime slotDate;

        // List the rooms that available in the database
        public void ListRooms(List<Room> rooms)
        {
            Console.WriteLine("\n------List Room------");
            Console.WriteLine("\n      Room Name");
            if (rooms.Count > 0)
            {
                foreach (Room room in rooms)
                    Console.WriteLine($"      {room.RoomName}");
            }
            else
            {
                Console.WriteLine("\t Room has not been created yet.");
            }
            Console.WriteLine();

        } // End of ListRooms()

        // List the slots that available in the system
        public void ListSlots(List<Slot> slots)
        {
            Console.WriteLine("\n--- List Slots ---");

            var check = false;
            while (!check)
            {
                if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter date for slots (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                {
                    Console.WriteLine("Date format incorrect. Try again (dd-mm-yyyy).");
                }
                else {check = true;}
            }

            if (slots.Count==0)
            {
                Console.WriteLine("\nThere is no slot that available now.");
            }
            else
            {
                Console.WriteLine($"\nSlots on {slotDate.ToShortDateString()}");
                Console.WriteLine("\n    Room name \t Start time \t End time \t Staff ID \t Bookings");
                Console.WriteLine("--------------------------------------------------------------------------------");

                var slotQuery = slots.Where(x => x.SlotDatetime.Date == slotDate).ToList();
                if (slotQuery.Count==0)
                {
                    Console.WriteLine($"No slot available at {slotDate.ToShortDateString()}");
                }
                else
                {
                    foreach (Slot slot in slotQuery)
                        Console.WriteLine(slot);
                }
            }
        } // End of listSlots

    } // End of MainView

}
