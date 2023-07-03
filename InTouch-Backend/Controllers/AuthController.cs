using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsersService _service;

        public AuthController(UsersService service)
        {
            _service= service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] UserDTO user)
        {
            try
            {
               await _service.register(user);
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
        public async Task<IActionResult> login([FromBody] Login user)
        {
            try
            {
                var tokenString = await _service.login(user);
                return Ok(new Response
                { Status = "Success", Message = tokenString });

            }
            catch (DataException dx)
            {
                return Ok(new Response
                { Status = "Locked", Message = dx.Message });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }

        }



    }
}
