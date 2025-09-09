using AirportTicketBookingExercise.App.Commands.Enums;
using AirportTicketBookingExercise.App.Commands.Helpers;
using ATB.Data.Models;
using ATB.Logic.Service;

namespace AirportTicketBookingExercise.App.Commands.CommandExecuter
{
    public class ExecutePassengerCommands : ICommandExecuter<PassengerCommand>
    {
        private readonly IFlightservice _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        private PassengerHelper _passengerHelper;

        public ExecutePassengerCommands(IFlightservice flightService, IUserService userService, IBookingService bookingService, PassengerHelper passengerHelper)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
            _passengerHelper = passengerHelper;
        }

        public User? ExecuteCommand(User? loggedInUser, string[] productInfo, PassengerCommand command)
        {
            if (loggedInUser == null)
            {
                switch (command)
                {
                    case PassengerCommand.SignUp:
                        _passengerHelper.SignUp(productInfo);
                        return loggedInUser;
                    case PassengerCommand.LogIn:
                        return _passengerHelper.Login(loggedInUser, productInfo);
                    case PassengerCommand.None:
                        Console.WriteLine("\nPassenger, please enter an appropriate action.");
                        return loggedInUser;
                    default:
                        Console.WriteLine("\nYou must log in to use this command.");
                        return loggedInUser;
                }
            }
            else if (loggedInUser.UserType == UserType.Manager)
            {
                Console.WriteLine("Only passengers can use these commands! Please log out as a manager and log back in as a passenger");
                return loggedInUser;
            }
            else
            {
                switch (command)
                {
                    case PassengerCommand.LogOut:
                        Console.WriteLine("Logged out successfully. We hope to see you soon!");
                        return null;
                    case PassengerCommand.Book:
                        _passengerHelper.Book(loggedInUser, productInfo);
                        return loggedInUser;
                    case PassengerCommand.Search:
                        _passengerHelper.Search(productInfo);
                        return loggedInUser;
                    case PassengerCommand.Cancel:
                        _passengerHelper.Cancel(loggedInUser, productInfo);
                        return loggedInUser;
                    case PassengerCommand.Modify:
                        _passengerHelper.Modify(loggedInUser, productInfo);
                        return loggedInUser;
                    case PassengerCommand.Flights:
                        _passengerHelper.GetFlights();
                        return loggedInUser;
                    case PassengerCommand.Bookings:
                        _passengerHelper.GetBookings(loggedInUser);
                        return loggedInUser;
                    case PassengerCommand.None:
                        Console.WriteLine("\nPlease enter an appropriate action.");
                        return loggedInUser;
                }
            }
            return loggedInUser;
        }
    }
}
