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
            

            var confirmations =await dbContext.Confirmations.ToListAsync();

            var expired = confirmations.Where(c=> DateTime.Parse(c.ExpirationDate) < expirationTime);


            foreach (var user in expired)
            {
                await service.DeleteUser(user.userId);
            }

        }
      }
            
}
