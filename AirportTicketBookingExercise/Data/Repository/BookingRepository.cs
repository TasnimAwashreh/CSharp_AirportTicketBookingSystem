using ATB.Data.Extensions;
using ATB.Data.Models;
using ATB.Logic.Enums;
using CsvHelper;
using System.Globalization;
using AirportTicketBookingExercise.Logic.Utils;
using ATB.Logic;

namespace ATB.Data.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _bookingPath;

        public BookingRepository(string bookingPath)
        {
            _bookingPath = bookingPath;
        }

        public List<Booking> GetAllBookings()
        {
            return CsvActionsHelper.GetAllRecords<Booking, BookingMap>(_bookingPath);
        }

        public List<Booking> GetBookings(int passengerId)
        {
            return GetAllBookings()
                .Where(b => b.PassengerId == passengerId)
                .ToList();
        }

        public bool IsBookingValidById(int bookingId, int passengerId)
        {
            return GetAllBookings()
                .Any(b => b.BookingId == bookingId && b.PassengerId == passengerId);
        }

        public void CreateBooking(Booking booking)
        {
            booking.GenerateBookingId();
            CsvActionsHelper.CreateRecord<Booking, BookingMap>(_bookingPath, booking);
        }

        public bool DeleteBooking(int bookingId)
        {
            var bookings = GetAllBookings();
            var updatedBookings = bookings
                .Where(b => b.BookingId != bookingId)
                .ToList();
            if (updatedBookings.Count == bookings.Count)
                return false;
            return CsvActionsHelper.UpdateRecords<Booking, BookingMap>(_bookingPath, updatedBookings);
        }

        public bool UpdateBookingClass(int bookingId, BookingClass newClass)
        {
            var bookings = GetAllBookings();
            var target = bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (target == null)
                return false;
            target.BookingClass = newClass;
            return CsvActionsHelper.UpdateRecords<Booking, BookingMap>(_bookingPath, bookings);
        }

        public List<Booking> FilterBooking(BookingFilter filter)
        {
            var bookings = GetAllBookings();
            var query = 
                from booking in bookings
                where 
                    (filter.BookingClass == null || (booking.BookingClass.ToString() == filter.BookingClass))
                select booking;
            return query.ToList();
        }
    }
}
