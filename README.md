# fandueldemo
The demo has used below layered architecture.
1) Services - All the Business logic and interaction with the DB is placed in this folder.
2)  Controllers - Only used to interact with the User Input and return a valid HTTP response.
3)  Middleware - Created a base class to validate business rules and return valid error messages.
4)  Models - Defined four models in the application
      -  Sport
      -  Team
      -  Player
      -  Position
5)  ViewModels - Used this to Retrieve User inputs and display the required data as per the requirement.

Please note that swagger has been configured to test the application functionality. Also, ensure that Sport, Team and Players are added to the application before testing the actual functionality as per described in the exercise. 
