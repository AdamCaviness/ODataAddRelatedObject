using System;
using Microsoft.EntityFrameworkCore;
using TestOData.Models;

namespace TestOData.DbContexts
{
    public class ApiContext : DbContext
    {
		public ApiContext()
			: this(new DbContextOptions<ApiContext>())
		{
		}
		public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseInMemoryDatabase(databaseName: "ODataTestDb");

			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<WeatherForecast> WeatherForecasts { get; set; }
		public DbSet<WeatherReading> WeatherReadings { get; set; }
	}
}