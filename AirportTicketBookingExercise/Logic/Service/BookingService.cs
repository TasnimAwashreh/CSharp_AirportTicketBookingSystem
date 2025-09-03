using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Enums;
using System.Text;

namespace ATB.Logic.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFlightRepository _flightRepository;

        public BookingService(IBookingRepository BookingRepository, IFlightRepository flightRepository, IUserRepository userRepository)
        {
            this._bookingRepository = BookingRepository;
            this._userRepository = userRepository;
            this._flightRepository = flightRepository;
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public bool IsBookingValidById(int bookingId, int passengerId)
        {
            return _bookingRepository.IsBookingValidById(bookingId, passengerId);
        }

        public List<Booking> GetBookings(User loggedInUser)
        {
            List<Booking> bookings = new List<Booking>();
            if (loggedInUser != null)
                bookings = _bookingRepository.GetBookings(loggedInUser.UserId);
            return bookings;
        }

        public bool RemoveBookingById(int bookingId)
        {
            return _bookingRepository.DeleteBooking(bookingId);
        }

        public bool UpdateBookingClass(int bookingId, BookingClass newClass)
        {
            return _bookingRepository.UpdateBookingClass(bookingId, newClass);
        }

        public bool Cancel(int bookingId, User loggedInUser)
        {
            if (!_bookingRepository.IsBookingValidById(bookingId, loggedInUser.UserId))
                return false;
            bool isSuccess = _bookingRepository.DeleteBooking(bookingId);
            if (!isSuccess)
                return false;
            return true;
        }

        public bool Modify(int bookingId, BookingClass bookingClass, User loggedInUser)
        {
            if (!_bookingRepository.IsBookingValidById(bookingId, loggedInUser.UserId))
                return false;
            _bookingRepository.UpdateBookingClass(bookingId, bookingClass);
            return true;
        }

        public bool PassengerBookFlight(int flightId, BookingClass bookingClass, User loggedInUser)
        {
            Flight? flight = _flightRepository.GetFlight(flightId);
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
                FlightId = flightId,
                PassengerId = loggedInUser.UserId,
                BookingClass = bookingClass
            };
            _bookingRepository.CreateBooking(Booking);
            _flightRepository.AddPassengerToSeat(flight);
            return true;
        }

        public List<Booking> FilterBookings(string[] filterInput)
        {
            BookingFilter query = BookingFilters.Parse(filterInput.Skip(1).ToArray());
            List<Booking> bookingResults = new List<Booking>();

            User? passenger = _userRepository.GetUser(query.PassengerName);
            List<Flight> flights = _flightRepository.FilterFlights(query);
            List<Booking> bookings = _bookingRepository.FilterBooking(query);

            bookingResults =
                (
                 from b in bookings
                    join f in flights on b.FlightId equals f.FlightId
                    where (passenger == null || b.PassengerId == passenger.UserId)
                 select b
                 ).ToList();

            return bookingResults;
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
