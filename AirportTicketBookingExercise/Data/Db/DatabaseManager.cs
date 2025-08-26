using ATB.Data.Models;
using CsvHelper;
using Microsoft.Data.Sqlite;
using System.Formats.Asn1;
using System.Globalization;

namespace ATB.Data.Db
{
    public class DatabaseManager
    {
        private string _usersPath;
        private string _bookingsPath;

        public DatabaseManager(string usersPath, string bookingsPath)
        {
            _usersPath = usersPath;
            _bookingsPath = bookingsPath;
        }

        public void CreateDatabase()
        {
            if (!File.Exists(_usersPath))
            {
                using (var writer = new StreamWriter(_usersPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<UserMap>();
                    csv.WriteHeader<User>();
                    csv.NextRecord();
                }
            }
            if (!File.Exists(_bookingsPath))
            {
                using (var writer = new StreamWriter(_bookingsPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<BookingMap>();
                    csv.WriteHeader<Booking>();
                    csv.NextRecord();
                }
            }
        }

    }
}
