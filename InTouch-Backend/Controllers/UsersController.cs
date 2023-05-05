using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult login([FromBody]Login user)
        {
            int log = _service.login(user);
            if (log!=0)
            {
                return Ok(log);
            }
            else
            {
                return Ok(new Response
                { Status = "Error", Message = "Credentials wrong." });
            }
        }

        [HttpGet("get-users")]
        public IActionResult GetUsers() {
            return Ok(_service.getUsers());
        }

        [HttpGet("get-user-info")]
        public IActionResult GetUserInfo(int id) {
            return Ok(_service.getUserInfo(id));
        }

        [HttpGet("get-user-followers-follows")]

        public IActionResult getFollows_and_Followers(int userId)
        {
            return Ok(_service.getFollows_and_Followers(userId));
        }

        [HttpDelete("{id}")]
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
        }
    }

