using ATB.Data;
using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IBookingService
    {
        bool CreateBooking(Booking booking);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsByUserId(int passengerId);
        bool RemoveBookingById(int bookingId);
        bool ValidateBookingById(int bookingId, int passengerId);
        List<Booking> FilterBookings(BookingFilter query);
    }
}