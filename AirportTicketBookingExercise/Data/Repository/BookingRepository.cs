using ATB.Data.Models;
using ATB.Logic.Enums;
using ATB.Logic.Extensions;
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
            try
            {
                using (var reader = new StreamReader(_bookingPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<BookingMap>();
                    records = csv.GetRecords<Booking>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return records;
        }

        public bool CreateBooking(Booking booking)
        {
            booking.GenerateBookingId();
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
            catch (Exception ex) 
            {
                Console.WriteLine($"Error when trying to create booking: {ex.ToString()}");
                return false;
            }
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
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while trying to delete booking: {ex.ToString()}");
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
            catch (Exception ex) 
            {
                Console.WriteLine($"Error when trying to update booking class: {ex.ToString()}");
                return false;
            }
        }
    }
}
