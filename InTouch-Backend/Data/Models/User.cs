using System.Text.Json.Serialization;

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
        public string profile_img { get; set; }
        public int role { get; set; } = 0;//Default 0 per user, per admin 1


        //Navigation Properties
        [JsonIgnore]
        public List<Post> Posts { get; set; }
        [JsonIgnore]
        public List<Likes> Likes { get; set; }
        [JsonIgnore]
        public List<Comments> Comments { get; set; }
        [JsonIgnore]
        public List<Reports> Reports { get; set; }
        [JsonIgnore]
        public List<Follows> Followers { get; set; }
        [JsonIgnore]
        public List<Follows> Following { get; set; }
    }
}
