using ATB.Data.Models;
using ATB.Logic.Enums;
using ATB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATB.Data.Repository;

namespace AirportTicketBookingExercise.Data.Repository
{
    public class BookingsFilterRepository : IBookingsFilterRepository
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookRepository;

        public BookingsFilterRepository(IFlightRepository flightRepository, IUserRepository userRepository, IBookingRepository bookRepository)
        {
            _flightRepository = flightRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public List<Booking> FilterBookingsWithFlights(string[] filterInput)
        {
            BookingFilter query = BookingFilters.Parse(filterInput.Skip(1).ToArray());
            List<Booking> bookingResults = new List<Booking>();

            User? passenger = _userRepository.GetUser(query.PassengerName);
            List<Flight> flights = _flightRepository.FilterFlights(query);
            List<Booking> bookings = _bookRepository.FilterBooking(query);

            bookingResults =
                (
                 from b in bookings
                 join f in flights on b.FlightId equals f.FlightId
                 where (passenger == null || b.PassengerId == passenger.UserId)
                 select b
                 ).ToList();

            return bookingResults;
        }
    }
}
