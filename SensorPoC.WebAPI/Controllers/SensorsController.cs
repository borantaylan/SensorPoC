using FluentValidation;
using ImaginaryCompany.SensorDataProvider;
using Microsoft.AspNetCore.Mvc;
using SensorPoC.Domain.Contracts;
using SensorPoC.Storage.Exceptions;

namespace SensorPoC.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly IValidator<Sensor> validator;
        private readonly IUnitOfWork unitOfWork;

        public SensorsController(IValidator<Sensor> validator, IUnitOfWork unitOfWork)
        {
            this.validator = validator;
            this.unitOfWork = unitOfWork;
        }

        // GET: api/sensors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sensors = await unitOfWork.SensorRepository.GetAllAsync();
            return Ok(sensors);
        }

        // GET: api/sensors/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sensor = await unitOfWork.SensorRepository.GetAsync(id);
            if (sensor == null) return NotFound();
            return Ok(sensor);
        }

        // POST: api/sensors
        [HttpPost]
        public async Task<IActionResult> Create(Sensor sensor)
        {
            await validator.ValidateAndThrowAsync(sensor);
            unitOfWork.SensorRepository.Create(sensor);
            await unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = sensor.Identity }, sensor);
        }

        // PUT: api/sensors
        [HttpPut()]
        public async Task<IActionResult> Update(Sensor sensor) //Discussable to put id in the route...
        {
            try
            {
                await validator.ValidateAndThrowAsync(sensor);
                unitOfWork.SensorRepository.Update(sensor);
                await unitOfWork.SaveChangesAsync();
            }
            catch (EntityNotFoundException) //TODO Better to do it in an exception middleware.
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/sensors/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await unitOfWork.SensorRepository.Delete(id);
                await unitOfWork.SaveChangesAsync();
            }
            catch (EntityNotFoundException) //TODO Better to do it in an exception middleware.
            {
                return NotFound();
            }
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/sensor/{id}/query?from=dd/mm/yyyy&to=dd/mm/yyyy
        [HttpGet("{id:guid}/query")]
        public async Task<IActionResult> Query(
            [FromRoute] Guid id,
            [FromQuery] DateTimeOffset? from,
            [FromQuery] DateTimeOffset? to,
            [FromServices] ISensorDataProviderClient sensorDataProviderClient)
        {
            var list = await sensorDataProviderClient.FetchSensorDataAsync(id, from?.Date, to?.Date).ToListAsync();

            return Ok(list);
        }
    }
}