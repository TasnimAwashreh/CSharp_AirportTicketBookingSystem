
using System.ComponentModel.DataAnnotations;

namespace ATB.Data.Models
{
    public enum UserType
    {
        Manager,
        Passenger
    }
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public UserType UserType { get; set; }

    }
}
