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
        public async Task<IActionResult> requestFollow([FromBody] FollowRequestsDTO request)
        {
           await _service.requestFollow(request);
            return Ok();
        }

        [HttpDelete("unrequest-follow"), Authorize]
        public async Task<IActionResult> unRequestFollow([FromBody] FollowRequestsDTO request)
        {
           await _service.unRequestFollow(request);
            return Ok();
        }

        [HttpGet("is-requested"), Authorize]
        public async Task<IActionResult> isRequested(int userOne,int userTwo)
        {
            FollowRequestsDTO request = new FollowRequestsDTO()
            {
                FollowRequestId = userOne,
                FollowRequestedId = userTwo
            };
            return Ok(await _service.isRequested(request));
        }

        [HttpGet("get-requests"), Authorize]
        public async Task<IActionResult> getUserRequests(int userId)
        {
            return Ok(await _service.getUserRequests(userId));
        }
        [HttpPost("handle-accept"), Authorize]
        public async Task<IActionResult> handleAccept([FromBody] FollowRequestsDTO request)
        {
            await _service.handleAccept(request);
            return Ok();
        }
        [HttpDelete("handle-decline"), Authorize]
        public async Task<IActionResult> handleDecline([FromBody] FollowRequestsDTO request)
        {
            try { 
                await _service.handleDecline(request);
                return Ok(new Response { Status = "Success", Message = "Request deleted successfully." });
            } catch(Exception e)
            {
                return Ok(new Response { Status = "Error", Message = e.Message });
            }
        }
    } 
}
