using ATB.Data.Repository;
using ATB.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Authentication;

namespace ATB.Logic.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        { 
        
            _userRepository = userRepository;
        }

        public User? Authenticate(User user)
        {
            var existingUser = GetUserByName(user.Name);
            if (existingUser == null || existingUser.Password != user.Password || existingUser.UserType != user.UserType) 
            {
                throw new KeyNotFoundException();
            }
            return user;
        }

        public void CreateUser(User user)
        {
            var context = new ValidationContext(user, null, null);
            var validationResults = new List<ValidationResult>();
            bool isFieldValid = Validator.TryValidateObject(user, context, validationResults, true);
            if (!isFieldValid)
                throw new ValidationException();

            if (_userRepository.GetUser(user.Name) != null)
                throw new DuplicateNameException();
            _userRepository.CreateUser(user);
        }

        public User? GetUser(int userId)
        {
            User? user = _userRepository.GetUser(userId);
            if (user == null)
                throw new KeyNotFoundException();
            return user;
        }

        public User? GetUserByName(string username)
        {
            User? user = _userRepository.GetUser(username);
            if (user == null)
                throw new KeyNotFoundException();
            return user;
        }

        public List<User> GetUserByType(UserType type)
        {
            return _userRepository.GetUsersByType(type);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
    }
}
