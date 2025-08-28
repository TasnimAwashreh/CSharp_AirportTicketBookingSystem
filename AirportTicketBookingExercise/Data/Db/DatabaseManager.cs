using ATB.Data.Models;
using CsvHelper;
using CsvHelper.Configuration;
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
                CreateCSVFile<User, UserMap>(_usersPath);
                CreateCSVFile<Booking, BookingMap>(_bookingsPath);
                CreateCSVFile<Flight, FlightMap>(_flightsPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Please fix Create Database error (below) and restart the system. \nError: {ex}");
                return;
            }
        }

        private void CreateCSVFile<TModel, TMap>(string path)
            where TMap : ClassMap<TModel>
        {
            if (File.Exists(path))
                return;

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TMap>();
                csv.WriteHeader<TModel>();
                csv.NextRecord();
            }
        }
    }
}
