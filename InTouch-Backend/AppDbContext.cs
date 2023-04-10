using InTouch_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        { 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPostLike>()
                .HasKey(up => new { up.UserId, up.PostId });
            modelBuilder.Entity<UserPostLike>()
                .HasOne(u => u.User)
                .WithMany(p => p.Likes)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserPostLike>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(u => u.PostId);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get;set; }  
        public DbSet<UserPostLike> Likes { get; set; }

        
    }
}
