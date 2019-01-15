# Appointment Scheduling and Reservation System (ASR)
ASR system is a scheduling system to help student to make booking appointment based on the availability of the staff. The system has different functionality for each user. Staff can list the room availability before they can create a appointment schedule in the system and also remove the schedule if the schedule is not booked by a student. The student can check the availability of the staff on a specific day, when and where they can booked the appointment with them, or cancel the booking.

## Getting Started
When ASR application is start, it will create connection to database server (Microsoft Azure server) to get the data before the application creates user (student and staff) object and slot object (if available). If the database server is unavailable when the ASR application run, it will run on offline mode. The menu for student and staff is still available, but cannot perform the functionalities properly, such as cannot create or remove the appointment schedule.  

## Coding Pattern
1. Singleton pattern


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
  


