using Bitcoin.Api.Models;

namespace Bitcoin.Api.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string email, string password, string role)
        {
            var user = new User();
            user.Username = username;
            user.Email = email;
            user.Password = password;
            user.Role = role;

            return user;
        }
    }
}