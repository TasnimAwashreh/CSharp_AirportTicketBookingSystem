
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

        private void ValidateField<T>(string fieldName, string fieldInput,
                Func<string, T> parseValue, StringBuilder strBuilder, int rowCount)
        {
            PropertyInfo property = typeof(Flight).GetProperty(fieldName);
            var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
            try
            {
                T value;
                if (typeof(T) == typeof(DateTime))
                    DateTime.ParseExact(fieldInput, "dd/MM/yy", CultureInfo.InvariantCulture);
                else
                    value = parseValue(fieldInput);
                foreach (ValidationAttribute attr in attributes)
                {
                    if (!attr.IsValid(fieldInput))
                        strBuilder.Append($"Row {rowCount}: {attr.ErrorMessage} \n");
                }
            }
            catch (Exception e)
            {
                strBuilder.Append($"Error at row {rowCount}: Wrong Format for field {fieldName} \n");
            }
        }

        public string ValidateFlightData(string importPath)
        {
            using (TextFieldParser csvParser = new TextFieldParser(importPath))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                StringBuilder strBuilder = new StringBuilder("");
                int rowCount = 1;
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    ValidateField("FlightId", fields[0], int.Parse, strBuilder, rowCount);
                    ValidateField("FlightName", fields[1], x => x, strBuilder, rowCount);
                    ValidateField("DepartureCountry", fields[2], x => x, strBuilder, rowCount);
                    ValidateField("DestinationCountry", fields[3], x => x, strBuilder, rowCount);
                    ValidateField("DepartureDate", fields[4], DateTime.Parse, strBuilder, rowCount);
                    ValidateField("DepartureAirport", fields[5], x => x, strBuilder, rowCount);
                    ValidateField("ArrivalAirport", fields[6], x => x, strBuilder, rowCount);
                    ValidateField("EconomyPrice", fields[7], Decimal.Parse, strBuilder, rowCount);
                    ValidateField("BuisnessPrice", fields[8], Decimal.Parse, strBuilder, rowCount);
                    ValidateField("FirstClassPrice", fields[9], Decimal.Parse, strBuilder, rowCount);
                    ValidateField("SeatCapacity", fields[10], int.Parse, strBuilder, rowCount);
                    rowCount++;
                }
                return strBuilder.ToString();
            }
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
                        Flight flight = new Flight
                        {
                            FlightId = int.Parse(fields[0]),
                            FlightName = fields[1],
                            DepartureCountry = fields[2],
                            DestinationCountry = fields[3],
                            DepartureDate = DateTime.ParseExact(fields[4], "dd/MM/yy", CultureInfo.InvariantCulture),
                            DepartureAirport = fields[5],
                            ArrivalAirport = fields[6],
                            EconomyPrice = Decimal.Parse(fields[7]),
                            BuisnessPrice = Decimal.Parse(fields[8]),
                            FirstClassPrice = Decimal.Parse(fields[9]),
                            SeatCapacity = int.Parse(fields[10])
                        };
                        importedFlightData.Add(flight);
                    }
                    _flightRepo.AddFlights(importedFlightData);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
