using System;
using System.Linq;
using PZ3.Models;

namespace PZ3.Services
{
    public class AuthService
    {
        public AuthorizedUser CheckUser(string login, string password)
        {
            PhotoCenterEntities db = PhotoCenterEntities.GetContext();

            Users user = db.Users.FirstOrDefault(x => x.Login == login && x.IsActive);

            if (user == null)
                return null;

            string inputHash = Hash.HashPassword(user.Salt, password);

            if (string.Equals(inputHash, user.PasswordHash, StringComparison.OrdinalIgnoreCase) == false)
                return null;

            AuthorizedUser result = new AuthorizedUser();
            result.UserId = user.UserID;
            result.Login = user.Login;
            result.Role = user.Role;

            return result;
        }
    }
}