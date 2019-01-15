# Appointment Scheduling and Reservation System (ASR)
ASR system is a scheduling system to help student to make booking appointment based on the availability of the staff. The system has different functionality for each user. Staff can list the room availability before they can create a appointment schedule in the system and also remove the schedule if the schedule is not booked by a student. The student can check the availability of the staff on a specific day, when and where they can booked the appointment with them, or cancel the booking.

## Getting Started
When ASR application is start, it will create connection to database server (Microsoft Azure server) to get the data before the application creates user (student and staff) object and slot object (if available). If the database server is unavailable when the ASR application run, it will run on offline mode. The menu for student and staff is still available, but cannot perform the functionalities properly, such as cannot create or remove the appointment schedule.  

## Coding Pattern
1. Singleton pattern
   I used singleton pattern for ASRController class to make sure only one ASRController that run when this class is called by client class. The singleton pattern is not implemented in ASRDatabase class because ASRDatabase is a static class and used by class controllers to create connection to database. Therefore, by implemented singleton pattern in ASRController class, it also makes sure only one database connection that running, since only one instance ASRController that created.  
   
2. Facade pattern
   This ASR application using facade pattern, to split the functionalities between Staff and Student which is controlled by StaffController class and StudentController class. However to run the application with full functionality, it is controlled by ASRController. The User only interact with ASR_App class which is totally does not have any logic functionality to proccess to user input (the view).
   The reason I used this pattern, because the ASR system is using database to store the users, rooms and slots information. All the logic of the application is dealing with manipulating of Slot table which related with User and Room tables. Therefore, I created controller classes for handling student and staff functionalities, then the main controller maintains the schedules in Slot table by calling the methods from Staff and student controller. By seperating the functionality of staff and student and main program, it is easy to maintain the functionality for staff and student. It also makes the class more simple by creating the method for each functionality and passes the collections of slot list. Using facade pattern in this application reduces the complexity of handling with many collections of list because each class handle with one or two collection lists. Each class does not need handle with many methods, keep the code simple and not too long. Each controller class does not need to know all the processes of the application, the main controller which is as wrapper class will hide the implemention detail. The client class only deal with one driver class.
   
## Dependecies (3rd party plug-in)
This ASR Application using 3rd plug-in to perform the functionalities:
1. System.Data.SqlClient
2. System.Linq
3. Microsoft.Extensions.Configuration.Json
4. System.Globalization

## Acknowlegment
The creation of connection to database server is using an example from tute lab week 4: InventoryPriceManagement

### Author
- Student name: Martina Yulianti
  Student number : s3625730
  


