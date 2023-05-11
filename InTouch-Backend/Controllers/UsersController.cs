using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var tokenString = _service.login(user);

            if (tokenString != null)
            {
                return Ok(tokenString);
            }
            else
            {
                return BadRequest(new Response
                { Status = "Error", Message = "Credentials wrong." });
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



    }
    }


