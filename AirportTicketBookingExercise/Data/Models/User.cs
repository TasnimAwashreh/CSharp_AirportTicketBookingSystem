
using CsvHelper.Configuration;
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
    public sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Map(m => m.UserId).Name("UserId");
            Map(m => m.Name).Name("Name");
            Map(m => m.Password).Name("Password");
            Map(m => m.UserType).Name("UserType");
        }
    }
}
