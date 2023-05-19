using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
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



        [HttpPost("save-post")]
        public IActionResult SavePost([FromBody] SavedPostsDTO savePost)
        {
            _savedPostServices.SavePost(savePost);
                return Ok();
        }

            [HttpDelete("unsave-post")]

            public IActionResult unSavePost([FromBody] SavedPostsDTO savePost)
            {
            _savedPostServices.unSavePost(savePost);
                return Ok();
            }

            [HttpGet("is-liked")]

            public IActionResult isSaved(int userId, int postId)
            {
                var savePost = new SavedPostsDTO()
                {
                    PostId = postId,
                    UserId = userId
                };
                return Ok(_savedPostServices.isSaved(savePost));
            }
        }

    
}
