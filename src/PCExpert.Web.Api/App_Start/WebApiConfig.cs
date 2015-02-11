using System.Web.Http;
using PCExpert.DomainFramework.Utils;
using PCExpert.Web.Model.Core;

namespace PCExpert.Web.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			Argument.NotNull(config);
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(WebApiRouteNames.DefaultApi, "api/{controller}/{id}", new { id = RouteParameter.Optional });
		}
	}
}