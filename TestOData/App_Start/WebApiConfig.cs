// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using TestOData.DbContexts;
using TestOData.Models;

namespace TestOData
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			DbContextSeed.Initialize();

			var model = GetEdmModel();
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
			config.MapODataServiceRoute("odata", "odata", model, new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
		}

		private static IEdmModel GetEdmModel()
		{
			var odataBuilder = new ODataConventionModelBuilder();

			odataBuilder.Namespace = "TestOData";
			odataBuilder.EntitySet<WeatherForecast>("WeatherForecasts");
			odataBuilder.EntitySet<WeatherReading>("WeatherReadings");

			return odataBuilder.GetEdmModel();
		}
	}
}
