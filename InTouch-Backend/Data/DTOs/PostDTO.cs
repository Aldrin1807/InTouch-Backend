namespace InTouch_Backend.Data.DTOs
{
    public class PostDTO
    {
        public string Content { get; set; }
        public IFormFile? Image { get; set; }


        //Navigation Properties
        public int userID { get; set; }
    }
}
