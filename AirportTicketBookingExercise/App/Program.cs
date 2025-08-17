
using AirportTicketBookingExercise.Data.Db.DAOs;
using AirportTicketBookingExercise.Extensions;
using AirportTicketBookingExercise.Logic.Handlers.Command;
using ATB.Data.Repository;
using ATB.Logic;
using ATB.Logic.Enums;
using ATB.Logic.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{

    public static void startLoop(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
        databaseManager.CreateDatabase();
        var manager = scope.ServiceProvider.GetRequiredService<BookingManager>();


        while (true)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Empty input: Please try again");
            else
                manager.processInput(input);
        }
    }

    public static string introduction()
    {
        string controls =
            $"""
            ============================================================================================
            **                                                                                        **
            **                           Welcome to the Airport Ticket Booking System                 **
            **                                                                                        **
            ============================================================================================

            """;
        return controls;
    }
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddDaos()
            .AddRepositories()
            .AddServices()
            .AddCommandHandlers()
            .AddManager();

        Console.WriteLine(introduction());
        var serviceProvider = serviceCollection.BuildServiceProvider();
        startLoop(serviceProvider);
    }

    private static void processInput(BookingManager manager, string managerInput, ManagerCommandHandler managerCommandHnadler)
    {
        string[] productInfo = managerInput.Split(' ');
        ManagerCommand command = ManagerCommands.GetManagerCommand(productInfo[0]);
        managerCommandHnadler.executeManagerCommand(productInfo, command);
    }
}
