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
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Likes>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Comments>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            

            //MANY to MANY per tabelen e Reports
          modelBuilder.Entity<Reports>()
                .HasKey(up => new { up.UserId, up.PostId });
            modelBuilder.Entity<Reports>()
                .HasOne(u => u.User)
                .WithMany(p => p.Reports)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Reports>()
                .HasOne(u => u.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull); 


            //MANY to MANY per tabelen e Followave
            modelBuilder.Entity<Follows>()
                .HasKey(f => new { f.FollowerId, f.FollowingId });

            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Follows>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Many to many per tabelen e follow requestav

            modelBuilder.Entity<FollowRequests>()
               .HasKey(f => new { f.FollowRequestId, f.FollowRequestedId});

            modelBuilder.Entity<FollowRequests>()
                .HasOne(f => f.FollowRequest)
                .WithMany(u => u.FollowRequested)
                .HasForeignKey(f => f.FollowRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<FollowRequests>()
                .HasOne(f => f.FollowRequested)
                .WithMany(u => u.FollowRequest)
                .HasForeignKey(f => f.FollowRequestedId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //MANY to MANY per tabelen e SavedPosts
            modelBuilder.Entity<SavedPost>()
                .HasKey(up => new { up.UserId, up.PostId });
            modelBuilder.Entity<SavedPost>()
                .HasOne(u => u.User)
                .WithMany(p => p.SavedPosts)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<SavedPost>()
                .HasOne(u => u.Post)
                .WithMany(p => p.SavedPosts)
                .HasForeignKey(u => u.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get;set; }  
        public DbSet<Likes> Likes { get; set; }

        public DbSet<SavedPost> SavedPosts { get; set; }

        public DbSet<Reports> Reports { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Follows> Follows { get; set; }

        public DbSet<FollowRequests> FollowRequests { get; set; }

    }
}
