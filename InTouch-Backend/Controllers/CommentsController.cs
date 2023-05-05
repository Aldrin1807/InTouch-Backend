using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("get-post-comments")]
        public IActionResult getComments(int postId)
        {
            return Ok(_commentsService.getComments(postId));
        }

        [HttpPost("make-comment")]

        public IActionResult makeComment(CommentsDTO comment)
        {
            _commentsService.makeComment(comment);
            return Ok();
        }

        [HttpGet("get-nr-comments")]
        public IActionResult getNrComments(int postId)
        {
            return Ok(_commentsService.getNrComments(postId));
        }
    }
}
