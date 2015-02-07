using System;
using System.Web.Http.Routing;

namespace PCExpert.Web.Api.Common.WebModel
{
	public interface ILinkSetter
	{
		void SetLinks(UrlHelper urlHelper, object model);

		Type ModelType { get; }
	}
}