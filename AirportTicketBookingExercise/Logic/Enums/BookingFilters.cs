using AirportTicketBookingExercise.Data;
using ATB.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Logic.Enums
{

    public enum FilterParam
    {
        flight,
        price,
        departure_country,
        destination_country,
        departure_date,
        departure_airport,
        arrival_airport,
        flight_class,
        passenger,
        none
    }
    public class BookingFilters
    {
        public static FilterParam GetFilterParam(string command)
        {
            switch (command.ToLower())
            {
                case "flight":
                    return FilterParam.flight;
                case "price":
                    return FilterParam.price;
                case "departure_country":
                    return FilterParam.departure_country;
                case "destination_country":
                    return FilterParam.destination_country;
                case "departure_date":
                    return FilterParam.departure_date;
                case "departure_airport":
                    return FilterParam.departure_airport;
                case "arrival_airport":
                    return FilterParam.arrival_airport;
                case "flight_class":
                    return FilterParam.flight_class;
                case "passenger":
                    return FilterParam.passenger;
                default:
                    return FilterParam.none;
            }


        }
        public static BookingFilter Parse(string[] parts)
        {
            var query = new BookingFilter();

            foreach (var part in parts)
            {
                var keyValue = part.Split('=', 2, StringSplitOptions.TrimEntries);
                if (keyValue.Length != 2) continue;

                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();

                var filter = BookingFilters.GetFilterParam(key);

                switch (filter)
                {
                    case FilterParam.flight:
                        query.FlightName = value;
                        break;
                    case FilterParam.price:
                        if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var price))
                            query.Price = price;
                        break;
                    case FilterParam.departure_country:
                        query.DepartureCountry = value;
                        break;
                    case FilterParam.destination_country:
                        query.DestinationCountry = value;
                        break;
                    case FilterParam.departure_date:
                        if (DateTime.TryParse(value, out var date))
                            query.DepartureDate = date;
                        break;
                    case FilterParam.departure_airport:
                        query.DepartureAirport = value;
                        break;
                    case FilterParam.arrival_airport:
                        query.ArrivalAirport = value;
                        break;
                    case FilterParam.flight_class:
                        query.BookingClass = value;
                        break;
                    case FilterParam.passenger:
                        query.PassengerName = value;
                        break;
                    case FilterParam.none:
                    default:
                        break;
                }
            }

            return query;
        }
    }
 
}
