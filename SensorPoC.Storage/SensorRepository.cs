using Microsoft.EntityFrameworkCore;
using SensorPoC.Domain.Contracts;
using SensorPoC.Storage.Exceptions;

namespace SensorPoC.Storage
{
    internal class SensorRepository : ISensorRepository
    {
        private readonly SensorDbContext sensorDbContext;

        public SensorRepository(SensorDbContext sensorDbContext)
        {
            this.sensorDbContext = sensorDbContext;
        }

        /// <inheritdoc/>
        public void Create(Sensor sensor)
        {
            sensorDbContext.Sensors.Add(sensor);
        }

        /// <inheritdoc/>
        public async Task Delete(Guid identity)
        {
            var sensor = await sensorDbContext.Sensors.SingleOrDefaultAsync(x => x.Identity == identity);
            if (sensor == null)
            {
                throw new EntityNotFoundException();
            }
            sensorDbContext.Sensors.Remove(sensor);
        }

        /// <inheritdoc/>
        public async Task<Sensor> GetAsync(Guid identity)
        {
            var sensor = await sensorDbContext.Sensors.SingleOrDefaultAsync(x => x.Identity == identity);
            if (sensor == null)
            {
                throw new EntityNotFoundException();
            }
            return sensor;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Sensor>> GetAllAsync()
        {
            return await sensorDbContext.Sensors.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public void Update(Sensor sensor)
        {
            sensorDbContext.Entry(sensor).State = EntityState.Modified;
        }
    }
}
