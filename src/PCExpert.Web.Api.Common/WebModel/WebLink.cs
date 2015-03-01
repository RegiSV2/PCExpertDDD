using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common.WebModel
{
	/// <summary>
	///     Represents a REST resource link
	/// </summary>
	public sealed class WebLink
	{
		public WebLink(string rel, string href, string method)
		{
			Argument.NotNull(rel);
			Argument.NotNull(href);
			Argument.NotNull(method);

			Rel = rel;
			Href = href;
			Method = method;
		}

		public string Rel { get; private set; }
		public string Href { get; private set; }
		public string Method { get; private set; }
	}
}