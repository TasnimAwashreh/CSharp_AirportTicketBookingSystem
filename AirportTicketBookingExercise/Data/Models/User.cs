using ATB.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATB.Data.Models
{
    public enum UserType
    {
        Manager,
        Passanger
    }
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }

    }
}
