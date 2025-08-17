using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Helper
{
    public static  class CommandParser
    {
        public static string[] ParseCommand(string input)
        {
            string[] line = input.Split(' ');
            return line;

        }
    }
}
