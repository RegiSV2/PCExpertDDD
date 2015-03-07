using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Routing;
using PCExpert.Web.Api;
using System.Web.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.RequestContainer;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;
using PCExpert.Core.DataAccess;

namespace PCExpert.Web.App
{
    public class Startup
    {
		public IConfiguration Configuration { get; set; }

		public Startup(IHostingEnvironment env)
	    {
			Configuration = new Configuration()
				.Add(new IniFileConfigurationSource("config.json"))
				.AddEnvironmentVariables();
		}
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			
		}

        public void Configure(IApplicationBuilder app)
        {
			app.UseServices(services =>
			{
				services.AddMvc();
				services.AddLogging();
				services.AddEntityFramework(Configuration)
				.AddDbContext<PCExpertContext>();

				return IoCConfig.InitIoC(services);
			});
			
			//app.UseMvc(routes =>
			//{
			//	routes.MapRoute(WebApiRouteNames.DefaultApi, "api/{controller}/{id}", new { id = RouteParameter.Optional });
   //         });
	        app.UseWelcomePage();
        }
    }
}
