
using ImaginaryCompany.SensorDataProvider;
using Microsoft.EntityFrameworkCore;
using SensorPoC.Domain.Contracts;
using SensorPoC.Storage;

namespace SensorPoC.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(corsOptions => corsOptions.AddDefaultPolicy(
                builder => builder
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()));
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDomain();
            builder.Services.AddStorage();
            builder.Services.AddImaginaryCompanySensorDataProvider();

            builder.Services.AddLogging(builder => builder.AddConsole());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.MapControllers();

            SeedDb(app.Services);
            app.Run();
        }

        private static void SeedDb(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetService<ISensorDbContext>();

            context!.Database.EnsureDeleted();
            context!.Database.EnsureCreated(); //Simple migration

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.SensorRepository.Create(new Sensor(Guid.NewGuid(), "MySensor", "LivingRoom", 9, 3, DateTimeOffset.UtcNow));
            unitOfWork.SaveChanges();
        }
    }
}
