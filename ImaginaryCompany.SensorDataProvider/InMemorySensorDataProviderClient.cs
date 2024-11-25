using SensorPoC.Domain.Contracts;

namespace ImaginaryCompany.SensorDataProvider
{
    /// <summary>
    /// Internal implementation of how to read sensor data from the data store.
    /// For simplicity instead of a webapi with authorization, just a simple inmemory data used
    /// for simulating "a company collecting sensor data".
    /// </summary>
    internal class InMemorySensorDataProviderClient : ISensorDataProviderClient
    {
        private readonly string clientApiKey;
        private readonly IEnumerable<SensorDataDto> timeSeriesData;
        private readonly Random random = new();

        /// <summary>
        /// Conceptually we will need to authorize M2M using the key
        /// </summary>
        public InMemorySensorDataProviderClient(string clientApiKey)
        {
            this.clientApiKey = clientApiKey;
            var todayDate = DateTime.UtcNow;
            timeSeriesData = Enumerable.Range(1, 200).Select(x => new SensorDataDto { StoredAt = todayDate.AddDays(-x), Timestamp = todayDate.AddDays(-x), Value = random.Next(1, 11), StreamId = Guid.Empty });
        }

        /// <summary>
        /// Fetch the time series values for given time for given sensor specified with the stream identity.
        /// This is an endpoint requires M2M authorization.
        /// </summary>
        public async IAsyncEnumerable<SensorDataDto> FetchSensorDataAsync(Guid streamId, DateTime? from, DateTime? to)
        {
            //For simplicity lets say clientApiKey must be 123456, and it has all the access to sensors.
            if (!string.Equals("123456", clientApiKey))
            {
                throw new NotAuthorizedException();
            }
            else
            {
                from ??= from ?? DateTime.MinValue;
                to ??= to ?? DateTime.MaxValue;
                if (from > to)
                {
                    throw new ArgumentException($"'From' date: {from} can not be later than 'to' date: {to}");
                }
                foreach (var timeSeriesPoint in timeSeriesData.Where(x => x.Timestamp >= from && x.Timestamp <= to))
                {
                    await Task.Yield(); // This gives control back to the caller.
                    yield return timeSeriesPoint;
                }
            }
        }
    }
}
