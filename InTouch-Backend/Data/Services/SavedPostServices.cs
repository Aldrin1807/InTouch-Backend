using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;

namespace InTouch_Backend.Data.Services
{
    public class SavedPostServices
    {
        private readonly AppDbContext _context;

        public SavedPostServices(AppDbContext context)
        {
            _context = context;
        }

         public void SavePost(SavedPostsDTO Post)
         {
            var _post = new SavedPost()
            {
                PostId = Post.PostId,
                UserId = Post.UserId,
            };
             _context.SavedPosts.Add(_post);
            _context.SaveChanges();
         }

        public void unSavePost(SavedPostsDTO like)
        {
            var _post = _context.SavedPosts.FirstOrDefault(l => l.UserId == like.UserId && l.PostId == like.PostId);
            if (_post != null)
            {
                _context.Remove(_post);
                _context.SaveChanges();
            }
        }
        public bool isSaved(SavedPostsDTO like)
        {
            return _context.SavedPosts.Any(l => l.UserId == like.UserId && l.PostId == like.PostId);
        }

        public List<Post> getSavedPosts(int userId)
        {
            var savedPostsID = _context.SavedPosts.Where(u => u.UserId == userId).Select(i => i.PostId)
                .ToList();
            List<Post> savedPosts = _context.Posts.Where(p => savedPostsID.Contains(p.Id)).ToList();
            return savedPosts;
        }

    }
}
