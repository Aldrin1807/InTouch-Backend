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
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService= reportsService;
        }

        [HttpPost("make-report"),Authorize]
        public async Task<IActionResult> makeReport([FromBody]ReportsDTO report)
        {
            try
            {
               await _reportsService.makeReport(report);
                return Ok(new Response
                { Status = "Success", Message = "Post reported succesfully." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message ="You already reported this" });
            }
        }

        [HttpGet("get-reports"), Authorize]
        public async Task<IActionResult> getReports()
        {
            return Ok(await _reportsService.getReports());
        }

        [HttpDelete("delete-report"), Authorize]
        public async Task<IActionResult> deleteReport([FromBody] ReportsDTO report) {
            try
            {
               await _reportsService.deleteReport(report);
                return Ok(new Response
                { Status = "Success", Message = "Report deleted succesfully." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }
    }
}
