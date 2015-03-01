using System.Web;
using System.Web.Hosting;
using System.Web.Http.Routing;
using Moq;
using NUnit.Framework;

namespace PCExpert.Web.Api.Common.Impl.Tests
{
	[TestFixture]
	public class LinkSetterTests
	{
		[Test]
		public void SetLinks_UrlHelperAndModelAreValid_ShouldSetLinks()
		{
			//Arrange
			var linksSetter = new MockLinkSetter();
			var entity = new FakeLinkContaining();

			//Act
			linksSetter.SetLinks(new UrlHelper(), entity);

			//Assert
			Assert.That(entity.Links != null);
			Assert.That(linksSetter.LastModelToSetLinks == entity);
		}
	}
}