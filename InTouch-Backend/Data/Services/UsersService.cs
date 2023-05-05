using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Data;
using System.Linq;

namespace InTouch_Backend.Data.Services
{
    public class UsersService
    {
        public AppDbContext _context;

 
        public UsersService(AppDbContext context)
        {
            _context = context;
    
        }
        public void register(UserDTO user)
        {
           
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
                Role = user.Role
            };

            var passwordHasher = new PasswordHasher<string>();
            _user.Password = passwordHasher.HashPassword(null, user.Password);

            if (user.Image != null && user.Image.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + user.Image.FileName;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "User Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.Image.CopyTo(fileStream);
                }

                _user.ImagePath = uniqueFileName;
            }

            _context.Users.Add(_user);
            _context.SaveChanges();

          
        }

        public int login(Login user)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Email == user.EmailorUsername || u.Username == user.EmailorUsername);

            if (_user != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                var result = passwordHasher.VerifyHashedPassword(null, _user.Password, user.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return _user.Id;
                }
            }
            return 0;
        }
        public List<User> getUsers() => _context.Users.ToList();

       

       public User getUserInfo(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public int[] getFollows_and_Followers(int userId)
        {

            var follows= _context.Follows.Where(f=> f.FollowerId==userId).ToList();
            var followers=_context.Follows.Where(f=>f.FollowingId==userId).ToList();

            int countFollows = follows.Count;
            int countFollowers = followers.Count;

            int[] temp = { countFollows, countFollowers };

            return temp;
        }

        public bool isFollowing(int userOne,int userTwo)
        {
            bool temp = _context.Follows.Any(f => f.FollowerId == userOne && f.FollowingId == userTwo);
            return temp;
        }

        public List<User> suggestedUsers(int userId)
        {
            List<User> allUsers = _context.Users.ToList();

         
            List<int> followingIds = _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToList();

         
            List<User> suggestedUsers = allUsers
                .Where(u => u.Id != userId && !followingIds.Contains(u.Id))
                .ToList();

         
            return suggestedUsers.Take(10).ToList();

        }

        public List<User> searchUsers (int userId ,string query)
        {
            List<User> searchresult = _context.Users.Where(u=> u.Id!=userId &&( u.FirstName.StartsWith(query) || u.Username.StartsWith(query) || u.LastName.StartsWith(query))).ToList();
            return searchresult;
        }

        public List<User> userFollowers(int userId)
        {
            List<int> followedUserIds = _context.Follows
            .Where(f => f.FollowingId == userId)
            .Select(f => f.FollowerId)
            .ToList();


            List<User> temp = _context.Users
                .Where(u => followedUserIds.Contains(u.Id))
                .ToList();

            return temp;
        }

        
        
    }
}
