using Azure.Storage.Blobs;
using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
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

         public async Task makePost(PostDTO post)
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

              
                BlobServiceClient blobServiceClient = new BlobServiceClient("BlobEndpoint=https://intouchimages.blob.core.windows.net/;QueueEndpoint=https://intouchimages.queue.core.windows.net/;FileEndpoint=https://intouchimages.file.core.windows.net/;TableEndpoint=https://intouchimages.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-04T07:41:45Z&st=2023-07-03T22:41:45Z&spr=https&sig=FbwCGBBhHqxzyLsI8%2BZE7zkPFz9%2B0mlKT8a0vD0ucBs%3D");
                
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("post-images");

              
                BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);
                using (var stream = post.Image.OpenReadStream())
                {

                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                _post.ImagePath = uniqueFileName;
            }
             else
             {
                 _post.ImagePath = "";
             }


           await  _context.Posts.AddAsync(_post);
             await _context.SaveChangesAsync();
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

        public async Task checkDeletedPosts (int userId)
        {
            List<int> userPosts =await _context.Posts.Where(p => p.userID == userId && p.isDeleted).Select(p=>p.Id).ToListAsync();
            if (userPosts.Count>0)
            {
               foreach(int id in userPosts)
                {
                    deletePost(id);
                }
                throw new Exception("Please don't use harmful words on your posts, it may cause your account to be locked or deleted.");
            }
        }
        public async Task<List<Post>> getFollowedPosts(int userId)
            {
                var followedUserIds =await _context.Follows
                    .Where(f => f.FollowerId == userId || f.FollowingId == userId)
                    .Select(f => f.FollowerId == userId ? f.FollowingId : f.FollowerId)
                    .ToListAsync();

                var posts =await _context.Posts
                    .Where(p => followedUserIds.Contains(p.userID))
                    .ToListAsync();
                return posts;
            }

        public async Task<User> getUserPostInfo(int postId)
        {

            var PostInfo =await _context.Posts.FirstOrDefaultAsync(u => u.Id == postId);
            var UserInfo =await _context.Users.FirstOrDefaultAsync(u => u.Id == PostInfo.userID);

            return UserInfo;
        }

        public async Task<List<Post>> getUserPosts(int userId)
        {

            var posts =await _context.Posts.Where(p=> p.userID==userId).ToListAsync();
            posts.Reverse();
            return posts;
        }

        public async Task deletePost(int postId)
        {
            var post=await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {

                BlobServiceClient blobServiceClient = new BlobServiceClient("BlobEndpoint=https://intouchimages.blob.core.windows.net/;QueueEndpoint=https://intouchimages.queue.core.windows.net/;FileEndpoint=https://intouchimages.file.core.windows.net/;TableEndpoint=https://intouchimages.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-04T07:41:45Z&st=2023-07-03T22:41:45Z&spr=https&sig=FbwCGBBhHqxzyLsI8%2BZE7zkPFz9%2B0mlKT8a0vD0ucBs%3D");

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("post-images");

               

                if (!string.IsNullOrEmpty(post.ImagePath))
                {
               
                    BlobClient blobClientDel = containerClient.GetBlobClient(post.ImagePath);
                    await blobClientDel.DeleteIfExistsAsync();
                }
                var postComments = _context.Comments.Where(c => c.PostId == postId);
                _context.Comments.RemoveRange(postComments);

                var postLikes = _context.Likes.Where(l => l.PostId == postId);
                _context.Likes.RemoveRange(postLikes);

                var postReports = _context.Reports.Where(r => r.PostId == postId);
                _context.Reports.RemoveRange(postReports);

                var savedPosts = _context.SavedPosts.Where(p => p.PostId == postId);
                _context.SavedPosts.RemoveRange(savedPosts);
                _context.Posts.Remove(post);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Post> getPostInfo(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
                return post;
        }

        public async Task setDeleteTrue(int postId)
        {
            var post =await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if(post == null)
            {
                throw new Exception("There is no post with this id");
            }
            post.isDeleted = true;
            var _report = _context.Reports.Where(Reports => Reports.PostId == post.Id).ToList();
            _context.RemoveRange(_report);
           await _context.SaveChangesAsync();
        }

    }
}
 