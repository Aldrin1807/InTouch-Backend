namespace InTouch_Backend.Data.Models
{
    public class FollowRequests
    {
        public int FollowRequestId { get; set; }
        public User FollowRequest { get; set; }

        public int FollowRequestedId { get; set; }
        public User FollowRequested { get; set; }
    }
}
