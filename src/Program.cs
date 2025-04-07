using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PgPayContext>(opt =>
            {
                string connect = System.Environment.GetEnvironmentVariable("PMB_BACKEND_CONNECT");
                opt.UseNpgsql(connect);
                opt.UseLazyLoadingProxies();
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}