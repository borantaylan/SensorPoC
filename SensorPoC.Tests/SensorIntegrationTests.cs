using ImaginaryCompany.SensorDataProvider;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SensorPoC.Domain.Contracts;
using SensorPoC.Storage.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace SensorPoC.Tests
{
    /// <summary>
    /// Mocked sensor api tests
    /// </summary>
    public class SensorsApiTests : IClassFixture<WebApplicationFactory<WebAPI.Program>>
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient client;
        private Mock<ISensorDataProviderClient> sensorDataProviderClientMock;
        private static readonly List<SensorDataDto> fakeQueryData =
        [
            new (){ Value = 4,StoredAt = DateTime.UtcNow.AddDays(-4),Timestamp = DateTime.UtcNow.AddDays(-4),StreamId = Guid.Empty},
            new (){ Value = 5,StoredAt = DateTime.UtcNow.AddDays(-3),Timestamp = DateTime.UtcNow.AddDays(-3),StreamId = Guid.Empty},
            new (){ Value = 6,StoredAt = DateTime.UtcNow.AddDays(-2),Timestamp = DateTime.UtcNow.AddDays(-2),StreamId = Guid.Empty},
            new (){ Value = 7,StoredAt = DateTime.UtcNow.AddDays(-1),Timestamp = DateTime.UtcNow.AddDays(-1),StreamId = Guid.Empty},
        ];

        public SensorsApiTests(WebApplicationFactory<WebAPI.Program> factory)
        {
            scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();

            client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the repository with a mock
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(ISensorDataProviderClient));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    sensorDataProviderClientMock = new Mock<ISensorDataProviderClient>();
                    sensorDataProviderClientMock.Setup(client => client.FetchSensorDataAsync(It.IsAny<Guid>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                        .Returns(fakeQueryData.ToAsyncEnumerable());

                    services.AddSingleton(sensorDataProviderClientMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            var newSensor = new Sensor
            (
                Guid.NewGuid(),
                "Temperature Sensor",
                "Factory Machine Room",
                10,
                5,
                DateTime.UtcNow
            );

            // Act
            var response = await client.PostAsJsonAsync("/api/sensors", newSensor);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseSensor = await response.Content.ReadFromJsonAsync<Sensor>();
            Assert.NotNull(responseSensor);
            Assert.Equal(newSensor.Name, responseSensor.Name);

            var storedSensor = await GetRepository().GetAsync(newSensor.Identity);
            Assert.NotNull(storedSensor);
            Assert.Equal(newSensor.Name, storedSensor.Name);
        }

        public static TheoryData<Guid, string, string, double, double, DateTimeOffset> Create_ErrorCases =
            new()
            {
                {Guid.NewGuid(),"x", "x", 0, 5, DateTime.Now },
                {Guid.NewGuid(),"x", "x", 0, 0, DateTime.Now },
                {Guid.NewGuid(),"x", "", 5, 0, DateTime.Now },
                {Guid.NewGuid(),"", "x", 5, 0, DateTime.Now },
                {default,"x", "x", 5, 0, DateTime.Now },
                {Guid.NewGuid(),"x", "x", 5, 0, default }
            };

        [Theory, MemberData(nameof(Create_ErrorCases))]
        public async Task CreateThrowsWhenInvalidInputGiven(Guid identity, string name, string location, double upperLimit, double lowerLimit, DateTimeOffset dateTime)
        {
            // Arrange
            var newSensor = new Sensor
            (
                identity,
                name,
                location,
                upperLimit,
                lowerLimit,
                dateTime
            );

            // Act
            var response = await client.PostAsJsonAsync("/api/sensors", newSensor);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var createdSensorIdentity = await CreateSensorAsync();

            var updatedSensor = new Sensor
            (
                createdSensorIdentity,
                "Temperature Sensor",
                "Factory Machine Room",
                10,
                5,
                DateTime.UtcNow
            );

            // Act
            var response = await client.PutAsJsonAsync("/api/sensors", updatedSensor);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var storedSensor = await GetRepository().GetAsync(createdSensorIdentity);
            Assert.NotNull(storedSensor);
            Assert.Equal(updatedSensor.Name, storedSensor.Name);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var createdSensorIdentity = await CreateSensorAsync();

            // Act
            var response = await client.DeleteAsync($"/api/sensors/{createdSensorIdentity}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await GetRepository().GetAsync(createdSensorIdentity));
        }

        [Fact]
        public async Task Get()
        {
            // Arrange
            var seededSensorIdentity = (await GetRepository().GetAllAsync()).Single().Identity;

            // Act
            var response = await client.GetAsync($"/api/sensors/{seededSensorIdentity}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sensor = await response.Content.ReadFromJsonAsync<Sensor>();
            Assert.NotNull(sensor);
        }

        [Fact]
        public async Task GetAll()
        {
            // Act
            var response = await client.GetAsync("/api/sensors");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sensors = await response.Content.ReadFromJsonAsync<List<Sensor>>(); //seeded data
            Assert.NotNull(sensors);
        }

        [Fact]
        public async Task Query()
        {
            // Arrange
            var sensorIdentity = Guid.NewGuid(); 
            var dateTimeFrom = "11/11/2021";
            var dateTimeTo = "11/11/2025";

            // Act
            var response = await client.GetAsync($"/api/sensors/{sensorIdentity}/query?from={dateTimeFrom}&to={dateTimeTo}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sensors = await response.Content.ReadFromJsonAsync<List<SensorDataDto>>();
            Assert.Equal(sensors!.Count, fakeQueryData.Count);
            sensorDataProviderClientMock.Verify(x => x.FetchSensorDataAsync(
                It.Is<Guid>(x => x == sensorIdentity),
                It.Is<DateTime?>(x => x == DateTime.Parse(dateTimeFrom)),
                It.Is<DateTime?>(x => x == DateTime.Parse(dateTimeTo))));
        }

        private ISensorRepository GetRepository()
        {
            return scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISensorRepository>();
        }

        private async Task<Guid> CreateSensorAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var sensor = new Sensor(
                identity: Guid.NewGuid(),
                name: "NewName",
                location: "NewLocation",
                upperLimit: 10,
                lowerLimit: 5,
                creationTime: DateTimeOffset.Now
            );
            unitOfWork.SensorRepository.Create(sensor);
            await unitOfWork.SaveChangesAsync();
            return sensor.Identity;
        }
    }
}