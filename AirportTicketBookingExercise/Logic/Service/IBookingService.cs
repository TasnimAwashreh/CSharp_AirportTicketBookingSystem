using ATB.Data;
using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IBookingService
    {
        bool CreateBooking(Booking Booking);
        List<Booking> GetAllBookings();
        List<Booking> GetBookings(int passengerId);
        bool RemoveBookingById(int BookingId);
        bool IsBookingValidById(int BookingId, int passengerId);
        List<Booking> FilterBookings(BookingFilter query);
    }
}