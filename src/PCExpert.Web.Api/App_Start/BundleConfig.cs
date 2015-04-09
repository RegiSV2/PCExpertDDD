using System.Web;
using System.Web.Optimization;

namespace PCExpert.Web.Api
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/bootstrap.js"));
			bundles.Add(new ScriptBundle("~/bundles/app").Include(
				"~/Scripts/app/app.js",
				"~/Scripts/app/componentInterfaceModule.js"));
			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
				"~/Scripts/angular.js",
				"~/Scripts/angular-route.js"));
			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css"));
		}
	}
}
