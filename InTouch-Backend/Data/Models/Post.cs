namespace InTouch_Backend.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }  
        public string ImageURL { get; set; }

        //Navigation Properties
        public int userID { get; set; }
        public User User { get; set; }
    }
}
