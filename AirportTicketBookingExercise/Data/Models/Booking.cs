using ATB.Logic.Enums;

namespace ATB.Data.Models
{
    public class Booking
    {
        public int BookingId;
        public int FlightId;
        public int PassengerId;
        public BookingClass BookingClass;

        public string ToString()
        {
            return $"{BookingId}| Flight Id: {this.FlightId}, Class: {this.BookingClass}";
        }

    }

}
