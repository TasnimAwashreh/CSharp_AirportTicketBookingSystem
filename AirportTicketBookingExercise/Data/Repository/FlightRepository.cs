using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Models;
using ATB.Logic;
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

        public List<Flight> Search(FilterParam searchParam, string valueParam)
        {
            List<Flight> filteredFlights = new List<Flight>();
            var flights = GetFlights();
            filteredFlights = searchParam switch
            {
                FilterParam.Flight => flights.Where(f => f.FlightName.Equals(valueParam)).ToList(),
                FilterParam.Price => flights.Where(f =>
                    f.BuisnessPrice == decimal.Parse(valueParam) ||
                    f.EconomyPrice == decimal.Parse(valueParam) ||
                    f.FirstClassPrice == decimal.Parse(valueParam)).ToList(),
                FilterParam.DepartureCountry => flights.Where(f => f.DepartureCountry.Equals(valueParam)).ToList(),
                FilterParam.DestinationCountry => flights.Where(f => f.DestinationCountry.Equals(valueParam)).ToList(),
                FilterParam.DepartureDate => flights.Where(f => f.DepartureDate.Equals(valueParam)).ToList(),
                FilterParam.DepartureAirport => flights.Where(f => f.DepartureAirport.Equals(valueParam)).ToList(),
                FilterParam.ArrivalAirport => flights.Where(f => f.ArrivalAirport.Equals(valueParam)).ToList(),
                _ => []
            };
            return filteredFlights;
        }

        public List<Flight> FilterFlights (BookingFilter filter)
        {
            var flights = GetFlights();
            var query =
                from flight in flights
                where
                    (filter.FlightName == null || flight.FlightName.Contains(filter.FlightName)) &&
                    (filter.DepartureCountry == null || flight.DepartureCountry == filter.DepartureCountry) &&
                    (filter.DestinationCountry == null || flight.DestinationCountry == filter.DestinationCountry) &&
                    (filter.DepartureDate == null || flight.DepartureDate.Date == filter.DepartureDate.Value.Date) &&
                    (filter.DepartureAirport == null || flight.DepartureAirport == filter.DepartureAirport) &&
                    (filter.ArrivalAirport == null || flight.ArrivalAirport == filter.ArrivalAirport)
                select flight;

            return query.ToList();
        }
    }
}
