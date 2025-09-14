namespace AirportTicketBookingExercise.App.Commands.Enums
{
    public enum ManagerCommand
    {
        None = 0,
        ManagerLogIn = 1,
        ManagerSignUp = 2,
        ManagerLogOut = 3,
        Validate = 4,
        Upload = 5,
        Filter = 6,

    }

    public static class ManagerCommands
    {
        public static ManagerCommand ParseManagerCommand(this string command)
        {
            switch (command.ToLower())
            {
                case "manager_signup":
                    return ManagerCommand.ManagerSignUp;
                case "manager_login":
                    return ManagerCommand.ManagerLogIn;
                case "manager_logout":
                    return ManagerCommand.ManagerLogOut;
                case "validate":
                    return ManagerCommand.Validate;
                case "upload":
                    return ManagerCommand.Upload;
                case "filter":
                    return ManagerCommand.Filter;
                default:
                    return ManagerCommand.None;
            }
        }
    }
}
