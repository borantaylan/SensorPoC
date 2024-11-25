namespace SensorPoC.Domain.Contracts
{
    public class Sensor
    {
        public Sensor(
            Guid identity,
            string name,
            string location,
            double upperLimit,
            double lowerLimit,
            DateTimeOffset creationTime)
        {
            Identity = identity;
            Name = name;
            Location = location;
            UpperLimit = upperLimit;
            LowerLimit = lowerLimit;
            CreationTime = creationTime;
        }

        public Guid Identity { get; private set;}
        public string Name { get; private set;}
        public string Location { get; private set;}
        public double UpperLimit { get; private set;}
        public double LowerLimit { get; private set;}
        public DateTimeOffset CreationTime { get; private set; }
    }
}
