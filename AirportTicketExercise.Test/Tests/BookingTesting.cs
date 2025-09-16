using AirportTicketBookingExercise.Data.Repository;
using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Enums;
using ATB.Logic.Service;
using FluentAssertions;
using Moq;

namespace AirportTicketExercise.Test.Tests
{
    [Collection("Booking Service")]
    public class BookingTesting
    {

        [Fact]
        public void BookFlight_WithValidData_ShouldBookFlight()
        {
            // Arrange
            var user = DummyData.ValidUser1;
            var flight = DummyData.ValidFlight1;
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockFlightRepository.Setup(r => r.GetFlight(1)).Returns(flight);
            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            // Act
            service.PassengerBookFlight(1, BookingClass.Economy, user);

            ///Assert
            mockBookingRepository.Verify(r => r.CreateBooking(It.Is<Booking>(b =>
                b.FlightId == 1 &&
                b.PassengerId == user.UserId &&
                b.BookingClass == BookingClass.Economy
            )), Times.Once);
        }

        [Fact]
        public void BookFlight_WithNonexistentFlight_ShouldCauseException()
        {
            // Arrange
            var user = DummyData.ValidUser1;
            var flight = DummyData.ValidFlight1;
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.PassengerBookFlight(1, BookingClass.Economy, user)
            );
            mockBookingRepository.Verify(r => r.CreateBooking(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void BookFlight_WithFullSeats_ShouldCauseException()
        {
            // Arrange
            var user = DummyData.ValidUser1;
            var flight = DummyData.ValidFlightFullSeats;
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockFlightRepository.Setup(r => r.GetFlight(1)).Returns(flight);
            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.PassengerBookFlight(1, BookingClass.Economy, user)
            );
            mockBookingRepository.Verify(r => r.CreateBooking(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void BookFlight_WithNoSpecificClass_ShouldCauseException()
        {
            // Arrange
            var user = DummyData.ValidUser1;
            var flight = DummyData.ValidFlightEmptyEconomyPrice;
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockFlightRepository.Setup(r => r.GetFlight(1)).Returns(flight);
            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                service.PassengerBookFlight(1, BookingClass.Economy, user)
            );
            mockBookingRepository.Verify(r => r.CreateBooking(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void GetAllBookings_ShouldReturnAllBookings()
        {
            // Arrange
            User user1 = DummyData.ValidUser1;
            User user2 = DummyData.ValidUser2;
            Flight flight1 = DummyData.ValidFlight1;
            Flight flight2 = DummyData.ValidFlight2;

            var expectedBookingList = new List<Booking>
            {
                new Booking { FlightId = flight1.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.Economy },
                new Booking { FlightId = flight2.FlightId, PassengerId = user2.UserId, BookingClass = BookingClass.Business }
            };

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository.Setup(r => r.GetAllBookings()).Returns(expectedBookingList);

            var service = new BookingService(
                    mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act
            List<Booking> actualBookingList = service.GetAllBookings();

            //Assert
            Assert.Equal(expectedBookingList, actualBookingList);
            mockBookingRepository.Verify(r => r.GetAllBookings(), Times.Once);
        }

        [Fact]
        public void GetUserBookings_ShouldReturnUserBookings()
        {
            // Arrange
            User user1 = DummyData.ValidUser1;
            User user2 = DummyData.ValidUser2;
            Flight flight1 = DummyData.ValidFlight1;
            Flight flight2 = DummyData.ValidFlight2;

            var expectedBookingList = new List<Booking>
            {
                new Booking { FlightId = flight1.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.Economy },
                new Booking { FlightId = flight2.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.First },
                new Booking { FlightId = flight1.FlightId, PassengerId = user2.UserId, BookingClass = BookingClass.Economy },
                new Booking { FlightId = flight2.FlightId, PassengerId = user2.UserId, BookingClass = BookingClass.Business },
                new Booking { FlightId = flight1.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.First }
            };
            var expectedFilteredList = expectedBookingList.Where(booking => booking.PassengerId == user1.UserId).ToList();

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository.Setup(r => r.GetBookings(user1.UserId))
                                 .Returns(expectedFilteredList);

            var service = new BookingService(
                    mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act
            List<Booking> actualBookingList = service.GetBookings(user1);

            //Assert
            Assert.Equal(expectedFilteredList, actualBookingList);
            mockBookingRepository.Verify(r => r.GetBookings(user1.UserId), Times.Once);
        }

        [Fact]
        public void CheckIfBookingIsValidById_WithValidBooking_ShouldReturnTrue()
        {
            // Arrange
            bool expectedIsValid = true;
            User user1 = DummyData.ValidUser1;
            User user2 = DummyData.ValidUser2;
            Flight flight1 = DummyData.ValidFlight1;
            Flight flight2 = DummyData.ValidFlight2;

            var expectedBookingList = new List<Booking>
            {
                new Booking { FlightId = flight1.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.Economy },
                new Booking { FlightId = flight2.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.First },
                new Booking { FlightId = flight1.FlightId, PassengerId = user2.UserId, BookingClass = BookingClass.Economy },
                new Booking { FlightId = flight2.FlightId, PassengerId = user2.UserId, BookingClass = BookingClass.Business },
                new Booking { FlightId = flight1.FlightId, PassengerId = user1.UserId, BookingClass = BookingClass.First }
            };

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository.Setup(r => r.IsBookingValidById(expectedBookingList[0].BookingId, user1.UserId))
                                 .Returns(true);

            var service = new BookingService(
                    mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            ///Act
            var actualIsValid = service.IsBookingValidById(expectedBookingList[0].BookingId, user1.UserId);

            //Assert
            Assert.Equal(expectedIsValid, actualIsValid);
            mockBookingRepository.Verify(r => r.IsBookingValidById(expectedBookingList[0].BookingId, user1.UserId), Times.Once);
        }

        [Fact]
        public void UpdateBookingClass_WithValidBooking_ShouldReturnTrue()
        {
            //arrange
            int bookingId = 42;
            BookingClass newClass = BookingClass.Business;

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository
                .Setup(r => r.UpdateBookingClass(bookingId, newClass))
                .Returns(true);

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            //Act
            bool result = service.UpdateBookingClass(bookingId, newClass);

            //Assert
            Assert.True(result);
            mockBookingRepository.Verify(
                r => r.UpdateBookingClass(bookingId, newClass),
                Times.Once);
        }

        [Fact]
        public void RemoveBookingById_WithExistingBooking_ShouldReturnTrue()
        {
            //Arrange
            int bookingId = 123;

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository
                .Setup(r => r.DeleteBooking(bookingId))
                .Returns(true);

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            //Act
            bool result = service.RemoveBookingById(bookingId);

            //Assert
            Assert.True(result);
            mockBookingRepository.Verify(r => r.DeleteBooking(bookingId), Times.Once);
        }


        [Fact]
        public void CancelBooking_WithValidBooking_ShouldCancelBooking()
        {
            //Arrange
            int bookingId = 123;
            var user = DummyData.ValidUser1;

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository
                .Setup(r => r.IsBookingValidById(bookingId, user.UserId))
                .Returns(true);
            mockBookingRepository
                .Setup(r => r.DeleteBooking(bookingId))
                .Returns(true);

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            // Act
            var ex = Record.Exception(() => service.Cancel(bookingId, user));

            //Assert
            Assert.Null(ex);
            mockBookingRepository.Verify(r => r.DeleteBooking(bookingId), Times.Once);
        }

        [Fact]
        public void CancelBooking_WithInvalidBooking_ShouldThrowException()
        {
            //Arrange
            int bookingId = 123;
            var user = DummyData.ValidUser1;

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);
            mockBookingRepository
                .Setup(r => r.DeleteBooking(bookingId))
                .Returns(false);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
                 service.Cancel(bookingId, user)
            );
        }

        [Fact]
        public void Modify_WithValidBooking_ShouldUpdateBooking()
        {
            //Arrange
            int bookingId = 50;
            BookingClass bookingClass = BookingClass.First;
            User user = DummyData.ValidUser1;

            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository.Setup(r => r.IsBookingValidById(bookingId, user.UserId))
                                .Returns(true);
            mockBookingRepository.Setup(r => r.UpdateBookingClass(bookingId, bookingClass))
                                 .Returns(true);

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            //act
            service.Modify(bookingId, bookingClass, user);

            //Assert
            mockBookingRepository.Verify(r => r.UpdateBookingClass(bookingId, bookingClass), Times.Once);
        }

        [Fact]
        public void Modify_WithInvalidBookingId_ShouldThrowException()
        {
            //Arrange
            int bookingId = 50;
            BookingClass bookingClass = BookingClass.First;
            User user = DummyData.ValidUser1;

            var mockBookingRepository = new Mock<IBookingRepository>();
            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockBookingFilterRepository = new Mock<IBookingsFilterRepository>();

            mockBookingRepository.Setup(r => r.UpdateBookingClass(bookingId, bookingClass))
                                 .Returns(false);

            var service = new BookingService(
                mockBookingRepository.Object, mockFlightRepository.Object, mockBookingFilterRepository.Object);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Modify(bookingId, BookingClass.Business, user));
        }
    }
}
