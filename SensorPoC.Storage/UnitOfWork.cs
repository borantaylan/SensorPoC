using Microsoft.Extensions.DependencyInjection;
using SensorPoC.Domain.Contracts;

namespace SensorPoC.Storage
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly SensorDbContext context;
        private readonly ISensorRepository sensorRepository;

        public UnitOfWork(SensorDbContext context, ISensorRepository sensorRepository)
        {
            this.context = context;
            this.sensorRepository = sensorRepository;
        }

        
        public ISensorRepository SensorRepository => sensorRepository;

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
