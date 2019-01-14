using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ASR_App;
using ASR_Model;

namespace Controller
{
    class StudentController
    {

        private DateTime slotDate;
        private DateTime slotTime;

        // Student make booking from available slot
        public Boolean MakeBooking(List<Student> students, List<Slot> slots)
        {
            bool success = false;
            Console.WriteLine("\n--- Make booking ---\n");
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

                var studentID = Util.Console.Ask("Enter student ID: ");

                // Check whether student id is valid
                if(students.Where(x => x.UserID == studentID).Any())
                {
                    if (slots.Count != 0)
                    {
                        if(slots.Where(x => x.StudentBookingID == studentID && x.SlotDatetime == slotDate).Any())
                        {
                            Console.WriteLine("\nStudent can only book 1 slot per day. Choose another day.");
                        }
                        else
                        {
                            if (slots.Where(x => x.RoomName == slotRoom && x.SlotDatetime == slotDate && x.StartTime == slotTime.ToShortTimeString() && x.StudentBookingID == "-").Any())
                            {
                                using (var connection = ASRDatabase.ConnectionString.CreateConnection())
                                {
                                    connection.Open();

                                    var command = connection.CreateCommand();
                                    command.CommandText = "update Slot set BookedInStudentID = @StudentID where RoomID = @RoomID AND StartTime = @StartTime";
                                    command.Parameters.AddWithValue("StudentID", studentID);
                                    command.Parameters.AddWithValue("RoomID", slotRoom.ToUpper());
                                    DateTime startTime = DateTime.Parse(slotDate.ToShortDateString() + " " + slotTime.ToShortTimeString());
                                    command.Parameters.AddWithValue("StartTime", startTime);

                                    command.ExecuteNonQuery();
                                    success = true;
                                }

                                Console.WriteLine("\nSlot has been booked successfully.");
                            }
                            else
                            {
                                Console.WriteLine("\nSlot schedule not exist or already booked. Choose another slot.");
                            }
                        }                     
                    }
                    else
                    {
                        Console.WriteLine("\nNo slot available now.");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid student id.");
                }

            }
            catch(FormatException err)
            {
                Console.WriteLine(err.Message);
            }

            return success;
        }

        // Student cancel booking
        public Boolean CancelBooking(List<Student> students, List<Slot> slots)
        {
            bool success = false;
            Console.WriteLine("\n--- Cancel booking ---\n");
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

                var studentID = Util.Console.Ask("Enter student ID: ");

                // Check student id is valid or not
                if (students.Where(x => x.UserID == studentID).Any())
                {
                    if (slots.Where(x => x.RoomName == slotRoom && x.SlotDatetime == slotDate && x.StartTime == slotTime.ToShortTimeString() && x.StudentBookingID == studentID).Any())
                    {
                        using (var connection = ASRDatabase.ConnectionString.CreateConnection())
                        {
                            connection.Open();

                            var command = connection.CreateCommand();
                            command.CommandText = "update Slot set BookedInStudentID = null where RoomID=@RoomID AND StartTime = @StartTime";
                            command.Parameters.AddWithValue("RoomID", slotRoom.ToUpper());
                            DateTime startTime = DateTime.Parse(slotDate.ToShortDateString() + " " + slotTime.ToShortTimeString());
                            command.Parameters.AddWithValue("StartTime", startTime);

                            command.ExecuteNonQuery();
                            success = true;
                        }

                        Console.WriteLine("\nBooking slot has been cancelled.");
                    }
                    else
                    {
                        Console.WriteLine("\nSchedule not found. Check again the room, date and time slot.");
                    }
                }
                else
                {
                    Console.WriteLine("\nStudent id is invalid.");
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
