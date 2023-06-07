using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly LikesService _likesService;

        public LikesController(LikesService likesService)
        {
            _likesService = likesService;   
        }

        [HttpGet("get-post-likes"),Authorize]
        public IActionResult getPostLike(int id)
        {
            return Ok(_likesService.getPostLike(id));
        }

        [HttpPost("like-post"), Authorize]
        public IActionResult likePost([FromBody]LikesDTO likes)
        {
            _likesService.likePost(likes);
            return Ok();
        }

        [HttpDelete("unlike-post"), Authorize]

        public IActionResult unLikePost([FromBody]LikesDTO likes) { 
            _likesService.unLikePost(likes);
            return Ok();
        }

        [HttpGet("is-liked"), Authorize]

        public IActionResult isLiked(int userId,int postId)
        {
            var likes = new LikesDTO()
            {
                PostId = postId,
                UserId = userId
            };
            return Ok(_likesService.isLiked(likes));
        }
    }
}
