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

        public async Task makeComment(CommentsDTO comment)
        {
            var _comment = new Comments()
            {
                UserId = comment.UserId,
                PostId = comment.PostId,
                comment = comment.comment
            };
            try
            {
              await  _context.Comments.AddAsync(_comment);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
          await  _context.SaveChangesAsync();

        }

        public async Task<List<object>> getComments(int postId)
        {
            var comments =await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .Select(c => new {
                    Id = c.Id,
                    userId = c.User.Id,
                    Username = c.User.Username,
                    ImagePath= c.User.ImagePath,
                    Comment = c.comment
                })
                .ToListAsync();

            return comments.Cast<object>().ToList();
        }
        public async Task<int> getNrComments(int postId)
        {
            int count = 0;
            List<Comments> comments =await _context.Comments.Where(p => p.PostId == postId).ToListAsync();
            for (int i = 0; i < comments.Count; i++)
            {
                count++;
            }
            return count;
        }

        public async Task deleteComment(int commentId)
        {
            var _comment =await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (_comment == null)
            {
                throw new Exception("Comment doesn't exist");
            }
            _context.Comments.Remove(_comment);
            await _context.SaveChangesAsync()
;        }

    }
}
