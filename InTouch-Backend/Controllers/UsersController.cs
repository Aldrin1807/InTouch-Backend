using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _service;
        public UsersController(UsersService service)
        {
            _service = service;

        }
        [HttpPost("register")]
        public IActionResult register([FromForm] UserDTO user)
        {
            try
            {
                _service.register(user);
                return Ok(new Response
                { Status = "Success", Message = "User Successfully registered." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }


        [HttpPost("login")]
        public IActionResult login([FromBody] Login user)
        {
            try
            {
                var tokenString = _service.login(user);
                return Ok(new Response
                { Status = "Success", Message = tokenString });

            }
            catch(DataException dx)
            {
                return Ok(new Response
                { Status = "Locked", Message = dx.Message });
            }
            catch(Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
            
        }

        [HttpGet("get-user-id")]
        public IActionResult getId(string token)
        {
            return Ok(_service.GetUserIdFromToken(token));
        }


        
        [HttpGet("get-users") ,Authorize]
        public IActionResult GetUsers()
        {
            return Ok(_service.getUsers());
        }


        [HttpGet("get-user-info"),Authorize]
        public IActionResult GetUserInfo(int id)
        {
            return Ok(_service.getUserInfo(id));
        }

        [HttpGet("get-user-followers-follows"), Authorize]

        public IActionResult getFollows_and_Followers(int userId)
        {
            return Ok(_service.getFollows_and_Followers(userId));
        }
        [HttpPut("Update-user{id}"), Authorize]
        public IActionResult UpdateUser(int id, [FromForm] UserDTO updatedUser)
        {
            try
            {
                // Update user profile

                _service.updateProfile(id, updatedUser);

                // Return success response
                return Ok(new { message = "User profile updated successfully" });
            }
            catch (Exception ex)
            {
                // Return error response
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}"), Authorize]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _service.getUserById(id);

                if (user == null)
                {
                    return NotFound
                        (new { message = "User is not not found" });
                }


                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("suggested-users"), Authorize]
        public IActionResult suggestedUsers(int userId)
        {
            return Ok(_service.suggestedUsers(userId));
        }

        [HttpGet("search"), Authorize]
        public IActionResult searchUsers(int userId, string query)
        {
            return Ok(_service.searchUsers(userId, query));
        }

        [HttpGet("user-followers"), Authorize]

        public IActionResult userFollowers(int userId)
        {
            return Ok(_service.userFollowers(userId));
        }
        [HttpGet("user-following"), Authorize]

        public IActionResult userFollowing(int userId)
        {
            return Ok(_service.userFollowing(userId));
        }


        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _service.DeleteUser(id);
            if (deleted)
            {
                return Ok(new Response { Status = "Success", Message = "User deleted successfully." });
            }
            else
            {
                return Ok(new Response { Status = "Error", Message = "User not found." });
            }
        }


        [HttpGet("is-following"),Authorize]

        public IActionResult isFollowing(int userOne, int userTwo)
        {
            return Ok(_service.isFollowing(userOne, userTwo));
        }

        [HttpGet("is-token-available")]
        public IActionResult isTokenAvailable(string token)
        {
            return Ok(_service.isTokenAvailable(token));
        }

        [HttpPut("update-password"),Authorize]
        public IActionResult updatePassword([FromBody] UpdatePassword updatePassword)
        {
            try
            {
                _service.updatePassword(updatePassword);
                return Ok(new Response
                { Status = "Success", Message = "Password updated succesfully." });
            }
            catch (Exception ex)
            {
                // Return error response
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }


        }

        [HttpPut("update-user-info"),Authorize]
        public IActionResult updateUserInfo([FromBody] UpdateUserInfo user)
        {
            try
            {
                _service.updateUserInfo(user);
                return Ok(new Response
                { Status = "Success", Message = "Personal info updated succesfully." });
            }
            catch (Exception ex)
            {
                // Return error response
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }

        [HttpPut("update-profile-pic"),Authorize]
        public IActionResult updateProfilePicture([FromForm] UpdateProfilePic newPic)
        {
            try
            {
                _service.updateProfilePicture(newPic);
                return Ok(new Response
                { Status = "Success", Message = "Profile Picture updated succesfully." });
            }
            catch (Exception ex)
            {
                // Return error response
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }

        [HttpGet("users/{id}/savedposts")]
        public IActionResult GetSavedPosts(int id)
        {
            try { 
            List<Post> savedPosts = _service.GetSavedPosts(id);

            if (savedPosts == null || savedPosts.Count == 0)
            {
                return NotFound(); // No saved posts found, return a 404 Not Found status code
            }

            return Ok(savedPosts);} catch (Exception ex)
            {// Return error response
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }

        [HttpPost("send-support-message")]
        public IActionResult sendSupportMessage(SupportMessagesDTO messageDTO)
        {
            try
            {
                _service.sendSupportMessage(messageDTO);
                return Ok(new Response
                { Status = "Success", Message = "Message send succesfully.Our team will decide to unlock your account or not." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }

        }

        [HttpPut("lock-account")]
        public IActionResult lockUserAccount(int userId)
        {
            try
            {
                _service.lockUserAccount(userId);
                return Ok(new Response
                { Status = "Success", Message = "This account has been succesfully locked" });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }
        [HttpPut("unlock-account")]
        public IActionResult unlockUserAccount(int userId)
        {
            try
            {
                _service.unlockUserAccount(userId);
                return Ok(new Response
                { Status = "Success", Message = "This account has been succesfully unlocked" });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }


    }
    }


