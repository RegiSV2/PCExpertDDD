using Microsoft.Owin;
using Owin;
using PCExpert.WebApp;

[assembly: OwinStartup(typeof (Startup))]

namespace PCExpert.WebApp
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}