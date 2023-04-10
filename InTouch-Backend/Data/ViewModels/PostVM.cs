namespace InTouch_Backend.Data.ViewModels
{
    public class PostVM
    {
        public string Content { get; set; }
        public string ImageURL { get; set; }

        public DateTime PostDate { get; set; }

        //Navigation Properties
        public int userID { get; set; }
    }
}
