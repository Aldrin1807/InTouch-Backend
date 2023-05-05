using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        public PostService _service;
        public PostsController(PostService service)
        {
            _service = service; 
        }
        [HttpPost("make-post")]
        public IActionResult makePost([FromForm]PostDTO post)
        {
            try
            {
                _service.makePost(post);
                return Ok(new Response
                { Status = "Success", Message = "Post made succesfully." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
            
        }
        [HttpGet("get-posts")]
        public IActionResult getFollowedPosts(int id)
        {
            List<Post> Posts = _service.getFollowedPosts(id);
            Posts.Reverse();
            return Ok(Posts);
        }

        [HttpGet("get-user-post-info")]

        public IActionResult getUserPostInfo(int postId)
        {
            return Ok(_service.getUserPostInfo(postId));
        }

        [HttpGet("get-user-post")]

        public IActionResult getUserPosts(int userId)
        {
            return Ok(_service.getUserPosts(userId));
        }

        [HttpDelete("delete-post")]

        public IActionResult deletePost(int postId)
        {
            _service.deletePost(postId);
            return Ok();
        }
    }
}
