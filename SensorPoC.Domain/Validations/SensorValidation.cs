using FluentValidation;
using SensorPoC.Domain.Contracts;

namespace SensorPoC.Domain.Validations
{
    /// <summary>
    /// Validation logic for sensors.
    /// </summary>
    internal class SensorValidator : AbstractValidator<Sensor>
    {
        public SensorValidator()
        {
            RuleFor(x => x.Identity).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.CreationTime).NotEmpty();
            RuleFor(x => x.UpperLimit).GreaterThan(x=>x.LowerLimit).WithMessage("Upper limit must be greater than lower limit.");
        }
    }
}
