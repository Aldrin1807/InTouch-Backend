using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;

namespace InTouch_Backend.Data.Services
{
    public class PostService
    {
        public AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public void makePost(PostVM post)
        {
            var _post = new Post()
            {
                Content=post.Content,
                ImageURL=post.ImageURL,
                userID=post.userID
            };
            _context.Posts.Add(_post);
            _context.SaveChanges();
        }
    }
}
