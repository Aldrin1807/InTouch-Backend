using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    
    
        [Route("api/[controller]")]
        [ApiController]
        public class SavedPostsController : ControllerBase
        {
            private readonly SavedPostServices _savedPostServices;

            public SavedPostsController(SavedPostServices savedPostServices)
            {
                _savedPostServices = savedPostServices;
            }



        [HttpPost("save-post"),Authorize]
        public async Task<IActionResult> SavePost([FromBody] SavedPostsDTO savePost)
        {
          await _savedPostServices.SavePost(savePost);
                return Ok();
        }

            [HttpDelete("unsave-post"),Authorize]

            public async Task<IActionResult> unSavePost([FromBody] SavedPostsDTO savePost)
            {
            await _savedPostServices.unSavePost(savePost);
                return Ok();
            }

            [HttpGet("is-saved"),Authorize]

            public async Task<IActionResult> isSaved(int userId, int postId)
            {
                var savePost = new SavedPostsDTO()
                {
                    PostId = postId,
                    UserId = userId
                };
                return Ok(await _savedPostServices.isSaved(savePost));
            }

        [HttpGet("get-saved-posts"), Authorize]
        public async Task<IActionResult> getSavedPosts(int userId)
        {
            return Ok(await _savedPostServices.getSavedPosts(userId));
        }
        }

    
}
