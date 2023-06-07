using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowRequestsController : ControllerBase
    {
        private readonly FollowRequestsService _service;

        public FollowRequestsController(FollowRequestsService service)
        {
            _service = service;
        }

        [HttpPost("request-follow"),Authorize]
        public IActionResult requestFollow([FromBody] FollowRequestsDTO request)
        {
            _service.requestFollow(request);
            return Ok();
        }

        [HttpDelete("unrequest-follow"), Authorize]
        public IActionResult unRequestFollow([FromBody] FollowRequestsDTO request)
        {
            _service.unRequestFollow(request);
            return Ok();
        }

        [HttpGet("is-requested"), Authorize]
        public IActionResult isRequested(int userOne,int userTwo)
        {
            FollowRequestsDTO request = new FollowRequestsDTO()
            {
                FollowRequestId = userOne,
                FollowRequestedId = userTwo
            };
            return Ok(_service.isRequested(request));
        }

        [HttpGet("get-requests"), Authorize]
        public IActionResult getUserRequests(int userId)
        {
            return Ok(_service.getUserRequests(userId));
        }
        [HttpPost("handle-accept"), Authorize]
        public IActionResult handleAccept([FromBody] FollowRequestsDTO request)
        {
            _service.handleAccept(request);
            return Ok();
        }
        [HttpDelete("handle-decline"), Authorize]
        public IActionResult handleDecline([FromBody] FollowRequestsDTO request)
        {
            _service.handleDecline(request);
            return Ok();
        }
    } 
}
