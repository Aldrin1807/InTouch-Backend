using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTouch_Backend.Data.ViewModels
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isPrivate { get; set; }
        public IFormFile Image{ get; set; }
        public int Role { get; set; } = 0;
    }
}

namespace InTouch_Backend
{
   public class UpdatePassword
    {
        public int Id{ get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
       
    }
}