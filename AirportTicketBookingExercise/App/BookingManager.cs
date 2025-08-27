using ATB.App.Handlers;
using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.App
{
    public class BookingManager
    {
        private PassengerCommandHandler _passengerCommandHandler;
        private ManagerCommandHandler _managerCommandHandler;
        private User? loggedInUser;

        public BookingManager(ManagerCommandHandler managerCommndHandler, PassengerCommandHandler passengerCommandHandler)
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
            if (loggedInUser == null)
            {
                switch (command)
                {
                    case ManagerCommand.ManagerSignUp:
                        if (productInfo.Length < 3)
                        {
                            Console.WriteLine("Please enter a username and password");
                            break;
                        }
                        bool isSignUpSuccessful = _managerCommandHandler.ManagerSignUp(productInfo[1], productInfo[2]);
                        if (isSignUpSuccessful) Console.WriteLine("You have signed up successfully, manager");
                        else Console.WriteLine("Username may be taken or you have not entered a valid username and password");
                        break;
                    case ManagerCommand.ManagerLogIn:
                        if (productInfo.Length < 3)
                        {
                            Console.WriteLine("Please enter your username and password to login, manager!");
                            break;
                        }
                        User? loggingInUser = _managerCommandHandler.ManagerLogIn(productInfo[1], productInfo[2]);
                        if (loggingInUser == null)
                            Console.WriteLine("Incorrect username or password, please try again");
                        else
                        {
                            loggedInUser = loggingInUser;
                            Console.WriteLine($"Welcome back, {loggedInUser.Name}!");
                        }
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                    default:
                        Console.WriteLine("Please enter an appropriate command.");
                        break;
                }
            }
            else if (loggedInUser.UserType == UserType.Passenger)
                Console.WriteLine("Only managers can use these commands!");
            else
            {
                switch (command)
                {
                    case ManagerCommand.ManagerLogOut:
                        loggedInUser = null;
                        Console.WriteLine("Logged out successfully! See you soon, manager!");
                        break;
                    case ManagerCommand.Upload:
                        if (productInfo.Length < 2)
                        {
                            Console.WriteLine("Please enter a valid CSV file path");
                            break;
                        }
                        bool isUploadSuccess = _managerCommandHandler.Upload(productInfo[1]);
                        if (!isUploadSuccess)
                            Console.WriteLine("The CSV is not in the correct format. " +
                            "Please use the 'Validate' command to check which fields must be changed.");
                        else
                            Console.WriteLine("Flight Data has been imported successfully");
                        break;
                    case ManagerCommand.Validate:
                        Console.WriteLine($"\n Validating... \n");
                        string errorStr = _managerCommandHandler.Validate(productInfo[1]);
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
                    case ManagerCommand.ManagerSignUp:
                        Console.WriteLine("Please log out first to sign up");
                        break;
                    case ManagerCommand.ManagerLogIn:
                        Console.WriteLine($"You are already logged in, {loggedInUser}!");
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                }
            }   
        }
        public void ExecutePassengerCommand(string[] productInfo, PassengerCommand command)
        {
            if (loggedInUser == null)
            {
                switch (command)
                {
                    case PassengerCommand.SignUp:
                        if (productInfo.Length < 3)
                        {
                            Console.WriteLine("Please enter a username and password to signup, passenger");
                            break;
                        }
                        bool isSuccessful = _passengerCommandHandler.PassengerSignUp(productInfo[1], productInfo[2]);
                        if (!isSuccessful)
                            Console.WriteLine("\nPlease make sure the username is not taken and that" +
                                    " you have placed your username and password");
                        else
                            Console.WriteLine("\nNew Passenger has been created");
                        break;
                    case PassengerCommand.LogIn:
                        if (productInfo.Length < 3)
                        {
                            Console.WriteLine("Please enter your username and password to log in");
                            break;
                        }
                        User? loggingInUser = _passengerCommandHandler.PassengerLogIn(productInfo[1], productInfo[2]);
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
            else if (loggedInUser.UserType == UserType.Passenger)
                Console.WriteLine("Only passengers can use these commands!");
            else
            {
                switch (command)
                {
                    case PassengerCommand.LogOut:
                        loggedInUser = null;
                        Console.WriteLine("Logged out successfully. We hope to see you soon!");
                        break;
                    case PassengerCommand.Book:
                        try
                        {
                            int flightId = int.Parse(productInfo[1]);
                            BookingClass bookingClass = productInfo[2].ParseBookingClass();
                            bool isBookingSuccessful = _passengerCommandHandler.PassengerBookFlight(flightId, bookingClass, loggedInUser);
                            if (!isBookingSuccessful)
                                Console.WriteLine("Please make sure the flight and flight class is available when booking");
                            else
                                Console.WriteLine("Booking successful! Please enjoy your flight");
                        }
                        catch (FormatException) { Console.WriteLine("Please enter the id of the flight you want to book and the class (first, economy, business)"); }
                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                        break;
                    case PassengerCommand.Search:
                        List<Flight> searchFlights = _passengerCommandHandler.Search(productInfo);
                        Console.WriteLine(_passengerCommandHandler.FlightsToString(searchFlights));
                        break;
                    case PassengerCommand.Cancel:
                        try
                        {
                            int bookingId = int.Parse(productInfo[1]);
                            bool isCancelSuccessful = _passengerCommandHandler.Cancel(bookingId, loggedInUser);
                            if (!isCancelSuccessful) Console.WriteLine("Please make sure to cancel with the booking id");
                            else Console.WriteLine("Booking has been successfully cancelled");
                        }
                        catch (FormatException) { Console.WriteLine("Please enter the id of the booking you wish to cancel"); }
                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                        break;
                    case PassengerCommand.Modify:
                        try
                        {
                            int bookingId = int.Parse(productInfo[1]);
                            BookingClass bookingClass = productInfo[2].ParseBookingClass();
                            bool isModifySuccessful = _passengerCommandHandler.Modify(bookingId, bookingClass, loggedInUser);
                            if (!isModifySuccessful) Console.WriteLine("Please make sure the booking exists and please enter the class");
                            else Console.WriteLine("Booking changed successfully!");
                        }
                        catch (IndexOutOfRangeException) { Console.WriteLine("Please enter the id of the booking you wish to modify, and the and the class (first, economy, business)"); }
                        catch (FormatException) { Console.WriteLine("Please enter the booking id and class in the correct format"); }
                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                        break;
                    case PassengerCommand.Flights:
                        List<Flight> getFlights = _passengerCommandHandler.Flights();
                        if (!getFlights.Any()) Console.WriteLine("There are currently no flights");
                        else Console.WriteLine(_passengerCommandHandler.FlightsToString(getFlights));
                        break;
                    case PassengerCommand.Bookings:
                        List<Booking> bookings = _passengerCommandHandler.Bookings(loggedInUser);
                        if (!bookings.Any()) Console.WriteLine("You currently have not booked any flights yet!");
                        Console.WriteLine(_passengerCommandHandler.BookingsToString(bookings));
                        break;
                    case PassengerCommand.None:
                        Console.WriteLine("\nPlease enter an appropriate action.");
                        break;
                }
            }
        }
    }
}

