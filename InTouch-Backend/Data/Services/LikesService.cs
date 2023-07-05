using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InTouch_Backend.Data.Services
{
    public class LikesService
    {
        private readonly AppDbContext _context;

        public LikesService(AppDbContext context) {

            _context = context;
        }

        public int getPostLike(int postID)
        {
            int likes = 0;

            foreach(var like in _context.Likes) { 
                if(like.PostId == postID){
                    likes++;
                }
            }
            return likes;
        }

        public async Task<bool> isLiked(LikesDTO like)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
        }

        public async Task likePost(LikesDTO like) {
            var _like = new Likes()
            {
                UserId= like.UserId,
                PostId = like.PostId
            };
           await _context.Likes.AddAsync(_like);
           await _context.SaveChangesAsync();
        }

        public async Task unLikePost(LikesDTO like)
        {
            var _like =await _context.Likes.FirstOrDefaultAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
            if (_like != null)
            {
                _context.Remove(_like);
               await _context.SaveChangesAsync();
            }
        }

    }
}
