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
    public class WeatherForecastsController : ODataController
    {
        private ApiContext _dbContext;

        public WeatherForecastsController(ApiContext context)
        {
            _dbContext = context;
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_dbContext.WeatherForecasts);
        }

        [EnableQuery]
        public SingleResult<WeatherForecast> Get([FromODataUri] Guid key)
        {
            IQueryable<WeatherForecast> result = _dbContext.WeatherForecasts.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(WeatherForecast weatherForecast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.WeatherForecasts.Add(weatherForecast);

            await _dbContext.SaveChangesAsync();

            return Created(weatherForecast);
        }
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<WeatherForecast> weatherForecast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _dbContext.WeatherForecasts.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            weatherForecast.Patch(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForecastExists(key))
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
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, WeatherForecast update)
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
                if (!WeatherForecastExists(key))
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
            var weatherForecast = await _dbContext.WeatherForecasts.FindAsync(key);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            _dbContext.WeatherForecasts.Remove(weatherForecast);

            await _dbContext.SaveChangesAsync();

            return base.StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        private bool WeatherForecastExists(Guid key)
        {
            return _dbContext.WeatherForecasts.Any(p => p.Id == key);
        }
    }
}
