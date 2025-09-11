using ATB.Data.Repository;
using ATB.Logic.Service;
using Microsoft.Extensions.DependencyInjection;
using ATB.Data.Db;
using AirportTicketBookingExercise.Data.Repository;
using AirportTicketBookingExercise.App.Commands.CommandExecuter;
using AirportTicketBookingExercise.App.Commands.Enums;
using AirportTicketBookingExercise.App.Commands.Helpers;
using AirportTicketBookingExercise.App;

namespace ATB.App.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services,
            string usersPath, string flightsPath, string bookingsPath)
        {

            services
                    .AddSingleton(_ => new DatabaseManager(usersPath, bookingsPath, flightsPath))
                    .AddScoped<IUserRepository>(_ => new UserRepository(usersPath))
                    .AddScoped<IBookingRepository>(_ => new BookingRepository(bookingsPath))
                    .AddScoped<IFlightRepository>(_ => new FlightRepository(flightsPath))
                    .AddScoped<IBookingsFilterRepository, BookingsFilterRepository>();
            
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

        public static IServiceCollection AddManager(this IServiceCollection services)
        {
            services.AddScoped<ManagerHelper>();
            services.AddScoped<PassengerHelper>();
            services.AddScoped<ICommandExecuter<ManagerCommand>, ExecuteManagerCommands>();
            services.AddScoped<ICommandExecuter<PassengerCommand>, ExecutePassengerCommands>();
            services.AddScoped<BookingManager>();
            return services;
        }
    }
}
