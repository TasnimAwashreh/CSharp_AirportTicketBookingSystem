using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;


namespace ATB.Logic.Handlers.Command
{
    public class PassengerCommandHandler
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IFlightservice _flightService;

        private User? loggedInUser;

        public PassengerCommandHandler(IBookingService bookingService, IUserService userService, IFlightservice flightService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _flightService = flightService;
        }

        public void ExecutePassengerCommand(string[] productInfo, PassengerCommand command)
        {
            if (loggedInUser != null)
            {
                switch (command)
                {
                    case PassengerCommand.LogOut:
                        PassengerSignOut();
                        break;
                    case PassengerCommand.Book:
                        PassengerBookFlight(productInfo);
                        break;
                    case PassengerCommand.Search:
                        Search(productInfo);
                        break;
                    case PassengerCommand.Cancel:
                        Cancel(productInfo);
                        break;
                    case PassengerCommand.Modify:
                        Console.WriteLine("Modify functionality not yet implemented.");
                        break;
                    case PassengerCommand.Flights:
                        Flights();
                        break;
                    case PassengerCommand.Bookings:
                        Bookings();
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
                        PassengerSignUp(productInfo);
                        break;
                    case PassengerCommand.LogIn:
                        PassengerLogIn(productInfo);
                        break;
                    case PassengerCommand.None:
                        Console.WriteLine("\nPassenger, please enter an appropriate action.");
                        break;
                    default:
                        Console.WriteLine("You must log in to use this command.");
                        break;
                }
            }
        }

        private void PassengerBookFlight(string[] productInfo)
        {
            try
            {
                int flightId = int.Parse(productInfo[1]);
                BookingClass BookingClass = productInfo[2].ParseBookingClass();

                Flight? flight = _flightService.GetFlight(flightId);
                if (flight != null && flight.SeatsAvailable < flight.SeatCapacity)
                {
                    decimal price = BookingClass switch
                    {
                        BookingClass.First => flight.FirstClassPrice,
                        BookingClass.Business => flight.BuisnessPrice,
                        BookingClass.Economy => flight.EconomyPrice,
                        _ => 0
                    };
                    if (price != 0)
                    {
                        var Booking = new Booking
                        {
                            FlightId = flightId,
                            PassengerId = loggedInUser!.UserId,
                            BookingClass = BookingClass
                        };
                        _bookingService.CreateBooking(Booking);
                        _flightService.AddPassengerToSeat(flight);

                        Console.WriteLine($"You have Booked flight ID {flightId} in {BookingClass} class.");
                    }
                    else
                    {
                        Console.WriteLine($"This flight does not offer {BookingClass} class.");
                    }
                }
                else
                {
                    Console.WriteLine("The selected flight is full or does not exist.");
                }
            }
            catch
            {
                Console.WriteLine("Please enter flight ID and class in the correct format.");
            }
        }

        private void Search(string[] productInfo)
        {
            if (productInfo.Length >= 3)
            {
                try
                {
                    FilterParam SearchParam = productInfo[1].ParseFilterParam();
                    string input = productInfo[2];

                    var Flights = _flightService.GetFlights();
                    List<Flight> FilteredFlights = SearchParam switch
                    {
                        FilterParam.Flight => Flights.Where(f => f.FlightName.Equals(input)).ToList(),
                        FilterParam.Price => Flights.Where(f =>
                            f.BuisnessPrice == decimal.Parse(input) ||
                            f.EconomyPrice == decimal.Parse(input) ||
                            f.FirstClassPrice == decimal.Parse(input)).ToList(),
                        FilterParam.DepartureCountry => Flights.Where(f => f.DepartureCountry.Equals(input)).ToList(),
                        FilterParam.DestinationCountry => Flights.Where(f => f.DestinationCountry.Equals(input)).ToList(),
                        FilterParam.DepartureDate => Flights.Where(f => f.DepartureDate.Equals(input)).ToList(),
                        FilterParam.DepartureAirport => Flights.Where(f => f.DepartureAirport.Equals(input)).ToList(),
                        FilterParam.ArrivalAirport => Flights.Where(f => f.ArrivalAirport.Equals(input)).ToList(),
                        _ => []
                    };

                    PrintOutFlights(FilteredFlights);
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Please enter the correct format.");
            }
        }

        private void Cancel(string[] productInfo)
        {
            try
            {
                int BookingId = int.Parse(productInfo[1]);
                if (_bookingService.IsBookingValidById(BookingId, loggedInUser!.UserId))
                {
                    bool isSuccess = _bookingService.RemoveBookingById(BookingId);
                    if (isSuccess)
                        Console.WriteLine($"Booking with ID {BookingId} deleted successfully.");
                    else
                        Console.WriteLine($"Failed to delete Booking with ID {BookingId}.");
                }
                else
                {
                    Console.WriteLine("You did not Book this specific flight.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid Booking ID.");
            }
        }

        private void Bookings()
        {
            var Bookings = _bookingService.GetBookings(loggedInUser!.UserId);
            PrintOutBookings(Bookings);
        }

        private void Flights()
        {
            var Flights = _flightService.GetFlights();
            PrintOutFlights(Flights);
        }

        private void PassengerSignUp(string[] productInfo)
        {
            if (productInfo.Length >= 3)
            {
                try
                {
                    var user = new User
                    {
                        Name = productInfo[1],
                        Password = productInfo[2],
                        UserType = UserType.Passenger
                    };

                    if (_userService.GetUserByName(user.Name) == null)
                    {
                        bool isSuccess = _userService.CreateUser(user);
                        if (isSuccess)
                            Console.WriteLine($"Passenger {user.Name} signed up successfully.");
                        else
                            Console.WriteLine("Passenger SignUp was unsuccessful.");
                    }
                    else Console.WriteLine("Passenger with this username already exists.");
                }
                catch
                {
                    Console.WriteLine("Something went wrong. Please try again later.");
                }
            }
            else Console.WriteLine("Please enter name and password to sign up as a passenger.");
        }

        private void PassengerLogIn(string[] productInfo)
        {
            if (productInfo.Length >= 3)
            {
                string username = productInfo[1];
                string password = productInfo[2];

                var user = _userService.Authenticate(username, password);

                if (user != null && user.UserType == UserType.Passenger)
                {
                    loggedInUser = user;
                    Console.WriteLine($"Welcome back, {user.Name}!");
                }
                else
                {
                    Console.WriteLine("Invalid credentials or not a passenger.");
                }
            }
            else
            {
                Console.WriteLine("Please enter your name and password.");
            }
        }

        private void PassengerSignOut()
        {
            if (loggedInUser != null)
            {
                Console.WriteLine($"Passenger {loggedInUser.Name} has logged out.");
                loggedInUser = null;
            }
            else
            {
                Console.WriteLine("No passenger is currently logged in.");
            }
        }

        private void PrintOutFlights(List<Flight> Flights)
        {
            foreach (var flight in Flights)
            {
                Console.WriteLine(flight.ToString());
            }
        }

        private void PrintOutBookings(List<Booking> BookingList)
        {
            foreach (var Booking in BookingList)
            {
                Console.WriteLine(Booking.ToString());
            }
        }
    }
}
