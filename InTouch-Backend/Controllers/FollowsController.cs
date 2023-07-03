using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("follow-user"), Authorize]
        public async Task<IActionResult> followUser([FromBody]FollowsDTO follow)
        {
          await  _service.followUser(follow);
            return Ok();
        }

        [HttpDelete("unfollow-user"), Authorize]
        public async Task<IActionResult> unFollowUser([FromBody] FollowsDTO follow)
        {
           await _service.unFollowUser(follow);
            return Ok();
        }

    }
}
