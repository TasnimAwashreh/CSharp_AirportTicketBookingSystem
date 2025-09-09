using AirportTicketBookingExercise.App.Commands.Enums;
using AirportTicketBookingExercise.App.Commands.Helpers;
using ATB.Data.Models;

namespace AirportTicketBookingExercise.App.Commands.CommandExecuter
{
    public class ExecuteManagerCommands : ICommandExecuter<ManagerCommand>
    {
        private ManagerHelper _managerHelper;

        public ExecuteManagerCommands(ManagerHelper managerHelper)
        {
            _managerHelper = managerHelper;
        }

        public User? ExecuteCommand(User? loggedInUser, string[] productInfo, ManagerCommand command)
        {
            if (loggedInUser == null)
            {
                switch (command)
                {
                    case ManagerCommand.ManagerSignUp:
                        _managerHelper.ManagerSignUp(productInfo);
                        return loggedInUser;
                    case ManagerCommand.ManagerLogIn:
                        return _managerHelper.ManagerLogin(productInfo);
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        return loggedInUser;
                    default:
                        Console.WriteLine("Please enter an appropriate command.");
                        return loggedInUser;
                }
            }
            else if (loggedInUser.UserType == UserType.Passenger)
            {
                                Console.WriteLine("Only managers can use these commands! Please log out as a passenger and log back in as a manager");
                return loggedInUser;
            }
            else
            {
                switch (command)
                {
                    case ManagerCommand.ManagerLogOut:
                        Console.WriteLine("Logged out successfully! See you soon, manager!");
                        return null;
                    case ManagerCommand.Upload:
                        _managerHelper.Upload(productInfo);
                        return loggedInUser;
                    case ManagerCommand.Validate:
                        _managerHelper.ValidateCSV(productInfo);
                        return loggedInUser;
                    case ManagerCommand.Filter:
                        _managerHelper.Filter(productInfo);
                        return loggedInUser;
                    case ManagerCommand.ManagerSignUp:
                        Console.WriteLine("Please log out first to sign up");
                        return loggedInUser;
                    case ManagerCommand.ManagerLogIn:
                        Console.WriteLine($"You are already logged in, {loggedInUser.Name}!");
                        return loggedInUser;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        return loggedInUser;
                }
            }
            return loggedInUser;
        }
    }
}
