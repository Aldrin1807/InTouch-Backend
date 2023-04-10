using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace InTouch_Backend.Data.Services
{
    public class UserService
    {
        public AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public void addUser(UserVM user)
        {
            var _user = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username= user.Username,
                Email = user.Email,
                Password = user.Password,
                role = user.role
            };
            _context.Users.Add(_user);
            _context.SaveChanges();
        }
        public List<User> getUsers() => _context.Users.ToList();
    }
}
