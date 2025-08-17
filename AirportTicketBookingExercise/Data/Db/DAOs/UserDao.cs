using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ATB.Data.Models;
using ATB.Logic.Enums;

namespace AirportTicketBookingExercise.Data.Db.DAOs
{
    public class UserDAO
    {
        private readonly string _connectionString;

        public UserDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateUser(User user)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO User (Name, Password, Type) VALUES ($name, $pass, $type)";
            cmd.Parameters.AddWithValue("$name", user.Name);
            cmd.Parameters.AddWithValue("$pass", user.Password);
            cmd.Parameters.AddWithValue("$type", user.UserType.ToString());
            cmd.ExecuteNonQuery();
        }

        public User? GetUserById(int userId)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM User WHERE UserId = $userid";
            cmd.Parameters.AddWithValue("$userid", userId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            return null;
        }

        public User? GetUserByUsername(string username)
        {
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM User WHERE Name = $name";
            cmd.Parameters.AddWithValue("$name", username);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            return null;
        }

        public List<User> GetUsersByType(UserType type)
        {
            var users = new List<User>();
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM User WHERE Type = $type";
            cmd.Parameters.AddWithValue("$type", type.ToString());
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapUser(reader));
            }
            return users;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new SqliteConnection($"Data Source={_connectionString}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM User";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapUser(reader));
            }
            return users;
        }


        private User MapUser(SqliteDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Password = reader.GetString(2),
                UserType = Enum.Parse<UserType>(reader.GetString(3), true)
            };
        }
    }
}
