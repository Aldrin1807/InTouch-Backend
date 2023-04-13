using System.Text.Json.Serialization;

namespace InTouch_Backend.Data.Models
{
    public class Post
    {

        public int Id { get; set; }
        public string Content { get; set; }  
        public string ImageURL { get; set; }
        public DateTime PostDate { get; set; }

        //Navigation Properties
        [JsonIgnore]
        public int userID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Likes> Likes { get; set; }
        [JsonIgnore]
        public List<Comments> Comments { get; set; }
        [JsonIgnore]
        public List<Reports> Reports { get; set; }
    }
}
