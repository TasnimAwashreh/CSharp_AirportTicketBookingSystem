using ATB.Data.Repository;
using ATB.Data.Models;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ATB.Logic.Service
{
    public class Flightservice : IFlightservice
    {
        private IFlightRepository _flightRepo;

        public Flightservice(IFlightRepository fightRepo)
        {
            _flightRepo = fightRepo;
        }

        public List<Flight> GetFlights()
        {
            return _flightRepo.GetFlights();
        }

        public Flight? GetFlight(int flightId)
        {
            return _flightRepo.GetFlights()
                        .Find(flight => flight.FlightId == flightId);
        }

        public void AddPassengerToSeat(Flight flight)
        {
            _flightRepo.AddPassengerToSeat(flight);
        }

        public void RemovePassengerToSeat(Flight flight)
        {
            _flightRepo.RemovePassengerFromSeat(flight);
        }

        public string ValidateFlightData(string importPath)
        {
            var strBuilder = new StringBuilder("");
            int rowCount = 1;
            var flights = new List<Flight>();
            using (TextFieldParser csvParser = new TextFieldParser(importPath))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    Flight flight = AddFlight(fields);
                    if (flight == null)
                        strBuilder.AppendLine($"Row {rowCount}: Please fix the fields formats");
                    else
                    {
                        var context = new ValidationContext(flight, null, null);
                        var validationResults = new List<ValidationResult>();
                        bool isFieldValid = Validator.TryValidateObject(flight, context, validationResults, true);

                        if (!isFieldValid)
                        {
                            foreach (var vr in validationResults)
                                strBuilder.AppendLine($"Row {rowCount}: {vr.ErrorMessage}");
                        }
                    }
                    rowCount++;
                }
            }
            return strBuilder.ToString();
        }

        public bool ImportFlightData(string importPath)
        {
            List<Flight> importedFlightData = new List<Flight>();
            using (TextFieldParser csvParser = new TextFieldParser(importPath))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                try
                {
                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        Flight flight = AddFlight(fields);
                        if (flight == null)
                        {
                            return false;
                        }
                        importedFlightData.Add(flight);
                    }
                    _flightRepo.AddFlights(importedFlightData);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while importing flight data: {ex.ToString()}");
                    return false;
                }
            }
        }

        private Flight AddFlight(string[] fields)
        {
            try
            {
                var flight = new Flight
                {
                    FlightId = int.Parse(fields[0]),
                    FlightName = fields[1],
                    DepartureCountry = fields[2],
                    DestinationCountry = fields[3],
                    DepartureAirport = fields[4],
                    ArrivalAirport = fields[5],
                    DepartureDate = DateTime.ParseExact(
                                        fields[6],
                                        new[] { "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy" },
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None),
                    EconomyPrice = Decimal.Parse(fields[7]),
                    BuisnessPrice = Decimal.Parse(fields[8]),
                    FirstClassPrice = Decimal.Parse(fields[9]),
                    SeatCapacity = int.Parse(fields[10])
                };
                return flight;
            }
            catch
            {
                return null;
            }

        }
    }
}
