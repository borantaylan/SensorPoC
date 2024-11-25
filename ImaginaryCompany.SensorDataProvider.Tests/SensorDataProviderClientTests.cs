using Microsoft.Extensions.DependencyInjection;

namespace ImaginaryCompany.SensorDataProvider.Tests
{
    public class SensorDataProviderClientTests
    {
        public static TheoryData<DateTime?, DateTime?, int> Fetch_Cases =
            new()
            {
                {DateTime.UtcNow.AddYears(-5), DateTime.UtcNow.AddYears(1), 200 },
                {null, null, 200 },
                {DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(1), 5 },
                {DateTime.UtcNow.AddDays(-5), null, 5 },
                {null, DateTime.UtcNow.AddDays(-1), 199 },
                {null, DateTime.UtcNow.AddDays(-2), 198 },
                {null, DateTime.UtcNow.AddDays(-3), 197 },
            };

        [Theory, MemberData(nameof(Fetch_Cases))]
        public async Task FetchSensorDataAsync(string dateFrom, string dateTo, int resultCount)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddImaginaryCompanySensorDataProvider();
            var sensorDataProviderClient = serviceCollection.BuildServiceProvider().GetRequiredService<ISensorDataProviderClient>();

            var sensorId = Guid.NewGuid(); //Doesnt matter for in memory

            var list = await sensorDataProviderClient.FetchSensorDataAsync(sensorId, dateFrom is null ? null : DateTime.Parse(dateFrom), dateTo is null ? null : DateTime.Parse(dateTo)).ToListAsync();
            Assert.Equal(resultCount, list.Count);
        }

        public static TheoryData<DateTime?, DateTime?> Fetch_ErrorCases =
            new()
            {
                { DateTime.UtcNow.AddYears(-5), DateTime.UtcNow.AddYears(-10) },
            };

        [Theory, MemberData(nameof(Fetch_ErrorCases))]
        public async Task FetchSensorDataAsyncThrowsWhenInvalidInputGiven(string dateFrom, string dateTo)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddImaginaryCompanySensorDataProvider();
            var sensorDataProviderClient = serviceCollection.BuildServiceProvider().GetRequiredService<ISensorDataProviderClient>();

            var sensorId = Guid.NewGuid(); //Doesnt matter for in memory

            await Assert.ThrowsAsync<ArgumentException>(async () => await sensorDataProviderClient.FetchSensorDataAsync(sensorId, DateTime.Parse(dateFrom), DateTime.Parse(dateTo)).ToListAsync());
        }
    }
}