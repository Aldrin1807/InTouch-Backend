namespace InTouch_Backend.Data.Services
{
    public class EmailCleanupService:BackgroundService
    {
        private readonly AppDbContext _context;
        private readonly UsersService _service;
        public EmailCleanupService(AppDbContext context, UsersService service)
        {
            _context = context;
            _service = service;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await DeleteUnconfirmedUsers();

                await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
            }
        }


        private async Task DeleteUnconfirmedUsers()
        {
            var usersNotConfirmed = _context.Users.Where(u => u.emailConfirmed == false).ToList();

            foreach(var user in usersNotConfirmed)
            {
              await _service.DeleteUser(user.Id);
            }
           
        }

    }
}
