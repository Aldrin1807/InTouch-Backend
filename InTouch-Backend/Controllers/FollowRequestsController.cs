using InTouch_Backend.Data.Services;

namespace InTouch_Backend.Controllers
{
    public class FollowRequestsController
    {
        private readonly FollowRequestsService _service;

        public FollowRequestsController(FollowRequestsService service)
        {
            _service = service;
        }

    }
}
