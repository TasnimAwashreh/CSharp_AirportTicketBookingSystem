using ATB.Data;
using ATB.Data.Repository;
using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Logic.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFlightRepository _flightRepository;
        public BookingService(IBookingRepository bookingRepository,IFlightRepository flightRepository, IUserRepository userRepository)
        {
            this._bookingRepository = bookingRepository;
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

        public bool ValidateBookingById(int bookingId, int passengerId)
        {
            return _bookingRepository.ValidatePassengerBooking(bookingId, passengerId);
        }

        public List<Booking> GetBookingsByUserId(int passengerId)
        {
            return _bookingRepository.GetBookingsByUserId(passengerId);
        }

        public bool RemoveBookingById(int bookingId)
        {
            return _bookingRepository.DeleteBooking(bookingId);
        }

        public List<Booking> FilterBookings(BookingFilter query)
        {
            var bookings = _bookingRepository.GetAllBookings();
            var flights = _flightRepository.getFlights();
            var passengers = _userRepository.GetAllUsers().Where(u => u.UserType == UserType.Passenger).ToList();

            var filtered = bookings.Where(b =>
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
                        case BookingClass.economy:
                            price = flight.EconomyPrice;
                            break;
                        case BookingClass.business:
                            price = flight.BuisnessPrice;
                            break;
                        case BookingClass.first:
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

                if (query.DepartureDate.HasValue && (!flight.DepartureDate.HasValue || flight.DepartureDate.Value.Date != query.DepartureDate.Value.Date))
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
            }).ToList();

            return filtered;
        }

    }
}
