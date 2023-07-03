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
        public async Task<IActionResult> getPostLike(int id)
        {
            return Ok(_likesService.getPostLike(id));
        }

        [HttpPost("like-post"), Authorize]
        public async Task<IActionResult> likePost([FromBody]LikesDTO likes)
        {
            await _likesService.likePost(likes);
            return Ok();
        }

        [HttpDelete("unlike-post"), Authorize]

        public async Task<IActionResult> unLikePost([FromBody]LikesDTO likes) { 
          await  _likesService.unLikePost(likes);
            return Ok();
        }

        [HttpGet("is-liked"), Authorize]

        public async Task<IActionResult> isLiked(int userId,int postId)
        {
            var likes = new LikesDTO()
            {
                PostId = postId,
                UserId = userId
            };
            return Ok(await _likesService.isLiked(likes));
        }
    }
}
