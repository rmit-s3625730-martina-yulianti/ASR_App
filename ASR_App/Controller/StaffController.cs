using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ASR_App;
using ASR_Model;

namespace Controller
{
    class StaffController
    {
        private DateTime slotDate;
        private DateTime slotTime;
        private const int ROOM_SLOTS = 2;
        private const int STAFF_SLOTS = 4;

        // Staff create slot 
        public bool CreateSlot(List<Staff> staffs, List<Slot> slots)
        {
            var success = false;
            Console.WriteLine("\n--- Create slot ---\n");

            var slotRoom="";
            try
            {
                slotRoom = Util.Console.AskChar("Enter room name: ");
                // Checking date input from user   
                var checkDate = false;
                while (!checkDate)
                {
                    if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for slot (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                    {
                        Console.WriteLine("\nDate format incorrect. Try again (dd-mm-yyyy).");
                    }
                    else
                    {
                        checkDate = true;
                    }
                }

                // Checking time input from user
                var checkTime = false;
                while (!checkTime)
                {
                    if (!(DateTime.TryParseExact(Util.Console.Ask("Enter time for slot (hh:mm): "),"HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else
                    {
                        checkTime = true;
                    }
                }
              
                var staffID = Util.Console.Ask("Enter staff ID: ");

                // Check whether staffId valid 
                if (staffs.Where(x => x.UserID == staffID).Any())
                {
                    // Check how many slot staff already create in that date
                    if(slots.Where(x => x.SlotDatetime == slotDate && x.StaffID == staffID).Count() >= STAFF_SLOTS)
                    {
                        Console.WriteLine("\nUnable to create slot. Maximum staff's slot is 4 slots per day, choose another day.");
                    }
                    else 
                    {
                        if (slots.Where(x => x.SlotDatetime == slotDate && x.RoomName == slotRoom).Count() >= ROOM_SLOTS)
                        {
                            Console.WriteLine("\nUnable to create slot. Maximum room's slot is 2 per day, choose another day.");
                        }
                        else
                        {

                            if (slots.Where(x => x.SlotDatetime == slotDate && x.RoomName == slotRoom && x.StartTime==slotTime.ToShortTimeString()).Any())
                            {
                                Console.WriteLine("\nUnable to create slot. Duplicate schedule, choose another time.");
                            }
                            else
                            {
                                using(var connection = ASRDatabase.ConnectionString.CreateConnection())
                                {
                                
                                    connection.Open();

                                    var command = connection.CreateCommand();
                                    command.CommandText = "insert into Slot (RoomID, StartTime,StaffID,BookedInStudentID) values (@RoomID, @StartTime, @StaffID, @BookedInStudentID)";
                                    command.Parameters.AddWithValue("RoomID", slotRoom.ToUpper());
                                    DateTime startTime = DateTime.Parse(slotDate.ToShortDateString() + " " + slotTime.ToShortTimeString());
                                    command.Parameters.AddWithValue("StartTime", startTime);
                                    command.Parameters.AddWithValue("StaffID", staffID);
                                    command.Parameters.AddWithValue("BookedInStudentID", DBNull.Value);

                                    command.ExecuteNonQuery();
                                    success = true;
                                }

                                Console.WriteLine("\nNew slot has been added to database.");
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("\nInvalid staff id.");
                }

            }
            catch(FormatException err)
            {
                Console.WriteLine(err.Message);
            }

            return success;
        } // End of CreateSlot

        // Remove the slot
        public bool RemoveSlot(List<Slot> slots)
        {
            bool success = false;

            Console.WriteLine("\n--- Remove slot ---\n");

            var slotRoom = "";
            try
            {
                slotRoom = Util.Console.AskChar("Enter room name: ");
                // Checking date input from user   
                var checkDate = false;
                while (!checkDate)
                {
                    if (!(DateTime.TryParseExact(Util.Console.Ask("Enter date for slot (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                    {
                        Console.WriteLine("\nDate format incorrect. Try again (dd-mm-yyyy).");
                    }
                    else
                    {
                        checkDate = true;
                    }
                }

                // Checking time input from user
                var checkTime = false;
                while (!checkTime)
                {
                    if (!(DateTime.TryParseExact(Util.Console.Ask("Enter time for slot (hh:mm): "), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else
                    {
                        checkTime = true;
                    }
                }

                if(slots.Where(x => x.RoomName == slotRoom && x.SlotDatetime == slotDate && x.StartTime == slotTime.ToShortTimeString() && x.StudentBookingID == "-").Any())
                {
                    using (var connection = ASRDatabase.ConnectionString.CreateConnection())
                    {

                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "delete from Slot where RoomID=@RoomID AND StartTime = @StartTime";
                        command.Parameters.AddWithValue("RoomID", slotRoom.ToUpper());
                        DateTime startTime = DateTime.Parse(slotDate.ToShortDateString() + " " + slotTime.ToShortTimeString());
                        command.Parameters.AddWithValue("StartTime", startTime);
                        
                        command.ExecuteNonQuery();
                        success = true;
                    }

                    Console.WriteLine("\nSlot has been removed from database.");
                }

            }
            catch (FormatException err)
            {
                Console.WriteLine(err.Message);
            }

            return success;
        }
    }
}
