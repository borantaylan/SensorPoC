namespace SensorPoC.Domain.Contracts
{
    /// <summary>
    /// Representation of sensor data.
    /// </summary>
    public class SensorDataDto()
    {
        /// <summary>
        /// The identity of the sensor providing the data.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// The timestamp that the telemetry is done. //TODO verify this.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The timestamp that the telemetry is stored. //TODO verify this.
        /// </summary>
        public DateTime StoredAt { get; set; }

        /// <summary>
        /// The value of the measurement.
        /// </summary>
        public double Value { get; set; }
    }
}
