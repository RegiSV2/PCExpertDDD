using System.Web.Http.Routing;

namespace PCExpert.Web.Api.Common.Impl.Tests
{
	public class MockLinkSetter : LinkSetter<FakeLinkContaining>
	{
		public FakeLinkContaining LastModelToSetLinks { get; private set; }

		protected override void SetLinks(UrlHelper urlHelper, FakeLinkContaining model)
		{
			LastModelToSetLinks = model;
		}
	}
}