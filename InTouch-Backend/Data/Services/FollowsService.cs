using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class FollowsService
    {
        private readonly AppDbContext _context;

            public FollowsService(AppDbContext context)
        {
            _context = context;
                
        }

        public async Task  followUser(FollowsDTO follow)
        {
            var _follow = new Follows()
            {
                FollowerId = follow.FollowerId,
                FollowingId = follow.FollowingId
            };
           await _context.AddAsync(_follow);
            await _context.SaveChangesAsync();
        }

        public async Task unFollowUser(FollowsDTO follow)
        {
            var _follow =await _context.Follows.FirstOrDefaultAsync(f=> f.FollowerId==follow.FollowerId && f.FollowingId==follow.FollowingId);

            if(_follow!= null)
            {
                _context.Remove(_follow);
               await _context.SaveChangesAsync();
            }
            
        }

    }
}
