using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common.WebModel
{
	public class LinkSettingEngine
	{
		private readonly UrlHelper _urlHelper;

		private readonly IDictionary<Type, ILinkSetter> _linkSetters;

		public LinkSettingEngine(CurrentRequest currentRequest, IDictionary<Type, ILinkSetter> linkSetters)
		{
			Argument.NotNull(currentRequest);
			Argument.NotNull(linkSetters);

			_urlHelper = new UrlHelper(currentRequest.Value);
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