using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using TestOData.DbContexts;
using TestOData.Models;

namespace TestOData.Controllers
{
    public class WeatherReadingsController : ODataController
    {
        private ApiContext _dbContext;

        public WeatherReadingsController(ApiContext context)
        {
            _dbContext = context;
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_dbContext.WeatherReadings);
        }

        [EnableQuery]
        public SingleResult<WeatherReading> Get([FromODataUri] Guid key)
        {
            IQueryable<WeatherReading> result = _dbContext.WeatherReadings.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(WeatherReading WeatherReading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.WeatherReadings.Add(WeatherReading);

            await _dbContext.SaveChangesAsync();

            return Created(WeatherReading);
        }
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<WeatherReading> weatherReading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _dbContext.WeatherReadings.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            weatherReading.Patch(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherReadingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, WeatherReading update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(update).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherReadingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(update);
        }
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            var WeatherReading = await _dbContext.WeatherReadings.FindAsync(key);
            if (WeatherReading == null)
            {
                return NotFound();
            }

            _dbContext.WeatherReadings.Remove(WeatherReading);

            await _dbContext.SaveChangesAsync();

            return base.StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        private bool WeatherReadingExists(Guid key)
        {
            return _dbContext.WeatherReadings.Any(p => p.Id == key);
        }
    }
}
