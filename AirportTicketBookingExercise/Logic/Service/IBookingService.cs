using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Logic.Service
{
    public interface IBookingService
    {
        List<Booking> GetAllBookings();
        public List<Booking> GetBookings(User loggedInUser);
        bool RemoveBookingById(int BookingId);
        bool IsBookingValidById(int BookingId, int passengerId);
        List<Booking> FilterBookings(string[] filterInput);
        public bool UpdateBookingClass(int bookingId, BookingClass newClass);
        public bool PassengerBookFlight(int flightId, BookingClass bookingClass, User loggedInUser);
        public bool Cancel(int bookingId, User loggedInUser);
        public bool Modify(int bookingId, BookingClass bookingClass, User loggedInUser);
        public string BookingsToString(List<Booking> BookingList);
    }
}