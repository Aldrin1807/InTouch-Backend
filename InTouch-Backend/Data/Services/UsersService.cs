using InTouch_Backend.Data.Models;
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
using Azure.Storage.Blobs;
using System.Net.Mail;
using MailKit.Security;
using MimeKit;
using System.Net;
using NuGet.Protocol.Plugins;
using Microsoft.Extensions.Hosting;

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
        public async Task register(UserDTO user)
        {
           
                bool emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                bool usernameExists =await _context.Users.AnyAsync(u => u.Username == user.Username);

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

               
                BlobServiceClient blobServiceClient = new BlobServiceClient("BlobEndpoint=https://intouchimages.blob.core.windows.net/;QueueEndpoint=https://intouchimages.queue.core.windows.net/;FileEndpoint=https://intouchimages.file.core.windows.net/;TableEndpoint=https://intouchimages.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-04T07:41:45Z&st=2023-07-03T22:41:45Z&spr=https&sig=FbwCGBBhHqxzyLsI8%2BZE7zkPFz9%2B0mlKT8a0vD0ucBs%3D");
               
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("user-images");

                BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);
                using (var stream = user.Image.OpenReadStream())
                {

                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                _user.ImagePath = uniqueFileName;
            }

              await  _context.Users.AddAsync(_user);
              await  _context.SaveChangesAsync();

                    var confirmationRecord = new Confirmations()
                    {
                       userId = _user.Id,
                       ConfirmationToken = CreateToken(_user),
                       ExpirationDate = DateTime.Now.AddMinutes(21).ToString()
                    };

                    await _context.Confirmations.AddAsync(confirmationRecord);
                    await _context.SaveChangesAsync();

                    // Send the confirmation email
                  await SendConfirmationEmailAsync(user.Email, confirmationRecord.ConfirmationToken);
        }
        async Task SendConfirmationEmailAsync(string email, string confirmationToken)
        {
         
            var message = new MailMessage
            {
                From = new MailAddress("intouchsm2023@gmail.com", "intouchsm2023@gmail.com"),
                Subject = "Confirmation Email",
                Body = $@"
                    <html>
                      <body>
                       <p>Thank you for registering with our service.</p>
                        <p>Please confirm your registration by clicking the following link:</p>
                        <a href=""https://intouch-socialmedia.netlify.app/confirm?token={confirmationToken}"">Click Here</a>
                        <br>
                        <p><strong>Note:</strong> The link is only valid for 20 minutes. If you fail to confirm within this time frame, your account will not be created, and your credentials will be deleted.</p>
                      </body>
                    </html>",
            IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));

           
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("intouchsm2023@gmail.com", "yafnjthflcnyjycf");

                try
                {
                    // Send the email
                    await client.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                  
                    throw new Exception("Failed to send confirmation email", ex);
                }
            }
        }

        public async Task ConfirmEmail(string token)
        {
            bool isAvailable = await _context.Confirmations.AnyAsync(c => c.ConfirmationToken == token);
            bool isTokenValid = await isTokenAvailable(token);

            if (!isAvailable || !isTokenValid)
            {
                throw new Exception("The link for confirmation has expired or not valid!");
            }
            var confirmation = await _context.Confirmations.FirstOrDefaultAsync(c => c.ConfirmationToken == token);
            if (confirmation != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == confirmation.userId);
                if (user != null)
                {
                    user.emailConfirmed = true;
                    _context.Confirmations.Remove(confirmation);
                    await _context.SaveChangesAsync();

                }
            }

        }

        public async Task updateProfilePicture (UpdateProfilePic newPic)
        {
            var _user =await _context.Users.FirstOrDefaultAsync(u => u.Id == newPic.Id);
            if (_user == null)
            {
                throw new Exception("User not found");
            }
            if (newPic.Image != null && newPic.Image.Length > 0)
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient("BlobEndpoint=https://intouchimages.blob.core.windows.net/;QueueEndpoint=https://intouchimages.queue.core.windows.net/;FileEndpoint=https://intouchimages.file.core.windows.net/;TableEndpoint=https://intouchimages.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-04T07:41:45Z&st=2023-07-03T22:41:45Z&spr=https&sig=FbwCGBBhHqxzyLsI8%2BZE7zkPFz9%2B0mlKT8a0vD0ucBs%3D");

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("user-images");

           

                if (!string.IsNullOrEmpty(_user.ImagePath))
                {
                  
                    BlobClient blobClientDel = containerClient.GetBlobClient(_user.ImagePath);
                    await blobClientDel.DeleteIfExistsAsync();
                }

              
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + newPic.Image.FileName;
                BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);

               
                using (var stream = newPic.Image.OpenReadStream())
                {


                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                _user.ImagePath = uniqueFileName;
            }
            _context.SaveChanges();
                
        }
         
        


        public async Task<string?> login(Login user)
        {
            var _user =await _context.Users.FirstOrDefaultAsync(u => u.Email == user.EmailorUsername || u.Username == user.EmailorUsername);

            if (_user == null)
            {
                throw new Exception("User not found");

            }else if (!_user.emailConfirmed)
            {
                throw new Exception("You need to confirm your email before logging in");
            } ;
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
        public async Task<bool> isTokenAvailable(string token)
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
       
       
        public async Task<List<User>> getUsers() =>await _context.Users.Where(u => u.Role == 0).ToListAsync();

     
        public async Task<User> getUserInfo(int id)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.emailConfirmed  && u.Role==0);
            return user;
        }

        public async Task<User> getUserById(int userId)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u=> u.Id==userId && u.emailConfirmed);
             if (user == null)
             {
                 throw new Exception("User no found");
             }
            return user;
        }

        public async Task<bool> isFollowing(int userOne, int userTwo)
        {
            bool temp =await _context.Follows.AnyAsync(f => f.FollowerId == userOne && f.FollowingId == userTwo);
            return temp;
        }
        public async Task<bool> DeleteUser(int id)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient("BlobEndpoint=https://intouchimages.blob.core.windows.net/;QueueEndpoint=https://intouchimages.queue.core.windows.net/;FileEndpoint=https://intouchimages.file.core.windows.net/;TableEndpoint=https://intouchimages.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-04T07:41:45Z&st=2023-07-03T22:41:45Z&spr=https&sig=FbwCGBBhHqxzyLsI8%2BZE7zkPFz9%2B0mlKT8a0vD0ucBs%3D");

              
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("user-images");

                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                   
                    BlobClient blobClientDel = containerClient.GetBlobClient(user.ImagePath);
                    await blobClientDel.DeleteIfExistsAsync();
                }

                _context.Users.Remove(user);
              await _context.SaveChangesAsync();
                return true;
            }

            return false;


        }
        public async Task<List<User>> suggestedUsers(int userId)
        {
            List<User> allUsers =await _context.Users.Where(u=> u.Role==0 && u.emailConfirmed).ToListAsync();
            List<int> followingIds =await _context.Follows
               .Where(f => f.FollowerId == userId)
               .Select(f => f.FollowingId)
               .ToListAsync();

            List<User> suggestedUsers = allUsers
            .Where(u => u.Id != userId && !followingIds.Contains(u.Id))
            .ToList();


            return suggestedUsers.Take(10).ToList();

        }
        public async Task<List<User>> searchUsers(int userId, string query)
        {

            List<User> searchresult =await _context.Users.Where(u => u.Id != userId&& u.Role==0 && u.emailConfirmed && (u.FirstName.Contains(query) || u.Username.Contains(query) || u.LastName.Contains(query))).ToListAsync();
            return searchresult;
        }
        public async Task<int[]> getFollows_and_Followers(int userId)
        {
            var follows = await _context.Follows.Where(f => f.FollowerId == userId).ToListAsync();
            var followers =await _context.Follows.Where(f => f.FollowingId == userId).ToListAsync();

            int countFollows = follows.Count;
            int countFollowers = followers.Count;

            int[] temp = { countFollows, countFollowers };

            return temp;
        }

        public async Task<List<User>> userFollowers(int userId)
        {
            List<int> followedUserIds = await _context.Follows
            .Where(f => f.FollowingId == userId)
            .Select(f => f.FollowerId)
            .ToListAsync();


            List<User> temp =await _context.Users
                .Where(u => followedUserIds.Contains(u.Id))
                .ToListAsync();

            return temp;
        }
        public async Task<List<User>> userFollowing(int userId)
        {
            List<int> followingUserIds =await _context.Follows
           .Where(f => f.FollowerId == userId)
           .Select(f => f.FollowingId)
           .ToListAsync();


            List<User> temp =await _context.Users
                .Where(u => followingUserIds.Contains(u.Id))
                .ToListAsync();
            return temp;
        }

        public async Task updatePassword (UpdatePassword updatePassword)
        {
            var _user =await _context.Users.FirstOrDefaultAsync(u => u.Id == updatePassword.Id);

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
              await _context.SaveChangesAsync();

            }

        }

        public async Task updateUserInfo (UpdateUserInfo user)
        {
            var _user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            bool usernameExist =await _context.Users.AnyAsync(u => u.Id != user.Id && u.Username.Equals(user.Username));

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
            _user.isPrivate = user.isPrivate;

          await _context.SaveChangesAsync();

        }

        public async Task lockUserAccount(int userId)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u=> u.Id== userId);
            if(user==null)
            {
                throw new Exception("User does not exist");
            }
            if (user.isLocked)
            {
                throw new Exception("This account has already been locked.");
            }
            user.isLocked = true;
           await _context.SaveChangesAsync();
        }

        public async Task unlockUserAccount(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            if (!user.isLocked)
            {
                throw new Exception("This account is unlocked.");
            }
            user.isLocked = false;
           await _context.SaveChangesAsync();
        }

        public async Task<List<int>> dashboardAnalytics()
        {
            List<int> analytics = new List<int>();
            int countUser =await _context.Users.Where(u=>u.Role==0).CountAsync();
            int countPosts =await _context.Posts.CountAsync();
            int countMessages =await _context.SupportMessages.CountAsync();
            int countReports =await _context.Reports.CountAsync();

            analytics.Add(countUser);
            analytics.Add(countPosts);
            analytics.Add(countMessages);
            analytics.Add(countReports);

            return analytics;
        }
    };
}

        
    
