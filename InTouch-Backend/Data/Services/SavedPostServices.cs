using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;

namespace InTouch_Backend.Data.Services
{
    public class SavedPostServices
    {
        private readonly AppDbContext _context;

        public SavedPostServices(AppDbContext context)
        {
            _context = context;
        }

       /* public SavePost(SavedPostsDTO Post)
        {
            var post = new SavedPost()
            {
                PostId = Post.PostId,
                UserId = Post.UserId,
            }
            return post;
        }*/
    }
}
