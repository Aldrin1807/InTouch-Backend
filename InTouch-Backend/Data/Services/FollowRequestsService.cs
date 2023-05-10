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
        
        public bool isRequested(FollowRequestsDTO request)
        {
            return _context.FollowRequests.Any(f => f.FollowRequestId == request.FollowRequestId && f.FollowRequestedId == request.FollowRequestedId);
        }

        public List<User> getUserRequests (int userId)
        {
            List<int> ids = _context.FollowRequests.Where(r => r.FollowRequestedId == userId)
                .Select(r=> r.FollowRequestId).ToList();
            List<User> request = _context.Users.Where(u => ids.Contains(u.Id)).ToList();
            return request;
        }

        public void handleAccept(int userOne, int userTwo)
        {
          
           
                Follows _follow = new Follows()
                {
                    FollowerId = userTwo,
                    FollowingId = userOne
                };
                _context.Follows.Add(_follow);
            var request = _context.FollowRequests.FirstOrDefault(r => r.FollowRequestId == userOne && r.FollowRequestedId == userTwo);
            if(request != null)
            _context.FollowRequests.Remove(request);
            _context.SaveChanges();
        }
        public void handleDecline(int userOne, int userTwo)
        {
            var request = _context.FollowRequests.FirstOrDefault(r => r.FollowRequestedId == userOne && r.FollowRequestId == userTwo);
            if (request != null)
            {
                _context.FollowRequests.Remove(request);
            }
            _context.SaveChanges();
        }
    }
}
