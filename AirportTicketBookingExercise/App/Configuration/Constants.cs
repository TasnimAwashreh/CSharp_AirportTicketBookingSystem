using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.App.Configuration
{
    public static class Constants
    {
        private const string _bookingFile = "bookings.csv";
        private const string _userFile = "users.csv";
        private const string _flightsFile = "flights.csv";
        private const string _testImportedFlightsFile = "importedflights.csv";

        public static readonly string BookingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", _bookingFile);
        public static readonly string UsersPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", _userFile);
        public static readonly string FlightsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", _flightsFile);

        public static readonly string TestBookingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Db", _bookingFile);
        public static readonly string TestUsersPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Db", _userFile);
        public static readonly string TestFlightsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Db", _flightsFile);
        public static readonly string TestImportedFlightsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Db", _testImportedFlightsFile);
    }
}
