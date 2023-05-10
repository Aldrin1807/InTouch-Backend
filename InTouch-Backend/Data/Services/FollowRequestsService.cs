using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;

namespace InTouch_Backend.Data.Services
{
    public class FollowRequestsService
    {
        private readonly AppDbContext _context;

        public FollowRequestsService(AppDbContext context)
        {
            _context = context; 
        }

        public void requestFollow(FollowRequestsDTO request)
        {
            var _request = new FollowRequests()
            {
                FollowRequestId = request.FollowRequestId,
                FollowRequestedId = request.FollowRequestedId
            };
            _context.Add(_request);
            _context.SaveChanges();
        }

        public void unRequestFollow(FollowRequestsDTO request)
        {
            var _request = _context.FollowRequests.FirstOrDefault(f => f.FollowRequestId == request.FollowRequestId && f.FollowRequestedId == request.FollowRequestedId);

            if (_request != null)
            {
                _context.Remove(_request);
                _context.SaveChanges();
            }

        }

    }
}
