using InTouch_Backend.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        [HttpPost]
        public IActionResult report([FromBody] Reports r)
        {
            return Ok();
        }
    }
}
