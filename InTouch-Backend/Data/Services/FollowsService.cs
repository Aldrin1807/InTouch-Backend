using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;

namespace InTouch_Backend.Data.Services
{
    public class FollowsService
    {
        private readonly AppDbContext _context;

            public FollowsService(AppDbContext context)
        {
            _context = context;
                
        }

        public void  followUser(FollowsDTO follow)
        {
            var _follow = new Follows()
            {
                FollowerId = follow.FollowerId,
                FollowingId = follow.FollowingId
            };
            _context.Add(_follow);
            _context.SaveChanges();
        }

        public void unFollowUser(FollowsDTO follow)
        {
            var _follow = _context.Follows.FirstOrDefault(f=> f.FollowerId==follow.FollowerId && f.FollowingId==follow.FollowingId);

            if(_follow!= null)
            {
                _context.Remove(_follow);
                _context.SaveChanges();
            }
            
        }

    }
}
