using Microsoft.Extensions.DependencyInjection;

namespace ImaginaryCompany.SensorDataProvider
{
    public static class SensorDataProviderIoCExtensions
    {
        /// <summary>
        /// Adds <see cref="ISensorDataProviderClient"/> implementation for given service collection.
        /// </summary>
        public static void AddImaginaryCompanySensorDataProvider(this IServiceCollection serviceCollection, string apiKey="123456")
        {
            serviceCollection.AddSingleton<ISensorDataProviderClient>(new InMemorySensorDataProviderClient(apiKey));
        }
    }
}
