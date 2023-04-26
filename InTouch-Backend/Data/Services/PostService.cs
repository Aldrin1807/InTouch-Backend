﻿using InTouch_Backend.Data.Models;
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

        public void makePost(PostDTO post)
        {
            var _post = new Post()
            {
                Content = post.Content,
                PostDate=DateTime.Now,
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
    }
}
