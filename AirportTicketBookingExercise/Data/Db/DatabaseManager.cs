using ATB.Data.Models;
using CsvHelper;
using System.Globalization;

namespace ATB.Data.Db
{
    public class DatabaseManager
    {
        private string _usersPath;
        private string _bookingsPath;
        private string _flightsPath;

        public DatabaseManager(string usersPath, string bookingsPath, string flightsPath)
        {
            _usersPath = usersPath;
            _bookingsPath = bookingsPath;
            _flightsPath = flightsPath;
        }

        public void CreateDatabase()
        {
            try
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

                if (!File.Exists(_flightsPath))
                {
                    using (var writer = new StreamWriter(_flightsPath))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<FlightMap>();
                        csv.WriteHeader<Flight>();
                        csv.NextRecord();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Database Manager: {ex.ToString()}");
            }
        }
    }
}
