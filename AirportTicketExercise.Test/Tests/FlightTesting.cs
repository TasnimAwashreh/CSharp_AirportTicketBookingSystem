using AirportTicketBookingExercise.Domain.Models;
using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Service;
using FluentAssertions;
using Moq;

namespace AirportTicketExercise.Test.Tests
{
    [Collection("Flight Service")]
    public class FlightTesting
    {
        [Fact]
        public void GetAllFlights_WithValidData_ShouldReturnAllFlights()
        {
            var expectedFlights = DummyData.ValidFlights;
            var mockRepo = new Mock<IFlightRepository>();
            mockRepo.Setup(r => r.GetFlights()).Returns(expectedFlights);
            var service = new FlightService(mockRepo.Object);

            var actualFlights = service.GetFlights();

            Assert.Equal(expectedFlights, actualFlights);
        }

        [Fact]
        public void ImportFlightData_WithValidFlightsFromCSV_ShouldReturnImportedFlights()
        {
            //Arrange
            var expectedFlights = DummyData.ValidFlights;

            var mockRepo = new Mock<IFlightRepository>();
            var service = new FlightService(mockRepo.Object);

            var fakeImportCSVFile = new FakeImportCSV();
            fakeImportCSVFile.InsertFlights(expectedFlights);

            //Act
            List<Flight> actualFlights = service.ImportFlightData(fakeImportCSVFile.importFlightPath);

            //Assert
            Assert.Equal(expectedFlights.Count, actualFlights.Count);
        }

        [Fact]
        public void ImportFlightData_WithInvalidFlightsFromCSV_ShouldReturnException()
        {
            //Arrange
            var expectedFlights = DummyData.InValidFlights;

            var mockRepo = new Mock<IFlightRepository>();
            var service = new FlightService(mockRepo.Object);

            var fakeImportCSVFile = new FakeImportCSV();
            fakeImportCSVFile.InsertFlights(expectedFlights);

            //Act & Assert
            Assert.Throws<FormatException>(() =>
                service.ImportFlightData(fakeImportCSVFile.importFlightPath)
            );
        }

        [Fact]
        public void ValidateImportedFlightData_WithValidFlightsFromCSV_ShouldReturnEmptyErrorString()
        {
            //Arrange
            var flights = DummyData.ValidFlights;

            var mockRepo = new Mock<IFlightRepository>();
            var service = new FlightService(mockRepo.Object);

            var fakeImportCSVFile = new FakeImportCSV();
            fakeImportCSVFile.InsertFlights(flights);

            //Act
            string result = service.ValidateFlightData(fakeImportCSVFile.importFlightPath);

            //Assert
            Assert.True(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void ValidateImportedFlightData_WithInvalidFlightsFromCSV_ShouldReturnErrorString()
        {
            //Arrange
            var flights = DummyData.InValidFlights;

            var mockRepo = new Mock<IFlightRepository>();
            var service = new FlightService(mockRepo.Object);

            var fakeImportCSVFile = new FakeImportCSV();
            fakeImportCSVFile.InsertFlights(flights);

            //Act
            string result = service.ValidateFlightData(fakeImportCSVFile.importFlightPath);

            //Assert
            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void Search_WithValidParamsAndFlights_ShouldReturnFilteredFlights()
        {
            //Arrange
            string param = "flight";
            string value = "SkyJet 101";
            string[] searchInput = { "search", $"{param}={value}" };

            var flights = DummyData.ValidFlights;
            var expectedFlights = flights.Where(f => f.FlightName == value).ToList();

            var mockRepo = new Mock<IFlightRepository>();
            mockRepo.Setup(r => r.FilterFlights(It.IsAny<BookingFilter>())).Returns(expectedFlights);

            var service = new FlightService(mockRepo.Object);

            //Act
            var actualFlights = service.Search(searchInput);

            //Assert
            Assert.Equal(expectedFlights.Count, actualFlights.Count);
        }
    }
}
