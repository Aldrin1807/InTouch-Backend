namespace InTouch_Backend.Data.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }   
        public string LastName { get; set; }    
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int role { get; set; } = 0;//Default 0 per user, per admin 1


        //Navigation Properties
        public List<Post> Posts { get; set; }
        public List<UserPostLike> Likes { get; set; }
    }
}
