using AirportTicketBookingExercise.Domain.Models;
using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Models;
using ATB.Logic.Enums;
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

        public List<Flight> FilterFlights (BookingFilter filter)
        {
            var flights = GetFlights();
            var query =
                from flight in flights
                where
                    (filter.FlightName == null || filter.FlightName.Equals(filter.FlightName, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.DepartureCountry == null || flight.DepartureCountry.Equals(filter.DepartureCountry, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.DestinationCountry == null || flight.DestinationCountry.Equals(filter.DestinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.DepartureDate == null || flight.DepartureDate.Date == filter.DepartureDate.Value.Date) &&
                    (filter.DepartureAirport == null || flight.DepartureAirport.Equals(filter.DepartureAirport, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.ArrivalAirport == null || flight.ArrivalAirport.Equals(filter.ArrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                    (filter.Price == null || (flight.BuisnessPrice == filter.Price ||
                        flight.EconomyPrice == filter.Price ||
                        flight.FirstClassPrice == filter.Price))
                select flight;

            return query.ToList();
        }
    }
}
