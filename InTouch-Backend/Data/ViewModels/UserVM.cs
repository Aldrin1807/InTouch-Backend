namespace InTouch_Backend.Data.ViewModels
{
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int role { get; set; } = 0;
        public string profile_img { get; set; }
    }
}
