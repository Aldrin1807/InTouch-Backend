namespace InTouch_Backend.Data.DTOs
{
    public class Comment
    {
        public int userId { get; set; }
        public string username { get; set; }

        public string imagePath { get; set; }
        public string comment { get; set; }
    }
}
