using ATB.Data.Models;
using System.Collections.Generic;

namespace AirportTicketBookingExercise.Data.Repository
{
    public interface IBookingRepository
    {
        public bool CreateBooking(Booking booking);
        public List<Booking> GetBookingsByUserId(int PassengerId);
        public List<Booking> GetAllBookings();
        public bool ValidatePassengerBooking(int bookingId, int passengerId);
        public bool DeleteBooking(int bookingId);

    }
}
