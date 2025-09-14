using AirportTicketBookingExercise.Data.Repository;
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
        private readonly IBookingsFilterRepository _bookingsFilterRepository;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository, IFlightRepository flightRepository, IBookingsFilterRepository bookingsFilterRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _flightRepository = flightRepository;
            _bookingsFilterRepository = bookingsFilterRepository;
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

        public void Cancel(int bookingId, User loggedInUser)
        {
            if (!_bookingRepository.IsBookingValidById(bookingId, loggedInUser.UserId))
                throw new KeyNotFoundException();
            bool isSuccess = _bookingRepository.DeleteBooking(bookingId);
            if (!isSuccess)
                throw new InvalidOperationException();
        }

        public void Modify(int bookingId, BookingClass bookingClass, User loggedInUser)
        {
            if (!_bookingRepository.IsBookingValidById(bookingId, loggedInUser.UserId))
                throw new KeyNotFoundException();
            _bookingRepository.UpdateBookingClass(bookingId, bookingClass);
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
            return _bookingsFilterRepository.FilterBookingsWithFlights(filterInput);
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
