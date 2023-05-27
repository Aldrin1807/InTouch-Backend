namespace InTouch_Backend.Data.DTOs
{
    public class SupportMessagesDTO
    {
        public string UsernameOrEmail { get; set; }
        public string message { get; set; }
    }
    public class DeleteSupportMessagesDTO
    {
        
        public int Id { get; set; }
    }
}
