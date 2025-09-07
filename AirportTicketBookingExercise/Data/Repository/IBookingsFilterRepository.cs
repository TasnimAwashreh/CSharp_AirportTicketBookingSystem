using ATB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Data.Repository
{
    public interface IBookingsFilterRepository
    {
        public List<Booking> FilterBookingsWithFlights(string[] filterInput);
    }
}
