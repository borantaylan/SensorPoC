using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SensorPoC.Domain.Validations;

namespace SensorPoC.Domain.Contracts
{
    /// <summary>
    /// IoC extensions for domain services.
    /// </summary>
    public static class IoCExtensions
    {
        /// <summary>
        /// Adds domain related services to <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">DI service collection</param>
        public static void AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IValidator<Sensor>, SensorValidator>();
        }
    }
}
