using ATB.Data.Repository;
using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { 
        
            _userRepository = userRepository;
        }

        public User? Authenticate(string username, string password)
        {

            var user = GetUserByName(username);
           
             if (user != null && user.Password.Equals(password)) {
                return user;
            }

            return null;
        }

        public bool CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }

        public User? GetUser(int userId)
        {
            return _userRepository.GetAllUsers()
                 .FirstOrDefault(u => u.UserId == userId);
        }

        public User? GetUserByName(string username)
        {
            return _userRepository.GetAllUsers()
                .FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public List<User> GetUserByType(UserType type)
        {
            return _userRepository.GetAllUsers()
                        .Where(u => u.UserType == type)
                        .ToList();
        }

        public bool UpdateUser(User user)
        {
                return _userRepository.UpdateUser(user);
        }
    }
}
