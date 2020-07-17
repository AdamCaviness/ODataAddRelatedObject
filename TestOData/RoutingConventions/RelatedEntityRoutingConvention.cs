using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Parser = Microsoft.OData.UriParser;

namespace TestOData
{
	public class RelatedEntityRoutingConvention : NavigationSourceRoutingConvention
	{
		public override string SelectController(ODataPath odataPath, HttpRequestMessage request)
		{
			if (odataPath.Segments.Any(s => s is UnresolvedPathSegment))
				return string.Empty;

			if (odataPath.PathTemplate.StartsWith("~/entityset/key/navigation"))
			{
				var segment = odataPath.Segments[odataPath.Segments.Count - 1] as Parser.NavigationPropertySegment;
				if (segment != null)
					return segment.NavigationSource.Name;
			}

			return null;
		}
		public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
		{
			if (odataPath == null || controllerContext == null || actionMap == null)
				return null;

			// Web API OData 5.9.1 started calling parameterless GET() action when querying with composite key. https://github.com/OData/WebApi/issues/884
			if (odataPath.PathTemplate == "~/entityset/key")
			{
				var keyValueSegment = odataPath.Segments[1] as Parser.KeySegment;
				controllerContext.RouteData.Values[ODataRouteConstants.Key] = string.Join(",", keyValueSegment.Keys.Select(p => p.Key + "=" + p.Value));
			}

			if (odataPath.PathTemplate.StartsWith("~/entityset/key/navigation"))
			{
				var actionName = controllerContext.Request.Method.Method;

				if (odataPath.PathTemplate.EndsWith("$ref"))
					actionName += "Ref";
				else
					actionName += "FromRelatedEntity";

				if (actionMap.Contains(actionName))
					return actionName;
			}

			return null;
		}
	}
}