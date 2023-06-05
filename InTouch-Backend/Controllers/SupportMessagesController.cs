using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
        [ApiController]
    public class SupportMessagesController : ControllerBase
    {
        
        
        
            private readonly SupportMessageService _supportMessage;

            public SupportMessagesController(SupportMessageService supportMessage)
            {
            _supportMessage = supportMessage;
            }

            

            [HttpGet("get-support-messages")]
            public IActionResult getSupportMessages()
            {
            return Ok(_supportMessage.getSupportMessages());
            }

            [HttpDelete("delete-support-message")]
            public IActionResult deleteReport([FromBody] DeleteSupportMessagesDTO messages)
            {
                try
                {
                    _supportMessage.deleteSupportMessage(messages);
                    return Ok(new Response
                    { Status = "Success", Message = "Message deleted succesfully." });
                }
                catch (Exception ex)
                {
                    return Ok(new Response
                    { Status = "Error", Message = ex.Message });
                }
            }

                [HttpPost("send-support-message")]
                public IActionResult sendSupportMessage(SupportMessagesDTO messageDTO)
                {
                    try
                    {
                        _supportMessage.sendSupportMessage(messageDTO);
                        return Ok(new Response
                        { Status = "Success", Message = "Message send succesfully.Our team will decide to unlock your account or not." });
                    }
                    catch (Exception ex)
                    {
                        return Ok(new Response
                        { Status = "Failed", Message = ex.Message });
                    }

        }
    }
    
}
