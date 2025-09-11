using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Service;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AirportTicketExercise.Test
{
    [Collection("User Service")]
    public class UserTesting : IClassFixture<AirportTicketBookingFixture>
    {
        private readonly AirportTicketBookingFixture _fixture;

        public UserTesting(AirportTicketBookingFixture atbFixture)
        {
            _fixture = atbFixture;
        }

        [Theory]
        [Trait("Category", "CreateUser")]
        [InlineData("CreateUser1", "NewPassword1", UserType.Manager)]
        [InlineData("!Passenger1", "!@#$%1", UserType.Passenger)]
        [InlineData("2331", "123456789101", UserType.Passenger)]
        public void CreateUser_WithValidData_ShouldCreateUser(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var expectedUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };
            _fixture.UserService.CreateUser(expectedUser);

            User? actualUser = _fixture.UserService.Authenticate(expectedUser);

            actualUser.Should().NotBeNull();
            actualUser.Should().BeEquivalentTo(expectedUser, options => options.Excluding(user => user.UserId));
        }

        [Theory]
        [Trait("Category", "CreateUser")]
        [InlineData("CreateUser1", "NewPassword1", UserType.Manager)]
        [InlineData("!Passenger1", "!@#$%1", UserType.Passenger)]
        [InlineData("2331", "123456789101", UserType.Passenger)]
        public void CreateUser_WithDuplicates_ShouldCauseExecption(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var user = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };

            _fixture.UserService.CreateUser(user);

            Assert.Throws<DuplicateNameException>(() =>
            {
                _fixture.UserService.CreateUser(user);
            });
        }

        [Theory]
        [Trait("Category", "CreateUser")]
        [InlineData("ManagerUser1234567", "Password", UserType.Manager)]
        [InlineData("ManagerUser", "Password123123123123", UserType.Manager)]
        [InlineData("", "Password", UserType.Passenger)]
        [InlineData("ManagerUser", "", UserType.Manager)]
        [InlineData("Hi", "Password123", UserType.Passenger)]
        [InlineData("Manager", "p1", UserType.Manager)]
        public void CreateUser_WithInvalidFields_ShouldCauseException(string username, string password, UserType userType)
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepository.Object);
            var user = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };

            Assert.Throws<ValidationException>(() =>
            {
                userService.CreateUser(user);
            });
        }


        [Theory]
        [Trait("Category", "AuthenticateUser")]
        [InlineData("ManagerUser2", "pass2", UserType.Manager)]
        [InlineData("!Passenger2", "!@#$%2", UserType.Passenger)]
        [InlineData("23312", "123456789", UserType.Passenger)]
        public void AuthenticateUser_WithValidData_ShouldReturnUser(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var expectedUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };
            _fixture.UserService.CreateUser(expectedUser);

            User? actualUser = _fixture.UserService.Authenticate(expectedUser);

            actualUser.Should().NotBeNull();
            actualUser.Should().BeEquivalentTo(expectedUser, options => options.Excluding(user => user.UserId));
        }

        [Theory]
        [Trait("Category", "AuthenticateUser")]
        [InlineData("ManagerUser3", "pass3", UserType.Manager)]
        [InlineData("!Passenger3", "!@#$%3", UserType.Passenger)]
        [InlineData("23323", "1234567891023", UserType.Passenger)]
        public void AuthenticateUser_WithNonExistingUser_ShouldCauseExeption(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var nonExistentUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };

            Assert.Throws<KeyNotFoundException>(() =>
                _fixture.UserService.Authenticate(nonExistentUser)
            );
        }

        [Theory]
        [Trait("Category", "GetUser")]
        [InlineData("ManagerUser3", "pass3", UserType.Manager)]
        [InlineData("!Passenger3", "!@#$%3", UserType.Passenger)]
        [InlineData("23323", "12345678910", UserType.Passenger)]
        public void GetUser_WithValidData_ShouldReturnUser(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var expectedUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };
            _fixture.UserService.CreateUser(expectedUser);

            User? actualUser = _fixture.UserService.GetUser(expectedUser.UserId);

            actualUser.Should().NotBeNull();
            actualUser.Should().BeEquivalentTo(expectedUser, options => options.Excluding(user => user.UserId));
        }

        [Theory]
        [Trait("Category", "GetUser")]
        [InlineData("ManagerUser3", "pass3", UserType.Manager)]
        [InlineData("!Passenger3", "!@#$%3", UserType.Passenger)]
        [InlineData("23323", "12345678910", UserType.Passenger)]
        public void GetUser_WithNonExistingUser_ShouldCauseException(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var nonExistentUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };

            Assert.Throws<KeyNotFoundException>(() =>
                _fixture.UserService.GetUser(nonExistentUser.UserId)
            );
        }

        [Theory]
        [Trait("Category", "GetUser")]
        [InlineData("ManagerUser3", "pass3", UserType.Manager)]
        [InlineData("!Passenger3", "!@#$%3", UserType.Passenger)]
        [InlineData("23323", "12345678910", UserType.Passenger)]
        public void GetUserByName_WithValidData_ShouldReturnUser(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var expectedUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };
            _fixture.UserService.CreateUser(expectedUser);

            User? actualUser = _fixture.UserService.GetUserByName(expectedUser.Name);

            actualUser.Should().NotBeNull();
            actualUser.Should().BeEquivalentTo(expectedUser, options => options.Excluding(user => user.UserId));
        }

        [Theory]
        [Trait("Category", "GetUser")]
        [InlineData("ManagerUser3", "pass3", UserType.Manager)]
        [InlineData("!Passenger3", "!@#$%3", UserType.Passenger)]
        [InlineData("23323", "12345678910", UserType.Passenger)]
        public void GetUserByName_WithNonExistingUser_ShouldReturnException(string username, string password, UserType userType)
        {
            _fixture.ResetFiles();
            var nonExistentUser = new User
            {
                Name = username,
                Password = password,
                UserType = userType
            };

            Assert.Throws<KeyNotFoundException>(() =>
                _fixture.UserService.GetUserByName(nonExistentUser.Name)
            );
        }

        [Theory]
        [Trait("Category", "GetUser")]
        [InlineData(UserType.Manager, 3)]
        [InlineData(UserType.Passenger, 2)]
        public void GetUsersByType_WithValidData_ShouldReturnUserList(UserType expectedUserType, int expectedSize)
        {
            //Arrange
            _fixture.ResetFiles();

            var user1 = new User
            {
                Name = "User1",
                Password = "Pass123",
                UserType = UserType.Manager
            };

            var user2 = new User
            {
                Name = "User2",
                Password = "Pass123",
                UserType = UserType.Passenger
            };

            var user3 = new User
            {
                Name = "User3",
                Password = "Pass123",
                UserType = UserType.Passenger
            };

            var user4 = new User
            {
                Name = "User4",
                Password = "Pass123",
                UserType = UserType.Manager
            };

            var user5 = new User
            {
                Name = "User5",
                Password = "Pass123",
                UserType = UserType.Manager
            };

            _fixture.UserService.CreateUser(user1);
            _fixture.UserService.CreateUser(user2);
            _fixture.UserService.CreateUser(user3);
            _fixture.UserService.CreateUser(user4);
            _fixture.UserService.CreateUser(user5);

            //Act
            var actualUserList = _fixture.UserService.GetUserByType(expectedUserType);

            //Assert
            actualUserList.Should()
                    .HaveCount(expectedSize)
                    .And.OnlyContain(user => user.UserType == expectedUserType);
        }
    }
}