using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Models;

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
            CsvActionsHelper.CreateCSVFile<User, UserMap>(_usersPath);
            CsvActionsHelper.CreateCSVFile<Booking, BookingMap>(_bookingsPath);
            CsvActionsHelper.CreateCSVFile<Flight, FlightMap>(_flightsPath);
            
        }
    }
}
