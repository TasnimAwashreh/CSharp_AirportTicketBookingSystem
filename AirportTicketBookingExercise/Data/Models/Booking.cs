using ATB.Logic.Enums;
using ATB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return $"Flight Id: {this.FlightId}, Class: {this.BookingClass}";
        }

    }

}
