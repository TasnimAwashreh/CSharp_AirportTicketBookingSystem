using ATB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Data.Repository
{
    public interface IUserRepository
    {

        public bool CreateUser(User User);
        public bool UpdateUser(User User);
        public User? GetUserById(int id);
        public User? GetUserByName(string username);
        public List<User> GetUserByType(UserType type);
        public List<User> GetAllUsers();
    }

}