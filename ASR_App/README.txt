README

Web Development Technology: Assignment 1
Due date : 16 January 2019



Author:
Student name   : Martina Yulianti

Student number : s3625730

Coding Pattern
 Used:
 
1. Singleton pattern
- I used singleton pattern for ASR_DATAController class to make sure only one ASR_DataController that run when this class is called by client class. The singleton pattern is not implemented in ASRDatabase class because ASRDatabase is a static class and used by class controllers to create connection to database. Therefore, by implemented singleton pattern in ASRController class, it also makes sure only one database connection that running, since only one instance ASRController that created.  
   
2. MVC pattern
 - In ASR application, each entity is related to each other. For example slot has to keep tracking how many slot staff can be created per day; the maximum slot that can be created 2 slots per room; the student only can booking if the staff available and other bussiness rules. If each we implement each functionality of Staff, Student, Room and Slot in they own class then it will increase high coupling in the application. Therefore, to reduce the coupling each class, I created seperate class for the staff and student functionality. This class is like controller for each entity. Staff controller handles the functionality for staff object and student controller handles the functionality of student object. The main controller of the ASR application will keep tracking the slot object, which is always been used either by staff or student controller. In order to split the controller for staff and student entity, I used facade pattern. To make sure each controller work together, it needs main controller that make used the functionality of staff and student controller, so the application can run as expected. The User only interact with ASR_App class which is totally does not have any logic functionality to proccess user input (only the view). 
   By seperating the functionality of staff and student and main program, it is easy to maintain the functionality for staff and student. It also makes the class more simple by creating the method for each functionality and passes the collections of slot list. Using facade pattern in this application reduces the complexity (low coupling) of handling with many collections of list because each class handle with one or two collection lists. Each class does not need handle with many methods, keep the code simple and not too long. Each controller class does not need to know all the processes of the application (encapsulation concept), the main controller which is a wrapper class will hide the implemention detail. The client class only deal with one driver class, which is the main controller but not with staff controller and student controller classes.

3. Factory pattern
   By using this pattern is hiding hardcoded menu display in controller class. Controller only call the method to create menu for each functionality in ASR application

Acknowlegment


The creation of connection to database server is using an example from tute lab week 4: InventoryPriceManagement
for education purposes only.