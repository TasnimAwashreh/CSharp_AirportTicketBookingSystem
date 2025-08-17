using AirportTicketBookingExercise.Data.Db.DAOs;
using AirportTicketBookingExercise.Data.Repository;
using AirportTicketBookingExercise.Logic.Handlers.Command;
using AirportTicketBookingExercise.Logic.Service;
using ATB.Data.Repository;
using ATB.Logic;
using ATB.Logic.Service;
using Microsoft.Extensions.DependencyInjection;


namespace AirportTicketBookingExercise.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDaos(this IServiceCollection services){

            const string DbFile = "bookingDb.db";
            string connectionString = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", DbFile);
            Console.WriteLine($"SQLite DB Path: {Path.GetFullPath(connectionString)}");

            services
                    .AddSingleton<UserDAO>(_ => new UserDAO(connectionString))
                     .AddSingleton<BookingDAO>(_ => new BookingDAO(connectionString))
                     .AddSingleton<DatabaseManager>(_ => new DatabaseManager(connectionString));

            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IFlightRepository, FlightRepository>()
                .AddScoped<IBookingRepository, BookingRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IFlightService, FlightService>()
                .AddScoped<IBookingService, BookingService>();

            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services
                .AddScoped<PassengerCommandHandler>()
                .AddScoped<ManagerCommandHnadler>();

            return services;
        }

        public static IServiceCollection AddManager(this IServiceCollection services)
        {
            services.AddScoped<BookingManager>();

            return services;

        }

    }
}
