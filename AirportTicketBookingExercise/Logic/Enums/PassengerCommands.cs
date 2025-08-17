
namespace ATB.Logic.Enums
{
    public enum PassengerCommand
    {
        signup,
        login,
        logout,
        book,
        search,
        cancel,
        modify,
        flights,
        bookings,
        none
    }
    public class PassengerCommands
    {
        public static PassengerCommand GetPassengerCommand(string command)
        {
            switch (command.ToLower())
            {
                case "signup":
                    return PassengerCommand.signup;
                case "login":
                    return PassengerCommand.login;
                case "logout":
                    return PassengerCommand.logout;
                case "book":
                    return PassengerCommand.book;
                case "search":
                    return PassengerCommand.search;
                case "cancel":
                    return PassengerCommand.cancel;
                case "modify":
                    return PassengerCommand.modify;
                case "flights":
                    return PassengerCommand.flights;
                case "bookings":
                    return PassengerCommand.bookings;
                default:
                    return PassengerCommand.none;
            }
        }
    }
}
