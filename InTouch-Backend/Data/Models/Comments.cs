using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Models
{
    public class Comments
    {

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        public int PostId { get; set; }
        public Post Post { get; set; }

        public string comment { get; set; }



    }
}
