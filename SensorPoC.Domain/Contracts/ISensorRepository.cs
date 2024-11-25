namespace SensorPoC.Domain.Contracts
{
    public interface ISensorRepository
    {
        /// <summary>
        /// Creates a sensor in the repo.
        /// </summary>
        void Create(Sensor sensor);

        /// <summary>
        /// Creates a sensor in the repo.
        /// </summary>
        Task Delete(Guid identity);

        /// <summary>
        /// Creates a sensor in the repo.
        /// </summary>
        Task<Sensor> GetAsync(Guid identity);

        /// <summary>
        /// Creates a sensor in the repo.
        /// </summary>
        Task<IReadOnlyCollection<Sensor>> GetAllAsync();

        /// <summary>
        /// Creates a sensor in the repo.
        /// </summary>
        void Update(Sensor sensor);
    }
}
