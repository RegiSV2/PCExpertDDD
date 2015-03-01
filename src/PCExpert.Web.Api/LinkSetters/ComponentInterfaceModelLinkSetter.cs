using System.Net;
using System.Web.Http.Routing;
using PCExpert.Web.Api.Common.Impl;
using PCExpert.Web.Api.Common.WebModel;
using PCExpert.Web.Api.Controllers;
using PCExpert.Web.Model.Core;

namespace PCExpert.Web.Api.LinkSetters
{
	public class ComponentInterfaceModelLinkSetter : LinkSetter<ComponentInterfaceModel>
	{
		protected override void SetLinks(UrlHelper urlHelper, ComponentInterfaceModel model)
		{
			var links = new[]
			{
				new WebLink("self",
					CreateRoute(urlHelper, WebApiRouteNames.DefaultApi, typeof (ComponentInterfaceController)),
					WebRequestMethods.Http.Get)
			};
			model.Links = links;
		}
	}
}