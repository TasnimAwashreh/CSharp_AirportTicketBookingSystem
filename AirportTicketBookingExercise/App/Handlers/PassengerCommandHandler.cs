using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;
using System.Text;


namespace ATB.App.Handlers
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

        public bool PassengerBookFlight(int flightId, BookingClass bookingClass, User loggedInUser)
        {
            try
            { 
                Flight? flight = _flightService.GetFlight(flightId);
                if (flight == null || flight.SeatsAvailable >= flight.SeatCapacity)
                    return false;
                decimal price = bookingClass switch
                {
                    BookingClass.First => flight.FirstClassPrice,
                    BookingClass.Business => flight.BuisnessPrice,
                    BookingClass.Economy => flight.EconomyPrice,
                    _ => 0
                };
                if (price == 0)
                    return false;
                var Booking = new Booking
                {
                    BookingId = new Random().Next(100000, 999999),
                    FlightId = flightId,
                    PassengerId = loggedInUser.UserId,
                    BookingClass = bookingClass
                };
                _bookingService.CreateBooking(Booking);
                _flightService.AddPassengerToSeat(flight);

                return true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error while booking: {ex.ToString()}");
                return false;
            }
        }

        public List<Flight> Search(string[] filterInfo)
        {
            List<Flight> filteredFlights = new List<Flight>();
            if (filterInfo.Length < 3)
                return _flightService.GetFlights();

            FilterParam SearchParam = filterInfo[1].ParseFilterParam();
            string input = filterInfo[2];

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
            return filteredFlights;
        }

        public bool Cancel(int bookingId, User loggedInUser)
        {
            try
            {
                if (!_bookingService.IsBookingValidById(bookingId, loggedInUser.UserId))
                    return false;
                bool isSuccess = _bookingService.RemoveBookingById(bookingId);
                if (!isSuccess)
                    return false;
                else return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Modify(int bookingId, BookingClass bookingClass, User loggedInUser)
        {
            if (!_bookingService.IsBookingValidById(bookingId, loggedInUser.UserId))
                return false;
            _bookingService.UpdateBookingClass(bookingId, bookingClass);
            return true;
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

        public bool PassengerSignUp(string name, string password)
        {
            try
            {
                var user = new User
                {
                    Name = name,
                    Password = password,
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
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while signing up passenger: {ex}");
                return false;
            }
        }

        public User? PassengerLogIn(string name, string pass)
        {
            var user = _userService.Authenticate(name, pass);
            if (user == null || user.UserType != UserType.Passenger)
                return null;
            return user;
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
