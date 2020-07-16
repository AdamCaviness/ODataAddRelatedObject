// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
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

			var routeName = "odata";
			var routePrefix = "odata";
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
			config.MapODataServiceRoute(
				routeName: routeName, 
				routePrefix: routePrefix, 
				model: GetEdmModel(), 
				pathHandler: new DefaultODataPathHandler(),
				routingConventions: GetRoutingConventions(routeName, config),
				batchHandler: new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
		}

		private static IEdmModel GetEdmModel()
		{
			var odataBuilder = new ODataConventionModelBuilder();

			odataBuilder.Namespace = "TestOData";
			odataBuilder.EntitySet<WeatherForecast>("WeatherForecasts");
			odataBuilder.EntitySet<WeatherReading>("WeatherReadings");

			return odataBuilder.GetEdmModel();
		}
		private static List<IODataRoutingConvention> GetRoutingConventions(string routeName, HttpConfiguration configuration)
		{
			var routingConventions = new List<IODataRoutingConvention>();
			routingConventions.Add(new RelatedEntityRoutingConvention());
			routingConventions.AddRange(ODataRoutingConventions.CreateDefaultWithAttributeRouting(routeName, configuration));
			return routingConventions;
		}
	}
}
