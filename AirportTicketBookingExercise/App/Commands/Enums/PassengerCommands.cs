namespace AirportTicketBookingExercise.App.Commands.Enums
{
    public enum PassengerCommand
    {
        None = 0,
        SignUp = 1,
        LogIn = 2,
        LogOut = 3,
        Book = 4,
        Search = 5,
        Cancel = 6,
        Modify = 7,
        Flights = 8,
        Bookings = 9,

    }

    public static class PassengerCommands
    {
        public static PassengerCommand ParsePassengerCommand(this string command)
        {
            switch (command.ToLower())
            {
                case "signup":
                    return PassengerCommand.SignUp;
                case "login":
                    return PassengerCommand.LogIn;
                case "logout":
                    return PassengerCommand.LogOut;
                case "book":
                    return PassengerCommand.Book;
                case "search":
                    return PassengerCommand.Search;
                case "cancel":
                    return PassengerCommand.Cancel;
                case "modify":
                    return PassengerCommand.Modify;
                case "flights":
                    return PassengerCommand.Flights;
                case "bookings":
                    return PassengerCommand.Bookings;
                default:
                    return PassengerCommand.None;
            }
        }
    }
}
