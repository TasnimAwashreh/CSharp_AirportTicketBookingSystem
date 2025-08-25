using ATB.Data.Repository;
using ATB.Logic.Handlers.Command;
using ATB.Logic.Service;
using Microsoft.Extensions.DependencyInjection;
using ATB.Data.Db;
using ATB.App;



namespace ATB.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            const string DbFile = "BookingDb.db";
            string connectionString = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", DbFile);

            const string csvFile = "Flights.csv";
            string filghtsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", csvFile);

            services
                    .AddSingleton<DatabaseManager>(_ => new DatabaseManager(connectionString))
                    .AddScoped<IUserRepository>(_ => new UserRepository(connectionString))
                    .AddScoped<IBookingRepository>(_ => new BookingRepository(connectionString))
                    .AddScoped<IFlightRepository>(_ => new FlightRepository(filghtsFilePath));
                    

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IFlightservice, Flightservice>()
                .AddScoped<IBookingService, BookingService>();
            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services
                .AddScoped<PassengerCommandHandler>()
                .AddScoped<ManagerCommandHandler>();

            return services;
        }
        public static IServiceCollection AddManager(this IServiceCollection services)
        {
            services.AddScoped<BookingManager>();
            return services;
        }

    }
}
