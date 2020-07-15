using System;
using Microsoft.EntityFrameworkCore;
using TestOData.Models;

namespace TestOData.DbContexts
{
    public class ApiContext : DbContext
    {

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
		public DbSet<WeatherReading> WeatherReadings { get; set; }
	}
}