namespace InTouch_Backend.Data.Models
{
    public class SavedPost
    {
        public int UserId { get; set; }
        public User User { get; set; }


        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
