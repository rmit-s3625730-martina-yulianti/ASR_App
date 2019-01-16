using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utilities;
using ASR_Model;
using Console = System.Console;

namespace View
{
    /* StudentView class is the display the result from student input
     */ 

    internal class StudentView 
    {
        private DateTime slotDate;
        private DateTime slotTime;
        private string studentID;
        private List<Student> Students = new List<Student>();

        public StudentView(List<Student> students)
        {
            Students = students;
        }

        // List all students
        public void ListStudents()
        {
            if (Students.Count == 0)
            {
                Console.WriteLine("\nNo Student available now.");
            }
            else
            {
                Console.WriteLine("--- List Students ---");
                Console.WriteLine("\n    ID \t\t\t Name                  " + $"\t Email");
                foreach (Student student in Students)
                    Console.WriteLine(student.ToString());

                Console.WriteLine("----------------------------------------------------------------------------");
            }

        } // End of ListStudent()

        // Student make booking from available slot
        public bool MakeBooking(List<Slot> slots, List<Room> rooms)
        {
            bool success = false;
            Console.WriteLine("\n--- Make booking ---\n");
            var slotRoom = "";
            try
            {
                // Room validation from user input
                var checkRoom = false;
                while (!checkRoom)
                {
                    slotRoom = Utilities.Console.AskChar("Enter room name: ");
                    if (!rooms.Where(x => x.RoomName == slotRoom.ToUpper()).Any())
                    {
                        Console.WriteLine("Room name doesn't exist in database. Try another");
                    }
                    else { checkRoom = true; }
                }
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

                // Checking time input from user
                var checkTime = false;
                while (!checkTime)
                {
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter time for slot (hh:mm): "), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else
                    {
                        if (slotTime.Minute != 00)
                        {
                            Console.WriteLine("\nBooking time's minute only allowed 00, e.g 10:00");
                        }
                        else { checkTime = true; }
                    }
                }

                // Check validation student id from user input
                var checkStudent = false;
                while (!checkStudent)
                {
                    studentID = Utilities.Console.Ask("Enter student ID: ");
                    if (StudentValidationInput(studentID))
                    {
                        checkStudent = true;
                    }
                }

                // Check if students list not empty
                if (Students.Count != 0)
                {
                    // Check whether student id is valid
                    if (Students.Where(x => x.UserID == studentID).Any())
                    {
                        if (slots.Count != 0)
                        {
                            // Check whether the student already make booking in the same date
                            if (slots.Where(x => x.StudentBookingID == studentID && x.SlotDatetime == slotDate).Any())
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
                            Console.WriteLine("\nNo slot available now. Cannot make booking slot.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid student id.");
                    }
                }
                else { Console.WriteLine("\nNo students list in system database."); }               
            }
            catch(FormatException err)
            {
                Console.WriteLine(err.Message);
            }
            return success;
        }

        // Student cancel booking
        public bool CancelBooking(List<Slot> slots)
        {
            bool success = false;
            Console.WriteLine("\n--- Cancel booking ---\n");
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
                    else { checkDate = true;}
                }

                // Checking time input from user
                var checkTime = false;
                while (!checkTime)
                {
                    if (!(DateTime.TryParseExact(Utilities.Console.Ask("Enter time for slot (hh:mm): "), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out slotTime)))
                    {
                        Console.WriteLine("\nTime format incorrect. Try again (hh:mm).");
                    }
                    else
                    {
                        if (slotTime.Minute != 00)
                        {
                            Console.WriteLine("The minute must be 00, e.g 10:00 or 13:00");
                        }
                        else { checkTime = true; }
                        
                    }
                }

                // Check validation student id from user input
                var checkStudent = false;
                while (!checkStudent)
                {
                    studentID = Utilities.Console.Ask("Enter student ID: ");
                    if (StudentValidationInput(studentID))
                    {
                        checkStudent = true;
                    }
                }

                // Check if students list not empty
                if (Students.Count != 0)
                {
                    // Check student id is valid or not
                    if (Students.Where(x => x.UserID == studentID).Any())
                    {
                        // check if slots list is empty
                        if (slots.Count != 0)
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
                        else { Console.WriteLine("\nNo slot available now. Cannot cancel booking slot."); }       
                    }
                    else
                    {
                        Console.WriteLine("\nStudent id is invalid.");
                    }
                }
                else { Console.WriteLine("\nNo students list in system database."); }
            }
            catch (FormatException err)
            {
                Console.WriteLine(err.Message);
            }
            return success;
        }


        // Student validation (start with "s" and followed by 7 numbers)
        private bool StudentValidationInput(string studentId)
        {
            var valid = false;
            if (studentId.StartsWith("s"))
            {
                // remove extra space from user input
                string StudentId = studentId.Replace(" ", String.Empty);
                // check if it's followed by 7 numbers
                if (int.TryParse(StudentId.Substring(1, StudentId.Length - 1), out int stdInt))
                {
                    if (StudentId.Substring(1, StudentId.Length - 1).Length == 7)
                    {
                        valid = true;
                    }
                    else { Console.WriteLine("The numbers'length must be 7.\n"); }
                }
                else { Console.WriteLine("Should be followed by 7 numbers.\n"); }
            }
            else { Console.WriteLine("Student id should start with 's'.\n"); }

            return valid;
        } // End of Student validation

    } // End of Student Controller
}
