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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.HttpResults;
using InTouch_Backend.Data.DTOs;

namespace InTouch_Backend.Data.Services
{
    public class UsersService
    {
        public AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PostService _postService;

        public UsersService(AppDbContext context,PostService postService, IConfiguration configuration)
        {
            _context = context;
            _postService = postService;
            _configuration = configuration;

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
                    isPrivate = user.isPrivate,
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

        public void updateProfilePicture (UpdateProfilePic newPic)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Id == newPic.Id);
            if (_user == null)
            {
                throw new Exception("User not found");
            }
            if(newPic.Image!=null && newPic.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(_user.ImagePath))
                {
                    string ImageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "User Images", _user.ImagePath);
                    if (File.Exists(ImageFilePath))
                    {
                        File.Delete(ImageFilePath);
                    }
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + newPic.Image.FileName;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "User Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    newPic.Image.CopyTo(fileStream);
                }

                _user.ImagePath = uniqueFileName;
            }

            _context.SaveChanges();
                
        }
           
        


        public string login(Login user)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Email == user.EmailorUsername || u.Username == user.EmailorUsername);

            if (_user == null)
            {
                throw new Exception("User not found");

            };
            if (_user.isLocked)
            {
              //  DeleteUser(_user.Id);
                throw new DataException("This account has been locked. Please send a message to us by clicking anywhere on this text. If the lock is lifted please don't post inappropriate content. Your account will be deleted.");

            }
           

            var passwordHasher = new PasswordHasher<string>();
            var result = passwordHasher.VerifyHashedPassword(null, _user.Password, user.Password);
            if(result != PasswordVerificationResult.Success)
            {
                throw new Exception("Failed to verify credentials");
            }
            if (result == PasswordVerificationResult.Success)
            {
                string token = CreateToken(_user);
                return token;
            }

            return null;
        }

        private string CreateToken(User _user)
        {
            if (_user == null)
            {
                throw new ArgumentNullException(nameof(_user));
            }

            List<Claim> claims = new List<Claim> {
                 new Claim(ClaimTypes.Email, _user.Email),
            new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, _user.Username),
            new Claim(ClaimTypes.Role, _user.Role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

           
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(20),
            notBefore: DateTime.UtcNow,
            signingCredentials: creds);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenString;
           
        }
        public bool isTokenAvailable(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false; 
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, 
                IssuerSigningKey = key, 
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ClockSkew = TimeSpan.Zero 
            };

            try
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true; // Token is valid
            }
            catch
            {
                return false; // Token validation failed
            }
        }
       
       
        public List<User> getUsers() => _context.Users.Where(u => u.Role == 0).ToList();

     
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

        public bool isFollowing(int userOne, int userTwo)
        {
            bool temp = _context.Follows.Any(f => f.FollowerId == userOne && f.FollowingId == userTwo);
            return temp;
        }
        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                var userPosts = _context.Posts.Where(u => u.userID == id);
                foreach (var item in userPosts)
                {
                    _postService.deletePost(item.Id);
                }
                var userFollows = _context.Follows.Where(u => u.FollowerId == id || u.FollowingId == id);
                _context.Follows.RemoveRange(userFollows);

                var userRequest = _context.FollowRequests.Where(u => u.FollowRequestId== id || u.FollowRequestedId == id);
                _context.FollowRequests.RemoveRange(userRequest);

                var userReports = _context.Reports.Where(r => r.UserId == id);
                _context.Reports.RemoveRange(userReports);

                var userLikes = _context.Likes.Where(l => l.UserId == id);
                _context.Likes.RemoveRange(userLikes);

                var userComments = _context.Comments.Where(c => c.UserId == id);
                _context.Comments.RemoveRange(userComments);

                var userSavedPosts = _context.SavedPosts.Where(s => s.UserId == id);
                _context.SavedPosts.RemoveRange(userSavedPosts);




                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }

            return false;


        }
        public List<User> suggestedUsers(int userId)
        {
            List<User> allUsers = _context.Users.Where(u=> u.Role==0).ToList();
            List<int> followingIds = _context.Follows
               .Where(f => f.FollowerId == userId)
               .Select(f => f.FollowingId)
               .ToList();

            List<User> suggestedUsers = allUsers
            .Where(u => u.Id != userId && !followingIds.Contains(u.Id))
            .ToList();


            return suggestedUsers.Take(10).ToList();

        }
        public List<User> searchUsers(int userId, string query)
        {

            List<User> searchresult = _context.Users.Where(u => u.Id != userId&& u.Role==0 && (u.FirstName.Contains(query) || u.Username.Contains(query) || u.LastName.Contains(query))).ToList();
            return searchresult;
        }
        public int[] getFollows_and_Followers(int userId)
        {
            var follows = _context.Follows.Where(f => f.FollowerId == userId).ToList();
            var followers = _context.Follows.Where(f => f.FollowingId == userId).ToList();

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
        public List<User> userFollowing(int userId)
        {
            List<int> followingUserIds = _context.Follows
           .Where(f => f.FollowerId == userId)
           .Select(f => f.FollowingId)
           .ToList();


            List<User> temp = _context.Users
                .Where(u => followingUserIds.Contains(u.Id))
                .ToList();
            return temp;
        }

        public void updatePassword (UpdatePassword updatePassword)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Id == updatePassword.Id);

            if (_user == null)
            {
                throw new Exception("User not found");

            };
            var passwordHasher = new PasswordHasher<string>();
            var result = passwordHasher.VerifyHashedPassword(null, _user.Password, updatePassword.OldPassword);


            if (result != PasswordVerificationResult.Success)
            {
                throw new Exception("Old password is wrong");
            }
            else
            {
                _user.Password = passwordHasher.HashPassword(null, updatePassword.NewPassword);
                _context.SaveChanges();

            }

        }

        public void updateUserInfo (UpdateUserInfo user)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            bool emailExist = _context.Users.Any(u => u.Id!=user.Id && u.Email.Equals(user.Email));
            bool usernameExist = _context.Users.Any(u => u.Id != user.Id && u.Username.Equals(user.Username));

            if (emailExist)
            {
                throw new Exception("The email you are trying to use already exist.");
            }
            if(usernameExist)
            {
                throw new Exception("The username you are trying to use already exist");
            }

            if (_user == null)
            {
                throw new Exception("User not found");

            };
            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;
            _user.Username = user.Username;
            _user.Email = user.Email;
            _user.isPrivate = user.isPrivate;

            _context.SaveChanges();

        }

        public void lockUserAccount(int userId)
        {
            var user = _context.Users.FirstOrDefault(u=> u.Id== userId);
            if(user==null)
            {
                throw new Exception("User does not exist");
            }
            if (user.isLocked)
            {
                throw new Exception("This account has already been locked.");
            }
            user.isLocked = true;
            _context.SaveChanges();
        }

        public void unlockUserAccount(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            if (!user.isLocked)
            {
                throw new Exception("This account is unlocked.");
            }
            user.isLocked = false;
            _context.SaveChanges();
        }

        public List<int> dashboardAnalytics()
        {
            List<int> analytics = new List<int>();
            int countUser = _context.Users.Where(u=>u.Role==0).Count();
            int countPosts = _context.Posts.Count();
            int countMessages = _context.SupportMessages.Count();
            int countReports = _context.Reports.Count();

            analytics.Add(countUser);
            analytics.Add(countPosts);
            analytics.Add(countMessages);
            analytics.Add(countReports);

            return analytics;
        }
    };
}

        
    
