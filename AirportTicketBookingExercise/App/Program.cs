using ATB.App.Configuration;
using ATB.App;
using ATB.Data.Db;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    public static void StartLoop(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
        databaseManager.CreateDatabase();
        var BookingManager = scope.ServiceProvider.GetRequiredService<BookingManager>();

        while (true)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Empty input: Please try again");
            else
                BookingManager.ProcessInput(input);
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
            .AddRepositories()
            .AddServices()
            .AddCommandHandlers()
            .AddManager();

        Console.WriteLine(introduction());
        var serviceProvider = serviceCollection.BuildServiceProvider();
        StartLoop(serviceProvider);
    }
}
