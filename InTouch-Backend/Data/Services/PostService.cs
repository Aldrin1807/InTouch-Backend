using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;

namespace InTouch_Backend.Data.Services
{
    public class PostService
    {
        public AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

         public void makePost(PostDTO post)
         {
             var _post = new Post()
             {
                 Content = post.Content,
                 PostDate=DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt"),
                 userID=post.userID
             };

             if (post.Image != null)
             {
                 string uniqueFileName = Guid.NewGuid().ToString() + "_" + post.Image.FileName;
                 string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Post Images");

                 if (!Directory.Exists(folderPath))
                 {
                     Directory.CreateDirectory(folderPath);
                 }


                 string filePath = Path.Combine(folderPath, uniqueFileName);
                 using (var fileStream = new FileStream(filePath, FileMode.Create))
                 {
                     post.Image.CopyTo(fileStream);
                 }

                 _post.ImagePath = uniqueFileName;
             }
             else
             {
                 _post.ImagePath = "";
             }


             _context.Posts.Add(_post);
             _context.SaveChanges();
         } 

        
       /* public void makePost(PostDTO post)
        {
            try
            {
                var _post = new Post()
                {
                    Content = post.Content,
                    PostDate = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt"),
                    userID = post.userID
                };

                if (post.Image != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + post.Image.FileName;
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Post Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        post.Image.CopyTo(fileStream);
                    }

                    _post.ImagePath = uniqueFileName;
                }
                else
                {
                    _post.ImagePath = "";
                }

                _context.Posts.Add(_post);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }

        }*/

        public void checkDeletedPosts (int userId)
        {
            List<int> userPosts = _context.Posts.Where(p => p.userID == userId && p.isDeleted).Select(p=>p.Id).ToList();
            if (userPosts.Count>0)
            {
               foreach(int id in userPosts)
                {
                    deletePost(id);
                }
                throw new Exception("Please don't use harmful words on your posts, it may cause your account to be locked or deleted.");
            }
        }
        public List<Post> getFollowedPosts(int userId)
            {
                var followedUserIds = _context.Follows
                    .Where(f => f.FollowerId == userId || f.FollowingId == userId)
                    .Select(f => f.FollowerId == userId ? f.FollowingId : f.FollowerId)
                    .ToList();

                var posts = _context.Posts
                    .Where(p => followedUserIds.Contains(p.userID))
                    .ToList();
                return posts;
            }

        public User getUserPostInfo(int postId)
        {

            var PostInfo = _context.Posts.FirstOrDefault(u => u.Id == postId);
            var UserInfo = _context.Users.FirstOrDefault(u => u.Id == PostInfo.userID);

            return UserInfo;
        }

        public List<Post> getUserPosts(int userId)
        {

            var posts = _context.Posts.Where(p=> p.userID==userId).ToList();
            posts.Reverse();
            return posts;
        }

        public void deletePost(int postId)
        {
            var post= _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
              
                var postComments = _context.Comments.Where(c => c.PostId == postId);
                _context.Comments.RemoveRange(postComments);

                var postLikes = _context.Likes.Where(l => l.PostId == postId);
                _context.Likes.RemoveRange(postLikes);

                var postReports = _context.Reports.Where(r => r.PostId == postId);
                _context.Reports.RemoveRange(postReports);

                _context.Posts.Remove(post);
            }
            _context.SaveChanges();
        }

        public Post getPostInfo(int postId)
        {
            return (_context.Posts.FirstOrDefault(p => p.Id == postId));
        }

        public void setDeleteTrue(int postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if(post == null)
            {
                throw new Exception("There is no post with this id");
            }
            post.isDeleted = true;
            var _report = _context.Reports.Where(Reports => Reports.PostId == post.Id).ToList();
            _context.RemoveRange(_report);
            _context.SaveChanges();
        }

    }
}
 