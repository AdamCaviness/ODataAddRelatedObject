﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestOData.Models;

namespace TestOData.DbContexts
{
    public class DbContextSeed
    {
        public static void Initialize()
        {
            var randomTime = new Random();
			var randomTemp = new Random();
            var contextOptions = new DbContextOptions<ApiContext>();
            using (var context = new ApiContext(contextOptions))
            {
				for (int i = 1; i < 6; i++)
                {
                    var forecast = new WeatherForecast()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddSeconds(randomTime.Next(1, 5) * -1),
                        Summary = "Record " + i.ToString()
                    };
                    var reading = new WeatherReading()
                    {
                        Id = Guid.NewGuid(),
                        Temperature = randomTemp.Next(-20, 55),
                        TemperatureSystem = "C"
                    };

                    forecast.Readings.Add(reading);

                    context.WeatherForecasts.Add(forecast);
                    context.WeatherReadings.Add(reading);
                }

                context.SaveChanges();
            }
        }
    }
}