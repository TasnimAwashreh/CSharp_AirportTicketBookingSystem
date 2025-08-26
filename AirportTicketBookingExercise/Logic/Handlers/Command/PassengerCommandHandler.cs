using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;
using System.Text;


namespace ATB.Logic.Handlers.Command
{
    public class PassengerCommandHandler
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IFlightservice _flightService;

        public PassengerCommandHandler(IBookingService bookingService, IUserService userService, IFlightservice flightService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _flightService = flightService;
        }

        public bool PassengerBookFlight(string[] productInfo, User loggedInUser)
        {
            try
            {
                int flightId = int.Parse(productInfo[1]);
                BookingClass BookingClass = productInfo[2].ParseBookingClass();

                Flight? flight = _flightService.GetFlight(flightId);
                if (flight == null || flight.SeatsAvailable >= flight.SeatCapacity)
                    return false;
                else
                {
                    decimal price = BookingClass switch
                    {
                        BookingClass.First => flight.FirstClassPrice,
                        BookingClass.Business => flight.BuisnessPrice,
                        BookingClass.Economy => flight.EconomyPrice,
                        _ => 0
                    };
                    if (price == 0)
                        return false;
                    else
                    {
                        var Booking = new Booking
                        {
                            BookingId = new Random().Next(100000, 999999),
                            FlightId = flightId,
                            PassengerId = loggedInUser.UserId,
                            BookingClass = BookingClass
                        };
                        _bookingService.CreateBooking(Booking);
                        _flightService.AddPassengerToSeat(flight);

                        return true;
                    };
                }
                
            }
            catch
            {
                return false;
            }
        }

        public List<Flight> Search(string[] productInfo)
        {
            List<Flight> filteredFlights = new List<Flight>();
            if (productInfo.Length < 3)
                return _flightService.GetFlights();
            else
            {
                FilterParam SearchParam = productInfo[1].ParseFilterParam();
                string input = productInfo[2];

                var flights = _flightService.GetFlights();
            filteredFlights = SearchParam switch
                {
                    FilterParam.Flight => flights.Where(f => f.FlightName.Equals(input)).ToList(),
                    FilterParam.Price => flights.Where(f =>
                        f.BuisnessPrice == decimal.Parse(input) ||
                        f.EconomyPrice == decimal.Parse(input) ||
                        f.FirstClassPrice == decimal.Parse(input)).ToList(),
                    FilterParam.DepartureCountry => flights.Where(f => f.DepartureCountry.Equals(input)).ToList(),
                    FilterParam.DestinationCountry => flights.Where(f => f.DestinationCountry.Equals(input)).ToList(),
                    FilterParam.DepartureDate => flights.Where(f => f.DepartureDate.Equals(input)).ToList(),
                    FilterParam.DepartureAirport => flights.Where(f => f.DepartureAirport.Equals(input)).ToList(),
                    FilterParam.ArrivalAirport => flights.Where(f => f.ArrivalAirport.Equals(input)).ToList(),
                    _ => []
                };
            }
            return filteredFlights;
        }

        public bool Cancel(string[] productInfo, User loggedInUser)
        {
            try
            {
                int bookingId = int.Parse(productInfo[1]);
                if (!_bookingService.IsBookingValidById(bookingId, loggedInUser.UserId))
                    return false;
                else
                {
                    bool isSuccess = _bookingService.RemoveBookingById(bookingId);
                    if (!isSuccess)
                        return false;
                    else return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Modify(string[] productInfo, User loggedInUser)
        {
            if (productInfo.Length < 3)
                return false;
            else
            {
                try
                {
                    int bookingId = int.Parse(productInfo[1]);
                    BookingClass bookingClass = productInfo[2].ParseBookingClass();

                    if (!_bookingService.IsBookingValidById(bookingId, loggedInUser.UserId))
                        return false;
                    else
                    {
                        _bookingService.UpdateBookingClass(bookingId, bookingClass);
                        return true;
                    }
                }
                catch { return false; }
            }

        }

        public List<Booking> Bookings(User loggedInUser)
        {
            List<Booking> bookings = new List<Booking>();
            if (loggedInUser != null)
                bookings = _bookingService.GetBookings(loggedInUser.UserId);
            return bookings;
        }

        public List<Flight> Flights()
        {
            List<Flight> flights = new List<Flight>();
            flights = _flightService.GetFlights();
            return flights;
            
        }

        public bool PassengerSignUp(string[] productInfo)
        {
            if (productInfo.Length < 3)
                return false;
            else
            {
                try
                {
                    var user = new User
                    {
                        UserId = new Random().Next(100000, 999999),
                        Name = productInfo[1],
                        Password = productInfo[2],
                        UserType = UserType.Passenger
                    };

                    if (_userService.GetUserByName(user.Name) != null)
                        return false;
                    else
                    {
                        bool isSuccess = _userService.CreateUser(user);
                        if (isSuccess)
                            return true;
                        else
                            return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public User? PassengerLogIn(string[] productInfo, User? loggedInUser)
        {
            if (productInfo.Length >= 3)
            {
                string username = productInfo[1];
                string password = productInfo[2];

                var user = _userService.Authenticate(username, password);

                if (user != null && user.UserType == UserType.Passenger)
                    return user;
            }
            return null;
        }

        public bool PassengerSignOut(User? loggedInUser)
        {
            if (loggedInUser == null)
                return false;
            else
            {
                loggedInUser = null;
                return true;
            }
        }

        public string FlightsToString(List<Flight> Flights)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var flight in Flights)
            {
                stringBuilder.AppendLine(flight.ToString());
            }
            return stringBuilder.ToString();    
        }
        public string BookingsToString(List<Booking> BookingList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var Booking in BookingList)
            {
                stringBuilder.AppendLine(Booking.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
