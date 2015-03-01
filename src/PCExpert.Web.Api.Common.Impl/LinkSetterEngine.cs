using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using PCExpert.DomainFramework.Utils;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Api.Common.Impl
{
	public class LinkSetterEngine : ILinkSetterEngine
	{
		private readonly IDictionary<Type, ILinkSetter> _linkSetters;
		private UrlHelper _urlHelper;

		public LinkSetterEngine(IDictionary<Type, ILinkSetter> linkSetters)
		{
			Argument.NotNull(linkSetters);

			CreateUrlHelper();
			_linkSetters = linkSetters;
		}

		private void CreateUrlHelper()
		{
			_urlHelper =
				HttpContext.Current == null
					? new UrlHelper()
					: new UrlHelper(HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage);
		}

		public void SetLinks(ILinksContaining model)
		{
			Argument.NotNull(model);

			ILinkSetter setter;
			if (_linkSetters.TryGetValue(model.GetType(), out setter))
				setter.SetLinks(_urlHelper, model);
		}
	}
}