
using InTouch_Backend.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Lidhja me Databaze
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnectionString")));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //
            builder.Services.AddTransient<UsersService>();
            builder.Services.AddTransient<PostService>();
            builder.Services.AddTransient<ReportsService>();
            builder.Services.AddTransient<LikesService>();
            builder.Services.AddTransient<FollowsService>();
            builder.Services.AddTransient<CommentsService>();


            builder.Services.AddCors();

            var app = builder.Build();
           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Konfigurimi per lejimin e thirrjeve te api endpoint nga hosta te ndryshem
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}