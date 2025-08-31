using ATB.Data.Extensions;
using ATB.Data.Models;
using ATB.Logic.Enums;
using CsvHelper;
using System.Globalization;

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
            List<Booking> records = new List<Booking>();
            using (var reader = new StreamReader(_bookingPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookingMap>();
                records = csv.GetRecords<Booking>().ToList();
            }
            return records;
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

        public bool CreateBooking(Booking booking)
        {
            booking.GenerateBookingId();
            using (var writer = new StreamWriter(_bookingPath, append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookingMap>();
                csv.WriteRecord<Booking>(booking);
                csv.NextRecord();
                return true;
            }
        }

        public bool DeleteBooking(int bookingId)
        {
            var bookings = GetAllBookings();
            var updatedBookings = bookings
                .Where(b => b.BookingId != bookingId)
                .ToList();
            if (updatedBookings.Count == bookings.Count)
                return false;

            using (var writer = new StreamWriter(_bookingPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookingMap>();
                csv.WriteRecords(updatedBookings);
            }
            return true;
        }

        public bool UpdateBookingClass(int bookingId, BookingClass newClass)
        {
                var bookings = GetAllBookings();
                var target = bookings.FirstOrDefault(b => b.BookingId == bookingId);
                if (target == null)
                    return false;
                target.BookingClass = newClass;
                using (var writer = new StreamWriter(_bookingPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<BookingMap>();
                    csv.WriteRecords(bookings);
                    csv.NextRecord();
                }
                return true;
        }
    }
}
