using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
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

        public bool isLiked(LikesDTO like)
        {
            return _context.Likes.Any(l => l.UserId == like.UserId && l.PostId == like.PostId);
        }

        public void likePost(LikesDTO like) {
            var _like = new Likes()
            {
                UserId= like.UserId,
                PostId = like.PostId
            };
            _context.Likes.Add(_like);
            _context.SaveChanges();
        }

        public void unLikePost(LikesDTO like)
        {
            var _like = _context.Likes.FirstOrDefault(l => l.UserId == like.UserId && l.PostId == like.PostId);
            if (_like != null)
            {
                _context.Remove(_like);
                _context.SaveChanges();
            }
        }

    }
}
