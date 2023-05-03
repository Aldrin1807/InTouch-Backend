using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

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


    }
}
 