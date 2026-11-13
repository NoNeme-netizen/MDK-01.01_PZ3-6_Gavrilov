using System;
using System.Configuration;
using System.Data.SqlClient;
using PZ3.Models;

namespace PZ3.Services
{
    public class AuthService
    {
        private readonly string connectionString;

        public AuthService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PhotoCenterDb"].ConnectionString;
        }

        // возвращает пользователя, если логин и пароль верные, иначе null
        public AuthorizedUser CheckUser(string login, string password)
        {
            string query = @"SELECT UserID, Login, Role, PasswordHash, Salt
                             FROM dbo.Users
                             WHERE Login = @Login AND IsActive = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() == false)
                            return null;

                        string savedHash = reader["PasswordHash"].ToString();
                        string salt = reader["Salt"].ToString();

                        // считаем хэш от введённого пароля и сравниваем с сохранённым
                        string inputHash = Hash.HashPassword(salt, password);

                        if (string.Equals(inputHash, savedHash, StringComparison.OrdinalIgnoreCase) == false)
                            return null;

                        AuthorizedUser user = new AuthorizedUser();
                        user.UserId = Convert.ToInt32(reader["UserID"]);
                        user.Login = reader["Login"].ToString();
                        user.Role = reader["Role"].ToString();

                        return user;
                    }
                }
            }
        }
    }
}