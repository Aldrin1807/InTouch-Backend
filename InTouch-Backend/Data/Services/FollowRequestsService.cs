namespace InTouch_Backend.Data.Services
{
    public class FollowRequestsService
    {
        private readonly AppDbContext _context;

        public FollowRequestsService(AppDbContext context)
        {
            _context = context; 
        }

    }
}
