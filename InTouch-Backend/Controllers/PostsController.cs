using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using InTouch_Backend.Data.DTOs;

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
        public async Task<IActionResult> makePost([FromForm]PostDTO post)
        {
            try
            {
               await _service.makePost(post);
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
        public async Task<IActionResult> getFollowedPosts(int id)
        {
            List<Post> Posts =await _service.getFollowedPosts(id);
            Posts.Reverse();
            return Ok(Posts);
        }

        [HttpGet("check-deleted-post"), Authorize]
        public async Task<IActionResult> checkDeletedPosts(int userId)
        {
            try
            {
               await _service.checkDeletedPosts(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("get-user-post-info"), Authorize]

        public async Task<IActionResult> getUserPostInfo(int postId)
        {
            return Ok(await _service.getUserPostInfo(postId));
        }

        [HttpGet("get-user-post"), Authorize]

        public async Task<IActionResult> getUserPosts(int userId)
        {
            return Ok(await _service.getUserPosts(userId));
        }

        [HttpDelete("delete-post"), Authorize]

        public async Task<IActionResult> deletePost(int postId)
        {
          await  _service.deletePost(postId);
            return Ok();
        }


        [HttpGet("get-post-info"), Authorize]
        public async Task<IActionResult> getPostInfo(int postId)
        {
            return Ok(_service.getPostInfo(postId));
        }

        [HttpPut("set-delete-attr"), Authorize]
        public async Task<IActionResult> setDeleteTrue(int postId)
        {
            try
            {
                await _service.setDeleteTrue(postId);
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
