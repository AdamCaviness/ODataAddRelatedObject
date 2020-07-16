using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TestOData.DbContexts;

namespace TestOData
{
	public static class HttpRequestMessageExtensions
	{
		private static readonly string _requestUriKey = "RequestUri";
		private static readonly string _batchContextKey = "BatchContext";
		private static readonly string _pathSegmentsKey = "PathSegments";
		private static readonly string _isInChangeSetKey = "IsInChangeSet";
		private static readonly string _queryApplied = "QueryApplied";

		/// <summary>
		/// Associates a given instance of a DbContext used in a batch changeset to the current request.
		/// </summary>
		/// <param name="request">The request to will the DbContext instance will be associated.</param>
		/// <param name="context">The DbContext instance to associate with the request.</param>
		public static void SetBatchContext(this HttpRequestMessage request, ApiContext context)
		{
			request.Properties[_batchContextKey] = context;
		}
		/// <summary>
		/// Gets the DbContext used by a batch changeset operation in this request.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static ApiContext GetBatchContext(this HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey(_batchContextKey))
				return request.Properties[_batchContextKey] as ApiContext;

			return null;
		}

		/// <summary>
		/// Associates a list of PathSegments to the current request.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="pathSegments"></param>
		public static void SetPathSegments(this HttpRequestMessage request, List<PathSegment> pathSegments)
		{
			request.Properties[_pathSegmentsKey] = pathSegments;
		}
		/// <summary>
		/// Gets the PathSegments of the current request.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static List<PathSegment> GetPathSegments(this HttpRequestMessage request)
		{
			return request.Properties[_pathSegmentsKey] as List<PathSegment>;
		}

		/// <summary>
		/// Returns true if the request was part of a ChangeSet, otherwise returns false.
		/// The name of this method is based on an existing OData method named IsBatchRequest.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static bool IsChangeSetRequest(this HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey(_isInChangeSetKey))
				return (bool)request.Properties[_isInChangeSetKey];

			request.Properties[_isInChangeSetKey] = false;
			return false;
		}
		/// <summary>
		/// Sets if the request is part of a ChangeSet. The default value is false. This should
		/// be set at a time when it is known if the inner request is part of ChangeSet. At present
		/// this is only known by EntityFrameworkBatchHandler.ParseBatchRequestsAsync().
		/// </summary>
		/// <param name="request"></param>
		/// <param name="isInChangeSet"></param>
		public static void SetIsChangeSetRequest(this HttpRequestMessage request, bool isInChangeSet)
		{
			request.Properties[_isInChangeSetKey] = isInChangeSet;
		}

		/// <summary>
		/// Sets the original Uri of the request so it can be checked later for auditing even if the Uri
		/// has been altered during processing of the request.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="requestUri"></param>
		public static void SetRequestUri(this HttpRequestMessage request, Uri requestUri)
		{
			if (request.Properties.ContainsKey(_requestUriKey))
				return;

			request.Properties[_requestUriKey] = requestUri;
		}
		/// <summary>
		/// Gets the original Uri of the request so it can be checked later for auditing even if the Uri
		/// has been altered during processing of the request.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static Uri GetRequestUri(this HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey(_requestUriKey))
				return request.Properties[_requestUriKey] as Uri;

			return null;
		}

		public static void SetQueryApplied(this HttpRequestMessage request, bool queryApplied)
		{
			request.Properties[_queryApplied] = queryApplied;
		}
		public static bool GetQueryApplied(this HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey(_queryApplied))
				return (bool)request.Properties[_queryApplied];

			return false;
		}
	}
}
