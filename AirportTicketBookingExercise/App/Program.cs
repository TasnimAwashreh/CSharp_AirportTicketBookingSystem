
using ATB.Data.Db.DAOs;
using ATB.Configuration;
using ATB.Logic;
using Microsoft.Extensions.DependencyInjection;

class Program
{

    public static void startLoop(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
        databaseManager.CreateDatabase();
        var bookingManager = scope.ServiceProvider.GetRequiredService<BookingManager>();


        while (true)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Empty input: Please try again");
            else
                bookingManager.processInput(input);
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
}
