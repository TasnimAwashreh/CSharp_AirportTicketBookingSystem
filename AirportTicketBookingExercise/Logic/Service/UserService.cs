using ATB.Data.Repository;
using ATB.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ATB.Logic.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        { 
        
            _userRepository = userRepository;
        }

        public User? Authenticate(string username, string password, UserType usertype)
        {
            var user = GetUserByName(username);
            if (user != null && user.Password.Equals(password) && user.UserType == usertype) 
            {
                return user;
            }
            return null;
        }

        public bool CreateUser(string name, string password, UserType usertype)
        {
            var user = new User
            {
                Name = name,
                Password = password,
                UserType = usertype
            };

            var context = new ValidationContext(user, null, null);
            var validationResults = new List<ValidationResult>();
            bool isFieldValid = Validator.TryValidateObject(user, context, validationResults, true);
            if (!isFieldValid)
            {
                throw new ValidationException();
            }

            if (_userRepository.GetUser(name) != null)
                return false;
            _userRepository.CreateUser(user);
            return true;
        }

        public User? GetUser(int userId)
        {
            return _userRepository.GetUser(userId);
        }

        public User? GetUserByName(string username)
        {
            return _userRepository.GetUser(username);
        }

        public List<User> GetUserByType(UserType type)
        {
            return _userRepository.GetUsersByType(type);
        }

        public bool UpdateUser(User user)
        {
                return _userRepository.UpdateUser(user);
        }
    }
}
