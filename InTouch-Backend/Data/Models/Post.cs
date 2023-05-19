using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InTouch_Backend.Data.Models
{
    public class Post
    {

        public int Id { get; set; }
        public string Content { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        public string PostDate { get; set; }

        public int userID { get; set; }

        //Navigation Properties


        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Likes> Likes { get; set; }
        [JsonIgnore]

        public List<SavedPost> SavedPosts { get; set; }
        [JsonIgnore]
        public List<Comments> Comments { get; set; }
        [JsonIgnore]
        public List<Reports> Reports { get; set; }

       
    }
}
