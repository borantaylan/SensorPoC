using SensorPoC.Domain.Contracts;
using System.Collections.Generic;

namespace ImaginaryCompany.SensorDataProvider
{
    /// <summary>
    /// The generic client providing data related methods.
    /// </summary>
    public interface ISensorDataProviderClient
    {
        /// <summary>
        /// Fetch time series data for given time range.
        /// </summary>
        /// <param name="streamId">Identity of the stream a.k.a sensor.</param>
        /// <param name="from">The beginning time of the queried time series data.</param>
        /// <param name="to">The end time of the queried time series data.</param>
        /// <returns>Time series data of measurements done by the sensor.</returns>
        IAsyncEnumerable<SensorDataDto> FetchSensorDataAsync(Guid streamId, DateTime? from, DateTime? to);
    }
}
