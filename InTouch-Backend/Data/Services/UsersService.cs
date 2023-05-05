﻿
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

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

        public int[] getFollows_and_Followers(int userId)
        {

            var follows= _context.Follows.Where(f=> f.FollowerId==userId).ToList();
            var followers=_context.Follows.Where(f=>f.FollowingId==userId).ToList();

            int countFollows = follows.Count;
            int countFollowers = followers.Count;

            int[] temp = { countFollows, countFollowers };

            return temp;
        }

        
        
    }
}
