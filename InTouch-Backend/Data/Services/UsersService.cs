using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace InTouch_Backend.Data.Services
{
    public class UsersService
    {
        public AppDbContext _context;
        public UsersService(AppDbContext context)
        {
            _context = context;
        }
        public void register(UserVM user)
        {
            // Check if email or username already exists
            bool emailExists = _context.Users.Any(u => u.Email == user.Email);
            bool usernameExists = _context.Users.Any(u => u.Username == user.Username);

            if (emailExists || usernameExists)
            {
                throw new Exception("Email or username already exists");
            }

            var _user = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                profile_img = user.profile_img,
                role = user.role
            };

            var passwordHasher = new PasswordHasher<string>();
            _user.Password = passwordHasher.HashPassword(null, user.Password);
            _context.Users.Add(_user);
            _context.SaveChanges();
        }
        public bool login(Login user)
        {
            var _user = _context.Users.SingleOrDefault(u => u.Email == user.EmailorUsername || u.Username == user.EmailorUsername);

            if (_user != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                var result = passwordHasher.VerifyHashedPassword(null, _user.Password, user.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return true;
                }
            }
            return false;
        }
        public List<User> getUsers() => _context.Users.ToList();
    }
}
