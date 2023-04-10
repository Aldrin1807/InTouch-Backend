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
        public IActionResult makePost([FromBody]PostVM post)
        {
            _service.makePost(post);
            return Ok();
        }
    }
}
