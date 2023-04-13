namespace InTouch_Backend.Data.ViewModels
{
    public class PostVM
    {
        public string Content { get; set; }
        public string ImageURL { get; set; }


        //Navigation Properties
        public int userID { get; set; }
    }
}
