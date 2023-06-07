using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

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
        [HttpPost("make-post"), Authorize]
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
        [HttpGet("get-posts"), Authorize]
        public IActionResult getFollowedPosts(int id)
        {
            List<Post> Posts = _service.getFollowedPosts(id);
            Posts.Reverse();
            return Ok(Posts);
        }

        [HttpGet("check-deleted-post"), Authorize]
        public IActionResult checkDeletedPosts(int userId)
        {
            try
            {
                _service.checkDeletedPosts(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("get-user-post-info"), Authorize]

        public IActionResult getUserPostInfo(int postId)
        {
            return Ok(_service.getUserPostInfo(postId));
        }

        [HttpGet("get-user-post"), Authorize]

        public IActionResult getUserPosts(int userId)
        {
            return Ok(_service.getUserPosts(userId));
        }

        [HttpDelete("delete-post"), Authorize]

        public IActionResult deletePost(int postId)
        {
            _service.deletePost(postId);
            return Ok();
        }


        [HttpGet("get-post-info"), Authorize]
        public IActionResult getPostInfo(int postId)
        {
            return Ok(_service.getPostInfo(postId));
        }

        [HttpPut("set-delete-attr"), Authorize]
        public IActionResult setDeleteTrue(int postId)
        {
            try
            {
                _service.setDeleteTrue(postId);
                return Ok(new Response
                { Status = "Success", Message = "The post will be deleted when the user is logged in." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }
    }
}
