using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService= reportsService;
        }

        [HttpPost]
        public IActionResult report([FromBody]ReportsDTO report)
        {
            _reportsService.makeReport(report);
            return Ok();
        }
    }
}
