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

        [HttpGet("get-user-info"),Authorize]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            return Ok( await _service.getUserInfo(id));
        }

        [HttpGet("get-user-followers-follows"), Authorize]

        public async Task<IActionResult> getFollows_and_Followers(int userId)
        {
            return Ok(await _service.getFollows_and_Followers(userId));
        }
       
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _service.getUserById(id);

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
        public async Task<IActionResult> suggestedUsers(int userId)
        {
            return Ok(await _service.suggestedUsers(userId));
        }

        [HttpGet("search"), Authorize]
        public async Task<IActionResult> searchUsers(int userId, string query)
        {
            return Ok(await _service.searchUsers(userId, query));
        }

        [HttpGet("user-followers"), Authorize]

        public async Task<IActionResult> userFollowers(int userId)
        {
            return Ok(await _service.userFollowers(userId));
        }
        [HttpGet("user-following"), Authorize]

        public async Task<IActionResult> userFollowing(int userId)
        {
            return Ok(await _service.userFollowing(userId));
        }


        [HttpDelete("{id}"), Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _service.DeleteUser(id);
            if (deleted)
            {
                return Ok(new Response { Status = "Success", Message = "User deleted successfully." });
            }
            else
            {
                return Ok(new Response { Status = "Error", Message = "User not found." });
            }
        }
        [HttpGet("get-users"), Authorize(Roles = "1")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _service.getUsers());
        }

        [HttpGet("is-following"),Authorize]

        public async Task<IActionResult> isFollowing(int userOne, int userTwo)
        {
            return Ok(await _service.isFollowing(userOne, userTwo));
        }

        [HttpGet("is-token-available")]
        public async Task<IActionResult> isTokenAvailable(string token)
        {
            return Ok(_service.isTokenAvailable(token));
        }

        [HttpPut("update-password"),Authorize]
        public async Task<IActionResult> updatePassword([FromBody] UpdatePassword updatePassword)
        {
            try
            {
                await _service.updatePassword(updatePassword);
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
        public async Task<IActionResult> updateUserInfo([FromBody] UpdateUserInfo user)
        {
            try
            {
                await _service.updateUserInfo(user);
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
        public async Task<IActionResult> updateProfilePicture([FromForm] UpdateProfilePic newPic)
        {
            try
            {
                await _service.updateProfilePicture(newPic);
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

    

        [HttpPut("lock-account"), Authorize(Roles = "1")]
        public async Task<IActionResult> lockUserAccount(int userId)
        {
            try
            {
                await _service.lockUserAccount(userId);
                return Ok(new Response
                { Status = "Success", Message = "This account has been succesfully locked" });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }
        [HttpPut("unlock-account"), Authorize(Roles = "1")]
        public async Task<IActionResult> unlockUserAccount(int userId)
        {
            try
            {
                await _service.unlockUserAccount(userId);
                return Ok(new Response
                { Status = "Success", Message = "This account has been succesfully unlocked" });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Failed", Message = ex.Message });
            }
        }

        [HttpGet("dashboard-analytics"),Authorize(Roles = "1")]
        public async Task<IActionResult> dashboardAnalytics()
        {
            return Ok(await _service.dashboardAnalytics());
        }

    }
    }


