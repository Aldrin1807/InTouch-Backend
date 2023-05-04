using InTouch_Backend.Data.DTOs;
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
            //Passwordi dhe username unique
                modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            //MANY to MANY per tabelen e Likes
            modelBuilder.Entity<Likes>()
                .HasKey(up => new { up.UserId, up.PostId });
            modelBuilder.Entity<Likes>()
                .HasOne(u => u.User)
                .WithMany(p => p.Likes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Likes>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //MANY to MANY per tabelen e Commenteve

            modelBuilder.Entity<Comments>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Comments>()
              .HasKey(up => new { up.Id, up.UserId, up.PostId });
            modelBuilder.Entity<Comments>()
                .HasOne(u => u.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comments>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            

            //MANY to MANY per tabelen e Reports
          modelBuilder.Entity<Reports>()
                .HasKey(up => new { up.UserId, up.PostId });
            modelBuilder.Entity<Reports>()
                .HasOne(u => u.User)
                .WithMany(p => p.Reports)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Reports>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.NoAction); 


            //MANY to MANY per tabelen e Followave
            modelBuilder.Entity<Follows>()
                .HasKey(f => new { f.FollowerId, f.FollowingId });

            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Cascade); 

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get;set; }  
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Follows> Follows { get; set; }
        
    }
}
