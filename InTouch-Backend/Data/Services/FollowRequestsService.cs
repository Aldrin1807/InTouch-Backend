using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class FollowRequestsService
    {
        private readonly AppDbContext _context;

        public FollowRequestsService(AppDbContext context)
        {
            _context = context; 
        }

        public async Task requestFollow(FollowRequestsDTO request)
        {
            var _request = new FollowRequests()
            {
                FollowRequestId = request.FollowRequestId,
                FollowRequestedId = request.FollowRequestedId
            };
           await _context.AddAsync(_request);
           await _context.SaveChangesAsync();
        }

        public async Task unRequestFollow(FollowRequestsDTO request)
        {
            var _request = _context.FollowRequests.FirstOrDefaultAsync(f => f.FollowRequestId == request.FollowRequestId && f.FollowRequestedId == request.FollowRequestedId);

            if (_request != null)
            {
                _context.Remove(_request);
              await  _context.SaveChangesAsync();
            }

        }
        
        public async Task<bool> isRequested(FollowRequestsDTO request)
        {
            return await _context.FollowRequests.AnyAsync(f => f.FollowRequestId == request.FollowRequestId && f.FollowRequestedId == request.FollowRequestedId);
        }

        
        public async Task<List<User>> getUserRequests(int userId)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null && !user.isPrivate)
            {
                List<FollowRequestsDTO> requests =await _context.FollowRequests
                    .Where(f => f.FollowRequestedId == userId)
                    .Select(f => new FollowRequestsDTO
                    {
                        FollowRequestId = f.FollowRequestId,
                        FollowRequestedId = f.FollowRequestedId
                    })
                    .ToListAsync();

                foreach (var f in requests)
                {
                  await handleAccept(f);
                }
            }

            List<int> ids =await _context.FollowRequests
                .Where(r => r.FollowRequestedId == userId)
                .Select(r => r.FollowRequestId)
                .ToListAsync();

            List<User> request =await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

            request.Reverse();
            return request;
        }

        public async Task handleAccept(FollowRequestsDTO request)
        {
            var _request =await _context.FollowRequests.FirstOrDefaultAsync(r => r.FollowRequestId == request.FollowRequestId && r.FollowRequestedId == request.FollowRequestedId);
            if (_request != null)
            {
                _context.FollowRequests.Remove(_request);
               await _context.SaveChangesAsync();
            }
            
            Follows _follow = new Follows()
                {
                    FollowerId = _request.FollowRequestId,
                    FollowingId = _request.FollowRequestedId
                };
              await  _context.Follows.AddAsync(_follow);
          
            await _context.SaveChangesAsync();
        }
        public async Task handleDecline(FollowRequestsDTO request)
        {
            var _request =await _context.FollowRequests.FirstOrDefaultAsync(r => r.FollowRequestedId == request.FollowRequestedId && r.FollowRequestId == request.FollowRequestId);
            if (_request == null)
            {
                throw new Exception("There is no request like this.");
            }
            _context.FollowRequests.Remove(_request);
            _context.SaveChangesAsync();
        }
    }
}
