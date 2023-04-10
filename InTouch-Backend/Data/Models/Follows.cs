namespace InTouch_Backend.Data.Models
{
    public class Follows
    {
        public int FollowerId { get; set; }
        public User Follower { get; set; }

        public int FollowingId { get; set; }
        public User Following { get; set; }
    }
}
