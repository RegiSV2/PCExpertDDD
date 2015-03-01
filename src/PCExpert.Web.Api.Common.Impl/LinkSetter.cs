using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http.Routing;
using PCExpert.DomainFramework.Utils;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Api.Common.Impl
{
	public abstract class LinkSetter<TLinksContaining> : ILinkSetter
		where TLinksContaining : class, ILinksContaining
	{
		private const string ControllerSuffix = "Controller";

		public void SetLinks(UrlHelper urlHelper, object model)
		{
			var linksCont = model as TLinksContaining;
			Debug.Assert(linksCont != null);

			linksCont.Links = new List<WebLink>();
			SetLinks(urlHelper, linksCont);
		}

		public Type ModelType
		{
			get { return typeof (TLinksContaining); }
		}

		protected abstract void SetLinks(UrlHelper urlHelper, TLinksContaining model);

		protected string CreateRoute(UrlHelper urlHelper, string routeName, Type controllerType)
		{
			Argument.NotNull(urlHelper);
			Argument.NotNullAndNotEmpty(routeName);
			Argument.NotNull(controllerType);
			Argument.NotNegative(controllerType.Name.Length - ControllerSuffix.Length);

			var controllerName = controllerType.Name.Substring(0, controllerType.Name.Length - ControllerSuffix.Length);
			return urlHelper.Link(routeName, new {controller = controllerName});
		}
	}
}