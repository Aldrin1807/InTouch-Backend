﻿using InTouch_Backend.Data.Services;
using InTouch_Backend.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _service;
        public UsersController(UsersService service)
        {
            _service = service;
          
        }
        [HttpPost("register")]
        public IActionResult register([FromForm] UserDTO user)
        {
            try
            {   
                _service.register(user);
                return Ok(new Response
                { Status = "Success", Message = "User Successfully registered." });
            }
            catch (Exception ex)
            {
                return Ok(new Response
                { Status = "Error", Message = ex.Message });
            }
        }
       
       
        [HttpPost("login")]
        public IActionResult login([FromBody]Login user)
        {
            int log = _service.login(user);
            if (log!=0)
            {
                return Ok(log);
            }
            else
            {
                return Ok(new Response
                { Status = "Error", Message = "Credentials wrong." });
            }
        }

        [HttpGet("get-users")]
        public IActionResult GetUsers() {
            return Ok(_service.getUsers());
        }

        [HttpPut("Update-user{id}")]
        public IActionResult UpdateUser(int id, [FromForm] UserDTO updatedUser)
        {
            try
            {
                // Update user profile
                
                _service.updateProfile(id, updatedUser);

                // Return success response
                return Ok(new { message = "User profile updated successfully" });
            }
            catch (Exception ex)
            {
                // Return error response
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _service.getUserById(id);    

                if (user == null)
                {
                    return NotFound
                        (new { message = "User is not not found" });
                }
                
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}
