using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Models;
using CsvHelper;
using System.Globalization;

namespace ATB.Data.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _flightsCSVPath;

        public FlightRepository(string flightsCSVPath)
        {
            _flightsCSVPath = flightsCSVPath;
        }

        public List<Flight> GetFlights()
        {
            return CsvActionsHelper.GetAllRecords<Flight, FlightMap>(_flightsCSVPath);
        }

        public Flight? GetFlight(int flightId)
        {
            return GetFlights()
                    .Find(flight => flight.FlightId == flightId);
        }

        public void AddPassengerToSeat(Flight flight)
        {
            flight.SeatsAvailable++;
        }

        public void RemovePassengerFromSeat(Flight flight)
        {
            flight.SeatsAvailable--;
        }

        public void AddFlights(List<Flight> flights)
        {
            CsvActionsHelper.CreateRecords<Flight, FlightMap>(_flightsCSVPath, flights);
        }
    }
}
