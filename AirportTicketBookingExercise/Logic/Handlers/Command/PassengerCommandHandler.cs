using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;


namespace ATB.Logic.Handlers.Command
{
    public class PassengerCommandHandler
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IFlightService _flightService;

        private User? loggedInUser;

        public PassengerCommandHandler(IBookingService bookingService, IUserService userService, IFlightService flightService)
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
                    case PassengerCommand.logout:
                        PassengerSignOut();
                        break;
                    case PassengerCommand.book:
                        PassengerBookFlight(productInfo);
                        break;
                    case PassengerCommand.search:
                        Search(productInfo);
                        break;
                    case PassengerCommand.cancel:
                        Cancel(productInfo);
                        break;
                    case PassengerCommand.modify:
                        Console.WriteLine("Modify functionality not yet implemented.");
                        break;
                    case PassengerCommand.flights:
                        Flights();
                        break;
                    case PassengerCommand.bookings:
                        Bookings();
                        break;
                    case PassengerCommand.none:
                        Console.WriteLine("\nPlease enter an appropriate action.");
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case PassengerCommand.signup:
                        PassengerSignUp(productInfo);
                        break;
                    case PassengerCommand.login:
                        PassengerLogIn(productInfo);
                        break;
                    case PassengerCommand.none:
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
                BookingClass bookingClass = BookingClasses.strToBookingClass(productInfo[2]);

                Flight? flight = _flightService.GetFlightById(flightId);
                if (flight != null && flight.seatsAvailable < flight.SeatCapacity)
                {
                    decimal price = bookingClass switch
                    {
                        BookingClass.first => flight.FirstClassPrice,
                        BookingClass.business => flight.BuisnessPrice,
                        BookingClass.economy => flight.EconomyPrice,
                        _ => 0
                    };
                    if (price != 0)
                    {
                        var booking = new Booking
                        {
                            FlightId = flightId,
                            PassengerId = loggedInUser!.UserId,
                            BookingClass = bookingClass
                        };
                        _bookingService.CreateBooking(booking);
                        _flightService.AddPassengerToSeat(flight);

                        Console.WriteLine($"You have booked flight ID {flightId} in {bookingClass} class.");
                    }
                    else
                    {
                        Console.WriteLine($"This flight does not offer {bookingClass} class.");
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
                    FilterParam searchParam = BookingFilters.GetFilterParam(productInfo[1]);
                    string input = productInfo[2];

                    var flights = _flightService.GetFlights();
                    List<Flight> filteredFlights = searchParam switch
                    {
                        FilterParam.flight => flights.Where(f => f.FlightName.Equals(input)).ToList(),
                        FilterParam.price => flights.Where(f =>
                            f.BuisnessPrice == decimal.Parse(input) ||
                            f.EconomyPrice == decimal.Parse(input) ||
                            f.FirstClassPrice == decimal.Parse(input)).ToList(),
                        FilterParam.departure_country => flights.Where(f => f.DepartureCountry.Equals(input)).ToList(),
                        FilterParam.destination_country => flights.Where(f => f.DestinationCountry.Equals(input)).ToList(),
                        FilterParam.departure_date => flights.Where(f => f.DepartureDate.Equals(input)).ToList(),
                        FilterParam.departure_airport => flights.Where(f => f.DepartureAirport.Equals(input)).ToList(),
                        FilterParam.arrival_airport => flights.Where(f => f.ArrivalAirport.Equals(input)).ToList(),
                        _ => []
                    };

                    PrintOutFlights(filteredFlights);
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
                int bookingId = int.Parse(productInfo[1]);
                if (_bookingService.ValidateBookingById(bookingId, loggedInUser!.UserId))
                {
                    bool isSuccess = _bookingService.RemoveBookingById(bookingId);
                    if (isSuccess)
                        Console.WriteLine($"Booking with ID {bookingId} deleted successfully.");
                    else
                        Console.WriteLine($"Failed to delete booking with ID {bookingId}.");
                }
                else
                {
                    Console.WriteLine("You did not book this specific flight.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid booking ID.");
            }
        }

        private void Bookings()
        {
            var bookings = _bookingService.GetBookingsByUserId(loggedInUser!.UserId);
            PrintOutBookings(bookings);
        }

        private void Flights()
        {
            var flights = _flightService.GetFlights();
            PrintOutFlights(flights);
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
                            Console.WriteLine("Passenger signup was unsuccessful.");
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

        private void PrintOutFlights(List<Flight> flights)
        {
            foreach (var flight in flights)
            {
                Console.WriteLine(flight.ToString());
            }
        }

        private void PrintOutBookings(List<Booking> bookingList)
        {
            foreach (var booking in bookingList)
            {
                Console.WriteLine(booking.ToString());
            }
        }
    }
}
