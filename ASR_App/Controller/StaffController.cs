using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utilities;
using ASR_Model;
using Console = System.Console;

namespace Controller
{
    /* StaffController class is the controller for the staff functionalities
     */

    internal class StaffController
    {
        private DateTime slotDate;
        private DateTime slotTime;
        private string staffID;
        private const int ROOM_SLOTS = 2;    // Each room can be booked for a maximum of 2 slots per day
        private const int STAFF_SLOTS = 4;   // A staff can book a maximum of 4 slots per day 

        // Staff create slot 
        public bool CreateSlot(List<Staff> staffs, List<Slot> slots)
        {
            var success = false;
            Console.WriteLine("\n--- Create slot ---\n");
            var slotRoom="";
            try
            {
                slotRoom = Utilities.Console.AskChar("Enter room name: ");
                // Checking date input from user   
                var checkDate = false;
                while (!checkDate)
                {
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter date for slot (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
                    {
                        Console.WriteLine("\nDate format incorrect. Try again (dd-mm-yyyy).");
                    }
                    else {checkDate = true;}
                }

                // Checking time input from user, booked time start from 9AM to 2PM
                var checkTime = false;
                while (!checkTime)
                {
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter time for slot (hh:mm) in 24 hours format: "),"HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else
                    {
                        // if the time format is correct then check validation of time
                        if (slotTime.Hour >= 9 && slotTime.Hour <= 14)
                        {
                            if (slotTime.Minute == 00)
                            {
                                checkTime = true;
                            }
                            else { Console.WriteLine("\nBooking time's minute only allowed 00, e.g 10:00"); }

                        }
                        else { Console.WriteLine("\nThe slot must be booked between school working hours of 9am to 2pm"); }                    
                    }
                }

                // Check validation staff id from user input
                var checkStaff = false;         
                while (!checkStaff)
                {
                    staffID = Utilities.Console.Ask("Enter staff ID: ");
                    if(StaffValidationInput(staffID))
                    {
                        checkStaff = true;
                    }
                }

                // Check whether staffId valid in database
                if (staffs.Where(x => x.UserID == staffID).Any())
                {
                    // Check how many slots that staff has created in that date
                    if(slots.Where(x => x.SlotDatetime == slotDate && x.StaffID == staffID).Count() >= STAFF_SLOTS)
                    {
                        Console.WriteLine("\nUnable to create slot. Maximum staff's slot is 4 slots per day, choose another day.");
                    }
                    else 
                    {
                        // Check how many slots that room has been booked in that date
                        if (slots.Where(x => x.SlotDatetime == slotDate && x.RoomName == slotRoom).Count() >= ROOM_SLOTS)
                        {
                            Console.WriteLine("\nUnable to create slot. Maximum room's slot is 2 per day, choose another day.");
                        }
                        else
                        {
                            // Check if the staff has been created the same booking time, but different room
                            if (slots.Where(x => x.SlotDatetime == slotDate && x.StartTime==slotTime.ToShortTimeString()).Any())
                            {
                                Console.WriteLine("\nUnable to create slot. Duplicate schedule, choose different time.");
                            }
                            else
                            {
                                // Valid schedule datetime, then execute to database slot
                                using (var connection = ASRDatabase.ConnectionString.CreateConnection())
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
                slotRoom = Utilities.Console.AskChar("Enter room name: ");
                // Checking date input from user   
                var checkDate = false;
                while (!checkDate)
                {
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter date for slot (dd-mm-yyyy): "), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotDate)))
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
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter time for slot (hh:mm): "), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else {checkTime = true;}
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

        } // End of Remove slot

        // Staff validation (start with "e" and followed by 5 numbers)
        private bool StaffValidationInput(string staffId)
        {
            var valid = false;
            if (staffId.StartsWith("e"))
            {
                // remove extra space from user input
                string StaffId = staffId.Replace(" ", String.Empty);
                // check if it's followed by 5 numbers
                if (int.TryParse(StaffId.Substring(1, StaffId.Length - 1), out int stdInt))
                {
                    if (StaffId.Substring(1, StaffId.Length - 1).Length == 5)
                    {
                        valid = true;
                    }
                    else { Console.WriteLine("The numbers'length must be 5.\n"); }
                }
                else { Console.WriteLine("Should be followed by 5 numbers.\n"); }
            }
            else { Console.WriteLine("Staff id should start with 'e'.\n"); }

            return valid;
        } // End of Staff validation

    } // End of class controller
}
