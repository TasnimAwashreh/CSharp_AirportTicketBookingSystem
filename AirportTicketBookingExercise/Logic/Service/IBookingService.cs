using ATB.Data;
using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IBookingService
    {
        bool CreateBooking(Booking Booking);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsByUserId(int passengerId);
        bool RemoveBookingById(int BookingId);
        bool ValidateBookingById(int BookingId, int passengerId);
        List<Booking> FilterBookings(BookingFilter query);
    }
}