using ATB.Data.Models;
using ATB.Logic.Enums;
using ATB.Logic.Service;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AirportTicketBookingExercise.App.Commands.Helpers
{
    public class PassengerHelper
    {
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public PassengerHelper(IFlightService flightService, IUserService userService, IBookingService bookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
        }

        public void SignUp(string[] productInfo)
        {
            if (productInfo.Length < 3)
            {
                Console.WriteLine("Please enter a username and password");
                return;
            }
            try
            {
                var user = new User
                {
                    Name = productInfo[1],
                    Password = productInfo[2],
                    UserType = UserType.Passenger
                };
                _userService.CreateUser(user);
                Console.WriteLine("You have signed up successfully, passenger");
            }
            catch (ValidationException ex) { Console.WriteLine("Username and Password must be in between 3 and 12"); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public User? Login(User? loggedInUser, string[] productInfo)
        {
            if (productInfo.Length < 3)
            {
                Console.WriteLine("Please enter your username and password to login, manager!");
                return null;
            }

            try
            {
                var user = new User
                {
                    Name = productInfo[1],
                    Password = productInfo[2],
                    UserType = UserType.Passenger
                };

                User? loggingInUser = _userService.Authenticate(user);
                if (loggingInUser == null)
                    Console.WriteLine("Incorrect username or password, please try again");
                else
                {
                    loggedInUser = loggingInUser;
                    Console.WriteLine($"Welcome back, {loggedInUser.Name}!");
                }
                return loggingInUser;
            }
            catch (KeyNotFoundException ex) 
            {
                Console.WriteLine("Username and Password must be in between 3 and 12");
                return null;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void Book(User loggedInUser, string[] productInfo)
        {
            try
            {
                if (productInfo.Length < 3)
                {
                    Console.WriteLine("Please enter the flight Id you want to book and the class");
                }
                int flightId = int.Parse(productInfo[1]);
                BookingClass bookingClass = productInfo[2].ParseBookingClass();
                _bookingService.PassengerBookFlight(flightId, bookingClass, loggedInUser);

                    Console.WriteLine("Please make sure the flight and flight class is available when booking");

                Console.WriteLine("Booking successful! Please enjoy your flight");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter the id of the flight you want to book and the class (first, economy, business)");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Please make sure the flight class is available when booking");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Please make sure the flight is available when booking");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Search(string[] productInfo)
        {
            if (productInfo.Length < 2)
                _flightService.GetFlights();
            else
            {
                List<Flight> searchFlights = _flightService.Search(productInfo);
                Console.WriteLine(_flightService.FlightsToString(searchFlights));
            }
        }

        public void Cancel(User loggedInUser, string[] productInfo)
        {
            try
            {
                int bookingId = int.Parse(productInfo[1]);
                _bookingService.Cancel(bookingId, loggedInUser);
                Console.WriteLine("Booking has been successfully cancelled");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("This booking was unable to cancel. Please try again another time");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("This booking does not exist. Please make sure the booking exists and please enter the class");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter the id of the booking you wish to cancel");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Modify(User loggedInUser, string[] productInfo)
        {
            try
            {
                int bookingId = int.Parse(productInfo[1]);
                BookingClass bookingClass = productInfo[2].ParseBookingClass();
                _bookingService.Modify(bookingId, bookingClass, loggedInUser);
                
                Console.WriteLine("Booking changed successfully!");
            }
            catch(KeyNotFoundException)
            {
                Console.WriteLine("This booking does not exist. Please make sure the booking exists and please enter the class");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Please enter the id of the booking you wish to modify, and the and the class (first, economy, business)");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter the booking id and class in the correct format");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void GetFlights()
        {
            List<Flight> getFlights = _flightService.GetFlights();
            if (!getFlights.Any()) Console.WriteLine("There are currently no flights");
            else Console.WriteLine(_flightService.FlightsToString(getFlights));
        }

        public void GetBookings(User loggedInUser)
        {
            List<Booking> bookings = _bookingService.GetBookings(loggedInUser);
            if (!bookings.Any()) Console.WriteLine("You currently have not booked any flights yet!");
            Console.WriteLine(_bookingService.BookingsToString(bookings));
        }
    }
}
