using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Api.Common.Impl.Tests
{
	[TestFixture]
	public class LinkSetterEngineTests
	{
		private FakeLinkContaining _entity;

		[SetUp]
		public void EstablishContext()
		{
			_entity = new FakeLinkContaining();
		}

		[Test]
		public void SetLinks_LinkSetterRegistered_ShouldCallLinkSetter()
		{
			//Arrange
			var mockLinkSetter = new MockLinkSetter();
			var engine = new LinkSetterEngine(new ILinkSetter[] { mockLinkSetter }.ToDictionary(x => x.ModelType));

			//Act
			engine.SetLinks(_entity);

			//Assert
			Assert.That(mockLinkSetter.LastModelToSetLinks == _entity);
		}

		[Test]
		public void SetLinks_LikSetterNotRegistered_ShouldNotSetLinks()
		{
			//Arrange
			var engine = new LinkSetterEngine(new Dictionary<Type, ILinkSetter>());

			//Act
			engine.SetLinks(_entity);

			//Assert
			Assert.That(_entity.Links == null);
		}
	}
}