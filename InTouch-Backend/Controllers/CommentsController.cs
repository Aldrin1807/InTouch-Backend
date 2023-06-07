using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _commentsService;

        public CommentsController(CommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpGet("get-post-comments"), Authorize]
        public IActionResult getComments(int postId)
        {
            return Ok(_commentsService.getComments(postId));
        }

        [HttpPost("make-comment"), Authorize]

        public IActionResult makeComment(CommentsDTO comment)
        {
            _commentsService.makeComment(comment);
            return Ok();
        }

        [HttpGet("get-nr-comments"), Authorize]
        public IActionResult getNrComments(int postId)
        {
            return Ok(_commentsService.getNrComments(postId));
        }
        [HttpDelete("delete-comment"), Authorize]
        public IActionResult deleteComment(int id) {
            try
            {
                _commentsService.deleteComment(id);
                return Ok(new Response
                { Status = "Success", Message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }
    }
}
