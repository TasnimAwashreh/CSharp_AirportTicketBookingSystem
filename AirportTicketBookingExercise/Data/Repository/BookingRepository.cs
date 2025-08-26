
using ATB.Data.Models;
using ATB.Logic.Enums;
using CsvHelper;
using Microsoft.Data.Sqlite;
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

        private Booking MapBooking(SqliteDataReader reader)
        {
            return new Booking
            {
                BookingId = reader.GetInt32(0),
                FlightId = reader.GetInt32(1),
                PassengerId = reader.GetInt32(2),
                BookingClass = reader.GetString(3).ParseBookingClass()
            };
        }
         
        public List<Booking> GetAllBookings()
        {
            using (var reader = new StreamReader(_bookingPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookingMap>();
                var records = csv.GetRecords<Booking>().ToList();
                return records;
            }
        }

        public bool CreateBooking(Booking booking)
        {
            try
            {
                using (var writer = new StreamWriter(_bookingPath, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<BookingMap>();
                    csv.WriteRecord<Booking>(booking);
                    csv.NextRecord();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Booking> GetBookings(int passengerId)
        {
            return GetAllBookings()
                .Where(b => b.PassengerId == passengerId)
                .ToList();
        }

        public bool IsPassengerBookingValid(int bookingId, int passengerId)
        {
            return GetAllBookings()
                .Any(b => b.BookingId == bookingId && b.PassengerId == passengerId);
        }

        public bool DeleteBooking(int bookingId)
        {
            try
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
            catch
            {
                return false;
            }
        }

        public bool UpdateBookingClass(int bookingId, BookingClass newClass)
        {
            try
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
            catch
            {
                return false;
            }
        }

    }
}
