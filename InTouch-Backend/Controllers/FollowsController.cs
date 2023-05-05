using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowsController : ControllerBase
    {
        private readonly FollowsService _service;

        public FollowsController(FollowsService service){
            _service = service;
        }

        [HttpPost("follow-user")]
        public IActionResult followUser([FromBody]FollowsDTO follow)
        {
            _service.followUser(follow);
            return Ok();
        }

        [HttpDelete("unfollow-user")]
        public IActionResult unFollowUser([FromBody] FollowsDTO follow)
        {
            _service.unFollowUser(follow);
            return Ok();
        }

    }
}
