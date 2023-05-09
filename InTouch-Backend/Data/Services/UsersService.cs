using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InTouch_Backend.Data.Services
{
    public class UsersService
    {
        public AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
    
        }
        public void register(UserDTO user)
        {
            try
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
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public string login(Login user)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Email == user.EmailorUsername || u.Username == user.EmailorUsername);

            if (_user != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                var result = passwordHasher.VerifyHashedPassword(null, _user.Password, user.Password);
                var claims = new[]
                {
            new Claim(ClaimTypes.Email, _user.Email),
            new Claim(ClaimTypes.NameIdentifier, _user.FirstName),
            new Claim(ClaimTypes.GivenName, _user.Username),
            new Claim(ClaimTypes.Surname, _user.LastName),
            new Claim(ClaimTypes.Role, _user.Role.ToString())
        };
                if (result == PasswordVerificationResult.Success)
                {
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),

                            SecurityAlgorithms.HmacSha256));
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return tokenString;
                }
            }
            return null;
        }
        public List<User> getUsers() => _context.Users.ToList();


        public void updateProfile(int userId, UserDTO updatedUser = null)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (updatedUser.FirstName != null)
            {
                user.FirstName = updatedUser.FirstName;
            }

        public void updateProfile(int userId, UserDTO updatedUser = null)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (updatedUser.FirstName != null)
            {
                user.FirstName = updatedUser.FirstName;
            }

            if (updatedUser.LastName != null)
            {
                user.LastName = updatedUser.LastName;
            }

            if (updatedUser.Username != null)
            {
                user.Username = updatedUser.Username;
            }

            if (updatedUser.Email != null)
            {
                user.Email = updatedUser.Email;
            }

            if (updatedUser?.Image != null && updatedUser.Image.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + updatedUser.Image.FileName;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "User Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    updatedUser.Image.CopyTo(fileStream);
                }

                user.ImagePath = uniqueFileName;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }


      

       public User getUserInfo(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }


        public User getUserById(int userId)
        {
            var user = _context.Users.Find(userId);
           /* if (user == null)
            {
                throw new Exception("User no found");
            }*/
            return user;
        }
            if (updatedUser.LastName != null)
            {
                user.LastName = updatedUser.LastName;
            }

        
       


        public int[] getFollows_and_Followers(int userId)
        {
            if (updatedUser.Username != null)
            {
                user.Username = updatedUser.Username;
            }

            if (updatedUser.Email != null)
            {
                user.Email = updatedUser.Email;
            }

            if (updatedUser?.Image != null && updatedUser.Image.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + updatedUser.Image.FileName;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


        
        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                
                
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
                   
            return false;
            
                
        }
       
        public bool isFollowing(int userOne, int userTwo)
        {
            bool temp = _context.Follows.Any(f => f.FollowerId == userOne && f.FollowingId == userTwo);
            return temp;
        }
                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    updatedUser.Image.CopyTo(fileStream);
                }

                user.ImagePath = uniqueFileName;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }


            List<int> followingIds = _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToList();



            List<User> suggestedUsers = allUsers
                .Where(u => u.Id != userId && !followingIds.Contains(u.Id))
                .ToList();


            return suggestedUsers.Take(10).ToList();
        public User getUserById(int userId)
        {
            var user = _context.Users.Find(userId);
           /* if (user == null)
            {
                throw new Exception("User no found");
            }*/
            return user;
        }

        
       

       public User getUserInfo(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public List<User> searchUsers(int userId, string query)
        public int[] getFollows_and_Followers(int userId)
        {
            List<User> searchresult = _context.Users.Where(u => u.Id != userId && (u.FirstName.Contains(query) || u.Username.Contains(query) || u.LastName.Contains(query))).ToList();
            return searchresult;

            var follows= _context.Follows.Where(f=> f.FollowerId==userId).ToList();
            var followers=_context.Follows.Where(f=>f.FollowingId==userId).ToList();

            int countFollows = follows.Count;
            int countFollowers = followers.Count;

            int[] temp = { countFollows, countFollowers };

            return temp;
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

        
        
        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                
                
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
                   
return false;
            
                
        }



    }
}
