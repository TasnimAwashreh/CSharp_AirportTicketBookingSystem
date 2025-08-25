
using ATB.Data.Models;
using ATB.Logic.Enums;
using Microsoft.Data.Sqlite;

namespace ATB.Data.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _connectionString;

        public BookingRepository(string connectionString) 
        {
            _connectionString = connectionString;
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

        public bool CreateBooking(Booking booking)
        {
            try
            {
                using var conn = new SqliteConnection($"Data Source={_connectionString}");
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Booking (FlightId, PassengerId, BookingClass) VALUES ($flight, $passenger, $class)";
                cmd.Parameters.AddWithValue("$flight", booking.FlightId);
                cmd.Parameters.AddWithValue("$passenger", booking.PassengerId);
                cmd.Parameters.AddWithValue("$class", booking.BookingClass.ToString());
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Booking> GetBookings(int passengerId)
        {
            var list = new List<Booking>();
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Booking WHERE PassengerId = $userId";
            cmd.Parameters.AddWithValue("$userId", passengerId);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(MapBooking(reader));
                }
            }
            return list;
        }

        public List<Booking> GetAllBookings()
        {
            var list = new List<Booking>();
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Booking";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapBooking(reader));
            }
            conn.Close();
            return list;
        }

        public bool IsPassengerBookingValid(int bookingId, int passengerId)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1 FROM Booking WHERE BookingId = $BookingId AND PassengerId = $passengerId";
            cmd.Parameters.AddWithValue("$BookingId", bookingId);
            cmd.Parameters.AddWithValue("$passengerId", passengerId);
            var reader = cmd.ExecuteReader();
            conn.Close();
            return reader.Read();
        }

        public bool DeleteBooking(int bookingId)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            bool res = false;
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Booking WHERE BookingId = $BookingId";
                cmd.Parameters.AddWithValue("$BookingId", bookingId);
                res = cmd.ExecuteNonQuery() > 0;
            }
            return res;
        }

        public void UpdateBookingClass(int BookingId, string newClass)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Booking SET BookingClass = $class WHERE BookingId = $id";
            cmd.Parameters.AddWithValue("$class", newClass);
            cmd.Parameters.AddWithValue("$id", BookingId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}
