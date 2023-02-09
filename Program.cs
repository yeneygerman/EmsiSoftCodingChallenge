using Data;
using EmsiSoft.RabbitMQ;
using EmsiSoft.RabbitMQ.Interface;
using EmsiSoft.Services;
using EmsiSoft.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmsiSoft
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<EmsiSoftContextDB>(options => options.UseSqlServer(Configuration.GetConnectionString("Local")));
            builder.Services.AddScoped<IHashesService, HashesService>();
            builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

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