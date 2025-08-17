using AirportTicketBookingExercise.Data.Repository;
using ATB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Logic.Service
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

        public User? GetUserById(int userId)
        {

           return _userRepository.GetUserById(userId);
        }

        public User? GetUserByName(string username)
        {
            return _userRepository.GetUserByName(username);
        }

        public List<User> GetUserByType(UserType type)
        {
            return _userRepository.GetUserByType(type);
        }

        public bool UpdateUser(User user)
        {
                return _userRepository.UpdateUser(user);
        }
    }
}
