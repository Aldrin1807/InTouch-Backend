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
        public bool isLocked { get; set; }

    }
    public class UpdatePassword
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }

    public class UpdateUserInfo
    {
        public int Id { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool isPrivate { get; set; }
    }

    public class UpdateProfilePic
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}

