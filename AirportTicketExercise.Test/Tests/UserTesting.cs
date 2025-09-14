using ATB.Data.Models;
using ATB.Data.Repository;
using ATB.Logic.Service;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AirportTicketExercise.Test.Tests
{
    public class UserTesting
    {
        private Mock<IUserRepository> _mockRepo;
        private UserService _service;

        public UserTesting()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        [Fact]
        public void AuthenticateUser_WithValidUser_ReturnsUser()
        {
            var expectedUser = DummyData.ValidUser1;
            _mockRepo.Setup(r => r.GetUser(expectedUser.Name)).Returns(expectedUser);

            User? actualUser = _service.Authenticate(expectedUser);

            Assert.Equal(expectedUser, actualUser);
        }

        [Fact]
        public void AuthenticateUser_WithInvalidUser_ThrowsException()
        {
            var expectedUser = DummyData.InvalidUser1;
            _mockRepo.Setup(r => r.GetUser(expectedUser.Name))
                     .Throws<KeyNotFoundException>();

            Assert.Throws<KeyNotFoundException>(() => _service.Authenticate(expectedUser));
        }

        [Fact]
        public void CreateUser_WithValidUser_AddsUserToDB()
        {
            var user = DummyData.ValidUser1;

            _service.CreateUser(user);

            _mockRepo.Verify(r => r.CreateUser(user), Times.Once);
        }

        [Fact]
        public void CreateUser_WithInvalidFields_ThrowsException()
        {
            var user = DummyData.InvalidUser1;
            _mockRepo.Setup(r => r.GetUser(user.Name)).Returns((User)null);

            Assert.Throws<ValidationException>(() => _service.CreateUser(user));
        }

        [Fact]
        public void GetUser_WithExistingUser_ReturnsUser()
        {
            var user = DummyData.InvalidUser1;
            _mockRepo.Setup(r => r.GetUser(user.UserId)).Returns(user);

            var result = _service.GetUser(user.UserId);

            Assert.Equal(user, result);
        }

        [Fact]
        public void GetUser_WithNonExistingUser_ThrowsException()
        {
            var user = DummyData.ValidUser1;
            _mockRepo.Setup(r => r.GetUser(user.UserId)).Returns((User)null);

            Assert.Throws<KeyNotFoundException>(() => _service.GetUser(user.UserId));
        }

        [Fact]
        public void GetUserByName_WithExistingUser_ReturnsUser()
        {
            var expectedUser = DummyData.InvalidUser1;
            _mockRepo.Setup(r => r.GetUser(expectedUser.Name)).Returns(expectedUser);

            var result = _service.GetUserByName(expectedUser.Name);

            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public void GetUserByName_WithNonExistingUser_ThrowsException()
        {
            var user = DummyData.ValidUser1;
            _mockRepo.Setup(r => r.GetUser(user.Name)).Returns((User)null);

            Assert.Throws<KeyNotFoundException>(() => _service.GetUserByName(user.Name));
        }

        [Fact]
        public void GetUserByType_WithExistingUser_ReturnsUser()
        {
            //Arrange
            var users = new List<User> { DummyData.ValidUser1, DummyData.ValidUser2 };
            UserType expectedUserType = UserType.Manager;
            var expectedUsers = users.Where(u => u.UserType == expectedUserType).ToList();
            
            _mockRepo.Setup(r => r.GetUsersByType(expectedUserType)).Returns(expectedUsers);

            //Act
            var actualUsers = _service.GetUserByType(expectedUserType);

            //Assert
            Assert.Equal(expectedUsers.Count, actualUsers.Count);
        }
    }
}
