
using Microsoft.Data.Sqlite;
using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Data.Db.DAOs
{
    public class BookingDAO
    {
        private readonly string _connectionString;

        public BookingDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateBooking(Booking booking)
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

        public bool IsBookingValid(int BookingId, int passengerId)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1 FROM Booking WHERE BookingId = $BookingId AND PassengerId = $passengerId";
            cmd.Parameters.AddWithValue("$BookingId", BookingId);
            cmd.Parameters.AddWithValue("$passengerId", passengerId);
            var reader = cmd.ExecuteReader();
            conn.Close();
            return reader.Read();
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

        public bool DeleteBooking(int BookingId)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            bool res = false;
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Booking WHERE BookingId = $BookingId";
                cmd.Parameters.AddWithValue("$BookingId", BookingId);
                res = cmd.ExecuteNonQuery() > 0;
            }
            return res;
        }

        private Booking MapBooking(SqliteDataReader reader)
        {
            return new Booking
            {
                BookingId = reader.GetInt32(0),
                FlightId = reader.GetInt32(1),
                PassengerId = reader.GetInt32(2),
                BookingClass = BookingClasses.ParseBookingClass(reader.GetString(3))
            };
        }
    }
}
