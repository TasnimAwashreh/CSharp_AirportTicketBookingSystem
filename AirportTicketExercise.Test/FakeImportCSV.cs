using AirportTicketBookingExercise.App.Configuration;
using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Models;
using CsvHelper;
using System.Globalization;

namespace AirportTicketExercise.Test
{
    public class FakeImportCSV : IDisposable
    {
        public string importFlightPath = Constants.TestImportedFlightsPath;

        public FakeImportCSV()
        {
            File.Delete(importFlightPath);
            CsvActionsHelper.CreateCSVFile<Flight, FlightMap>(importFlightPath);
        }

        public void InsertFlights(List<Flight> flights)
        {
            CsvActionsHelper.CreateRecords<Flight, FlightMap>(importFlightPath, flights);
        }
        public void Dispose()
        {
            if (File.Exists(importFlightPath))
                File.Delete(importFlightPath);
        }

    }
}
