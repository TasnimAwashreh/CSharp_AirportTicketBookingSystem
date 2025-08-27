using ATB.Data.Models;
using CsvHelper;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ATB.Data.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _flightsCSVPath;
        private List<Flight> _flights;

        public FlightRepository(string flightsCSVPath)
        {
            _flightsCSVPath = flightsCSVPath;
        }

        public List<Flight> GetFlights()
        {
            using (var reader = new StreamReader(_flightsCSVPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<FlightMap>();
                var records = csv.GetRecords<Flight>().ToList();
                return records;
            }
        }

        public void AddPassengerToSeat(Flight flight)
        {
            flight.SeatsAvailable++;
        }

        public void RemovePassengerFromSeat(Flight flight)
        {
            flight.SeatsAvailable--;
        }

        public bool AddFlights(List<Flight> flights)
        {
            try
            {
                using (var writer = new StreamWriter(_flightsCSVPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<BookingMap>();
                    csv.WriteRecords(flights);
                    return true;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while trying to add flights: {ex.ToString()}");
                return false;
            }
        }
    }
}
