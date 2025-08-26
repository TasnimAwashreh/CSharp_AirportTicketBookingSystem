using ATB.Logic.Handlers.Command;
using ATB.Logic.Enums;
using ATB.Logic.Handlers;
using ATB.Data.Models;
using ATB.Logic.Service;

namespace ATB.App
{
    public class BookingManager
    {

        private PassengerCommandHandler _passengerCommandHandler;
        private ManagerCommandHandler _managerCommandHandler;
        private User? loggedInUser;

        public BookingManager(
            ManagerCommandHandler managerCommndHandler, PassengerCommandHandler passengerCommandHandler)
        {
            _passengerCommandHandler = passengerCommandHandler;
            _managerCommandHandler = managerCommndHandler;
        }

        public void ProcessInput(string input)
        {
            string[] line = input.Split(' ');
            ManagerCommand managerCommand = line[0].ParseManagerCommand();
            PassengerCommand passengerCommand = line[0].ParsePassengerCommand();
            if (passengerCommand != PassengerCommand.None)
                ExecutePassengerCommand(line, passengerCommand);
            else if (managerCommand != ManagerCommand.None)
                executeManagerCommand(line, managerCommand);
            else Console.WriteLine("\n Please enter an appropriate action");
        }

        public void executeManagerCommand(string[] productInfo, ManagerCommand command)
        {
            if (loggedInUser != null)
            {
                switch (command)
                {
                    case ManagerCommand.ManagerLogOut:
                        if (loggedInUser == null || loggedInUser.UserType != UserType.Manager)
                            Console.WriteLine("You did not log in as a manager to log out");
                        else
                        {
                            loggedInUser = null;
                            Console.WriteLine("Logged out successfully! See you soon, manager!");
                        }
                        break;
                    case ManagerCommand.Upload:
                        bool isUploadSuccess = _managerCommandHandler.Upload();
                        if (isUploadSuccess) Console.WriteLine("Flight Data has been imported successfully");
                        else Console.WriteLine("The CSV is not in the correct format. " +
                            "Please use the 'Validate' command to check which fields must be changed.");
                        break;
                    case ManagerCommand.Validate:
                        Console.WriteLine($"\n Validating... \n");
                        string errorStr = _managerCommandHandler.Validate();
                        if (errorStr != "")
                            Console.WriteLine(errorStr);
                        else Console.WriteLine("No fields are in need of fixing!");
                        break;
                    case ManagerCommand.Filter:
                        Console.WriteLine($"\n Filter: \n");
                        List<Booking> bookingResults = _managerCommandHandler.Filter(productInfo);
                        if (bookingResults.Count > 0)
                        {
                            Console.WriteLine("\nFilter Results: \n");
                            foreach (Booking Booking in bookingResults)
                            {
                                Console.WriteLine(Booking.ToString() + $" Booked by passenger with Id ({Booking.PassengerId})");
                            }
                        }
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case ManagerCommand.ManagerSignUp:
                        bool isSignUpSuccessful = _managerCommandHandler.ManagerSignUp(productInfo);
                        if (isSignUpSuccessful) Console.WriteLine("You have signed up successfully, manager");
                        else Console.WriteLine("Username may be taken or you have not entered a valid username and password");
                        break;
                    case ManagerCommand.ManagerLogIn:
                        User? loggingInUser = _managerCommandHandler.ManagerLogIn(productInfo, loggedInUser);
                        if (loggingInUser != null)
                        {
                            loggedInUser = loggingInUser;
                            Console.WriteLine($"Welcome back, {loggedInUser.Name}!");
                        }
                        else Console.WriteLine("Please sign up as a manager first before trying to log in, then enter your username and password");
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                    default:
                        Console.WriteLine("You must log in to use this command");
                        break;
                }
            }
        }
        public void ExecutePassengerCommand(string[] productInfo, PassengerCommand command)
        {
            if (loggedInUser != null)
            {
                switch (command)
                {
                    case PassengerCommand.LogOut:
                        bool isSLogOutSuccessful = _passengerCommandHandler.PassengerSignOut(loggedInUser);
                        if (!isSLogOutSuccessful)
                            Console.WriteLine("You must be a passenger that is currently logged in to log out");
                        else Console.WriteLine("Logged out successfuly. We hope to see you again!");
                        break;
                    case PassengerCommand.Book:
                        bool isBookingSuccessful = _passengerCommandHandler.PassengerBookFlight(productInfo, loggedInUser);
                        if (!isBookingSuccessful)
                            Console.WriteLine("Please make sure the flight and flight class is available when booking");
                        else
                            Console.WriteLine("Booking successful! Please enjoy your flight");
                        break;
                    case PassengerCommand.Search:
                        List<Flight> searchFlights = _passengerCommandHandler.Search(productInfo);
                        Console.WriteLine(_passengerCommandHandler.FlightsToString(searchFlights));
                        break;
                    case PassengerCommand.Cancel:
                        bool isCancelSuccessful = _passengerCommandHandler.Cancel(productInfo, loggedInUser);
                        if (!isCancelSuccessful)
                            Console.WriteLine("Please make sure to cancel with the booking id");
                        else
                            Console.WriteLine("Booking has been successfully cancelled");
                        break;
                    case PassengerCommand.Modify:
                        bool isModifySuccessful = _passengerCommandHandler.Modify(productInfo, loggedInUser);
                        if (!isModifySuccessful)
                            Console.WriteLine("Please make sure the booking exists and please enter the class");
                        else
                            Console.WriteLine("Booking changed successfully!");
                        break;
                    case PassengerCommand.Flights:
                        List<Flight> getFlights = _passengerCommandHandler.Flights();
                        if (!getFlights.Any()) Console.WriteLine("There are currently no flights");
                        else Console.WriteLine(_passengerCommandHandler.FlightsToString(getFlights));
                        break;
                    case PassengerCommand.Bookings:
                        List<Booking> bookings =  _passengerCommandHandler.Bookings(loggedInUser);
                        if (!bookings.Any()) Console.WriteLine("You currently have not booked any flights yet!");
                        Console.WriteLine(_passengerCommandHandler.BookingsToString(bookings));
                        break;
                    case PassengerCommand.None:
                        Console.WriteLine("\nPlease enter an appropriate action.");
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case PassengerCommand.SignUp:
                        bool isSuccessful = _passengerCommandHandler.PassengerSignUp(productInfo);
                        if (isSuccessful)
                            Console.WriteLine("\nNew Passenger has been created");
                        else
                            Console.WriteLine("\nPlease make sure the username is not taken and that" +
                                " you have placed your username and password");
                        break;
                    case PassengerCommand.LogIn:
                        User? loggingInUser = _passengerCommandHandler.PassengerLogIn(productInfo, loggedInUser);
                        if (loggingInUser == null)
                            Console.WriteLine("\nPlease make sure that you signed up as a passenger first");
                        else
                        {
                            loggedInUser = loggingInUser;
                            Console.WriteLine($"\nWelcome back, {loggedInUser.Name}");
                        }
                        break;
                    case PassengerCommand.None:
                        Console.WriteLine("\nPassenger, please enter an appropriate action.");
                        break;
                    default:
                        Console.WriteLine("\nYou must log in to use this command.");
                        break;
                }
            }
        }

    }

}

