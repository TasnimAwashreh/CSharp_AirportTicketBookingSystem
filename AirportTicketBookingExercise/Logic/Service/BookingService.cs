using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Enums;

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

        public bool CreateBooking(Booking booking)
        {
            return _bookingRepository.CreateBooking(booking);
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public bool IsBookingValidById(int bookingId, int passengerId)
        {
            return _bookingRepository.GetAllBookings()
                .Any(b => b.BookingId == bookingId && b.PassengerId == passengerId);
        }

        public List<Booking> GetBookings(int passengerId)
        {
            return _bookingRepository.GetAllBookings()
                .Where(b => b.PassengerId == passengerId)
                .ToList();
        }

        public bool RemoveBookingById(int bookingId)
        {
            return _bookingRepository.DeleteBooking(bookingId);
        }

        public bool UpdateBookingClass(int bookingId, BookingClass newClass)
        {
            return _bookingRepository.UpdateBookingClass(bookingId, newClass);
        }

        public List<Booking> FilterBookings(BookingFilter query)
        {
            var bookings = _bookingRepository.GetAllBookings();
            var flights = _flightRepository.GetFlights();
            var passengers = _userRepository.GetAllUsers().Where(u => u.UserType == UserType.Passenger).ToList();

            var filteredBookings = bookings.Where(b =>
            {
                var flight = flights.FirstOrDefault(f => f.FlightId == b.FlightId);
                var passenger = passengers.FirstOrDefault(p => p.UserId == b.PassengerId);

                if (flight == null)
                    return false;

                if (!string.IsNullOrEmpty(query.FlightName) && (flight.FlightName == null || !flight.FlightName.Contains(query.FlightName)))
                    return false;

                if (query.Price.HasValue)
                {
                    decimal price = 0;
                    switch (b.BookingClass)
                    {
                        case BookingClass.Economy:
                            price = flight.EconomyPrice;
                            break;
                        case BookingClass.Business:
                            price = flight.BuisnessPrice;
                            break;
                        case BookingClass.First:
                            price = flight.FirstClassPrice;
                            break;
                    }
                    if (price > query.Price.Value)
                        return false;
                }
                if (!string.IsNullOrEmpty(query.DepartureCountry) && (flight.DepartureCountry == null || !flight.DepartureCountry.Contains(query.DepartureCountry)))
                    return false;
                if (!string.IsNullOrEmpty(query.DestinationCountry) && (flight.DestinationCountry == null || !flight.DestinationCountry.Contains(query.DestinationCountry)))
                    return false;
                if (query.DepartureDate.HasValue && (flight.DepartureDate.Date != query.DepartureDate.Value.Date))
                    return false;
                if (!string.IsNullOrEmpty(query.DepartureAirport) && (flight.DepartureAirport == null || !flight.DepartureAirport.Contains(query.DepartureAirport)))
                    return false;
                if (!string.IsNullOrEmpty(query.ArrivalAirport) && (flight.ArrivalAirport == null || !flight.ArrivalAirport.Contains(query.ArrivalAirport)))
                    return false;
                if (!string.IsNullOrEmpty(query.PassengerName) && (passenger == null || passenger.Name == null || !passenger.Name.Contains(query.PassengerName)))
                    return false;
                if (!string.IsNullOrEmpty(query.BookingClass))
                {
                    if (!Enum.TryParse<BookingClass>(query.BookingClass, true, out var bookingClass) || b.BookingClass != bookingClass)
                        return false;
                }
                return true;
            }
            )
            .ToList();
            return filteredBookings;
        }
    }
}
