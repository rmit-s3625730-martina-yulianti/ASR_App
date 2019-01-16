README

Web Development Technology: Assignment 1
Due date : 16 January 2019


Author:
Student name   : Martina Yulianti

Student number : s3625730

Coding Pattern Used:
 
-1. Singleton pattern
- I used singleton pattern for ASR_DataController class to make sure only one ASR_DataController that run when this class is called by client class. ASR_DataController is a class that create connection to database directly, therefore to make sure that only one instance that was created when the application is running, it is implemented singleton pattern. Because this ASR application is connection based application, therefore it is good to implement singleton pattern in database connection, hence the application does not consume a lot of memory resource.
   
-2. MVC pattern
  - Model View Controller pattern is an architecural pattern which seperate the logic and bussines rules. The logic of this application will be controlled by controller (ASRController class) to run the functionality for each user: Staff and Student. While the bussines rules of this application can be display to user by using query filter from the list of Slot, User(Staff and Student) and Room. ASR is console based application, therefore it is easier to seperate the view for each Main menu, Staff and Student menu based on their functionalities. Also it is open for code expansion in the furture and each class will not depend to each other (low coupling).  Since this application using data from database: User, Slot and Room table, these three datatable can be implemented as the model directly. However, in this application we still create a user class: Staff and Student, Slot and Room class in the application as container to filter data from database, and application do not need to check the database in the server. Especially when there is no internet connection the ASR application still can run in offline properly.
   It is a lot more faster to build the features for each entity since there are not depended to other features from another entity. And also, with MVC pattern we can create multiple view because with this pattern the controller for the application will be the same. MVC allowed us to build the application one at the time, and do unit test per each feature without disturbing other part. Each feature does need to complete synchoronously. The other thing, We can start build the application from model firt or the view, or the logic first.   
 
 -3. Factory design 
  - Console base application, especially in ASR application usually use menu base to interact with the user. There are 3 different type menu display in ASR application, Main, Staff and Student. By implementing factory design, it is easy for controller to call which menu that has to be display just by calling the menu name.  

Acknowlegment

The creation of connection to database server is using an example from tute lab week 4: InventoryPriceManagement
for education purposes only.