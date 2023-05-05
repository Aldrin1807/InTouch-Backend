using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class CommentsService
    {
        private readonly AppDbContext _context;

        public CommentsService(AppDbContext context)
        {
            _context = context;
        }

        public void makeComment(CommentsDTO comment)
        {
            var _comment = new Comments()
            {
                UserId = comment.UserId,
                PostId = comment.PostId,
                comment = comment.comment
            };
            try
            {
                _context.Comments.Add(_comment);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            _context.SaveChanges();

        }

        public List<object> getComments(int postId)
        {
            var comments = _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .Select(c => new {
                    userId = c.User.Id,
                    Username = c.User.Username,
                    Comment = c.comment
                })
                .ToList();

            return comments.Cast<object>().ToList();
        }
        public int getNrComments(int postId)
        {
            int count = 0;
            List<Comments> comments = _context.Comments.Where(p => p.PostId == postId).ToList();
            for (int i = 0; i < comments.Count; i++)
            {
                count++;
            }
            return count;
        }

    }
}
