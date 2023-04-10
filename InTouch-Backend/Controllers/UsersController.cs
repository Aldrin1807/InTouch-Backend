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
        public UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }
        [HttpPost("add-user")]
        public IActionResult addUser([FromBody]UserVM user)
        {
            _service.addUser(user);
            return Ok();
        }

        [HttpGet("get-users")]
        public IActionResult GetUsers() {
            return Ok(_service.getUsers());
        }
    }
}
