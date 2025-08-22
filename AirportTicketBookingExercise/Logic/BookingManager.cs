using ATB.Logic.Handlers.Command;
using ATB.Logic.Enums;

namespace ATB.Logic
{
    public class BookingManager
    {

        private PassengerCommandHandler _passengerCommandHandler;
        private ManagerCommandHandler _managerCommandHnadler;

        public BookingManager(
            ManagerCommandHandler managerCommndHandler, PassengerCommandHandler passengerCommandHandler)
        {
            _passengerCommandHandler = passengerCommandHandler;
            _managerCommandHnadler = managerCommndHandler;
        }

        public void processInput(string input)
        {
            string[] line = input.Split(' ');
            ManagerCommand managerCommand = ManagerCommands.ParseManagerCommand(line[0]);
            PassengerCommand passengerCommand = PassengerCommands.ParsePassengerCommand(line[0]);
            if (passengerCommand != PassengerCommand.None)
                _passengerCommandHandler.ExecutePassengerCommand(line, passengerCommand);
            else if (managerCommand != ManagerCommand.None)
                _managerCommandHnadler.executeManagerCommand(line, managerCommand);
            else Console.WriteLine("\n Please enter an appropriate action");
        }


    }

}

