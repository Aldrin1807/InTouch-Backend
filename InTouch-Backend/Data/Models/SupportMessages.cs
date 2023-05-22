namespace InTouch_Backend.Data.Models
{
    public class SupportMessages
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string message { get; set; }
    }
}
