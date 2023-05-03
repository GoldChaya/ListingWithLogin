using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Homework_April_19.Data
{
    public class ListingsRepository
    {
        private string _connectionString;
        public ListingsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Listing>GetAll(User specificUser)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Listing ";
                if (specificUser!=null)
            { 
                command.CommandText += " WHERE UserId =@id ";
                command.Parameters.AddWithValue("@id", specificUser.Id);
            };

            List<Listing> listings = new();
            connection.Open();
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                listings.Add(new Listing
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"]

                });
            }
            return listings;
        }
        public void Add(Listing listing)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Listing (Name, Date, PhoneNumber, Description, UserId) VALUES (@name, @date, @phoneNumber, @description, @userId)";
            command.Parameters.AddWithValue("@name",listing.Name);
            command.Parameters.AddWithValue("@date", listing.Date);
            command.Parameters.AddWithValue("@phoneNumber", listing.PhoneNumber);
            command.Parameters.AddWithValue("@description", listing.Description);
            command.Parameters.AddWithValue("@userId", listing.UserId);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void AddUser (User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users(Name, Email, PasswordHash) VALUES (@name, @email, @passwordHash)";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;

        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"],
            };
        }
        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Listing WHERE Id=@id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
