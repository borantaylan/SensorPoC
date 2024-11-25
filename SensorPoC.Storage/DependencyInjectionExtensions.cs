using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SensorPoC.Domain.Contracts;

namespace SensorPoC.Storage
{
    public static class DependencyInjectionExtensions
    {
        public static void AddStorage(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISensorRepository, SensorRepository>();
            services.AddDbContext<ISensorDbContext,SensorDbContext>(options =>
                options.UseInMemoryDatabase("SensorDb"),
                ServiceLifetime.Scoped);
        }
    }
}
