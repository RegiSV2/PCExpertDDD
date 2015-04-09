using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PCExpert.Web.Api.Filters;

namespace PCExpert.Web.Api
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			ConfigureMvc();

			ConfigureWebApi();
		}

		private static void ConfigureMvc()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private static void ConfigureWebApi()
		{
			GlobalConfiguration.Configure(
				config =>
				{
					IoCConfig.InitIoC(config);
					WebApiConfig.Register(config);
				});
#if !DEBUG
			GlobalConfiguration.Configuration.Filters.Add(new PCExpertExceptionFilter());
#endif
		}
	}
}