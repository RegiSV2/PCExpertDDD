using System.Web;
using System.Web.Http;
using PCExpert.Web.Api.Common;
using PCExpert.Web.Api.Filters;

namespace PCExpert.Web.Api
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(
				config =>
				{
					IoCConfig.InitIoC(config);
					WebApiConfig.Register(config);
				});
			GlobalConfiguration.Configuration.Filters.Add(new PCExpertExceptionFilter());
		}
	}
}