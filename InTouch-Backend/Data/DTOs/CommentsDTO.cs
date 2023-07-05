using InTouch_Backend.Data.Models;

namespace InTouch_Backend.Data.DTOs
{
    public class CommentsDTO
    {
        public int UserId { get; set; }

        public int PostId { get; set; }
        public string comment { get; set; }
    }
}
