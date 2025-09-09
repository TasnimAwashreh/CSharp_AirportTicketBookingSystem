using System.Globalization;

namespace ATB.Logic.Enums
{
    public enum FilterParam
    {
        None = 0,
        Flight = 1,
        Price = 2,
        DepartureCountry = 3,
        DestinationCountry = 4,
        DepartureDate = 5,
        DepartureAirport = 6,
        ArrivalAirport = 7,
        FlightClass = 8,
        Passenger = 9,
    }

    public static class BookingFilters
    {
        public static FilterParam ParseFilterParam(this string command)
        {
            switch (command.ToLower())
            {
                case "flight":
                    return FilterParam.Flight;
                case "price":
                    return FilterParam.Price;
                case "departure_country":
                    return FilterParam.DepartureCountry;
                case "destination_country":
                    return FilterParam.DestinationCountry;
                case "departure_date":
                    return FilterParam.DepartureDate;
                case "departure_airport":
                    return FilterParam.DepartureAirport;
                case "arrival_airport":
                    return FilterParam.ArrivalAirport;
                case "flight_class":
                    return FilterParam.FlightClass;
                case "passenger":
                    return FilterParam.Passenger;
                default:
                    return FilterParam.None;
            }
        }

        public static BookingFilter Parse(string[] parts)
        {
            var query = new BookingFilter();
            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                if (keyValue.Length != 2) continue;

                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();

                var Filter = key.ParseFilterParam();
                switch (Filter)
                {
                    case FilterParam.Flight:
                        query.FlightName = value;
                        break;
                    case FilterParam.Price:
                        if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var price))
                            query.Price = price;
                        break;
                    case FilterParam.DepartureCountry:
                        query.DepartureCountry = value;
                        break;
                    case FilterParam.DestinationCountry:
                        query.DestinationCountry = value;
                        break;
                    case FilterParam.DepartureDate:
                        if (DateTime.TryParse(value, out var date))
                            query.DepartureDate = date;
                        break;
                    case FilterParam.DepartureAirport:
                        query.DepartureAirport = value;
                        break;
                    case FilterParam.ArrivalAirport:
                        query.ArrivalAirport = value;
                        break;
                    case FilterParam.FlightClass:
                        query.BookingClass = value;
                        break;
                    case FilterParam.Passenger:
                        query.PassengerName = value;
                        break;
                    case FilterParam.None:
                    default:
                        break;
                }
            }
            return query;
        }
    }
}
