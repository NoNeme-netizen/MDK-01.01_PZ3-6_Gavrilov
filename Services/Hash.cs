using System;
using System.Security.Cryptography;
using System.Text;

namespace PZ3.Services
{
    public class Hash
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public static string HashPassword(string salt, string password)
        {
            return HashPassword(salt + password);
        }
    }
}