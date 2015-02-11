using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common.WebModel
{
	public class LinkSettingEngine
	{
		private readonly UrlHelper _urlHelper;

		private readonly IDictionary<Type, ILinkSetter> _linkSetters;

		public LinkSettingEngine(IDictionary<Type, ILinkSetter> linkSetters)
		{
			Argument.NotNull(linkSetters);

			_urlHelper = new UrlHelper(HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage);
			_linkSetters = linkSetters;
		}

		public void SetLinks(ILinksContaining model)
		{
			Argument.NotNull(model);

			ILinkSetter setter;
			if(_linkSetters.TryGetValue(model.GetType(), out setter))
				setter.SetLinks(_urlHelper, model);
		}
	}
}