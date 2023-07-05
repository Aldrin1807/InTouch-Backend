using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InTouch_Backend.Data.Services
{
    public class EmailCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var service = scope.ServiceProvider.GetRequiredService<UsersService>();

                    await DeleteUnconfirmedUsers(dbContext, service);
                }

                await Task.Delay(TimeSpan.FromMinutes(21), stoppingToken);
            }
        }

        private async Task DeleteUnconfirmedUsers(AppDbContext dbContext, UsersService service)
        {
            var expirationTime = DateTime.UtcNow;

            var usersNotConfirmed = await dbContext.Users
                .Where(u => !u.emailConfirmed)
                .ToListAsync();

            foreach (var user in usersNotConfirmed)
            {
                await service.DeleteUser(user.Id);
            }

        }
      }
            
}
