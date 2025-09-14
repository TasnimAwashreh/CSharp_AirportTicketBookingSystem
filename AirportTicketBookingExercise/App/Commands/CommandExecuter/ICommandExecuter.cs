using ATB.Data.Models;

namespace AirportTicketBookingExercise.App.Commands.CommandExecuter
{
    public interface ICommandExecuter<T>
    {
        User? ExecuteCommand(User? loggedInUser, string[] args, T command);
    }
}
