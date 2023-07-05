using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class SavedPostServices
    {
        private readonly AppDbContext _context;

        public SavedPostServices(AppDbContext context)
        {
            _context = context;
        }

         public async Task SavePost(SavedPostsDTO Post)
         {
            var _post = new SavedPost()
            {
                PostId = Post.PostId,
                UserId = Post.UserId,
            };
          await   _context.SavedPosts.AddAsync(_post);
           await _context.SaveChangesAsync();
         }

        public async Task unSavePost(SavedPostsDTO like)
        {
            var _post = await _context.SavedPosts.FirstOrDefaultAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
            if (_post != null)
            {
                    _context.Remove(_post);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> isSaved(SavedPostsDTO like)
        {
            return await _context.SavedPosts.AnyAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
        }

        public async Task<List<Post>> getSavedPosts(int userId)
        {
            var savedPostsID =await _context.SavedPosts.Where(u => u.UserId == userId).Select(i => i.PostId)
                .ToListAsync();
            List<Post> savedPosts =await _context.Posts.Where(p => savedPostsID.Contains(p.Id)).ToListAsync();
            return savedPosts;
        }

    }
}
