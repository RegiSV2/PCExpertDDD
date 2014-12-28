using System;
using NUnit.Framework;
using PCExpert.Core.DataAccess.Tests.Utils;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture]
	public class EFWorkplaceTests
	{
		private EfWorkplace _workplace;

		[SetUp]
		public void EstablishContext()
		{
			_workplace = new EfWorkplace(TestContextCreator.Create());
		}

		[Test]
		public void Save_NullEntity_SholdThrowArgumentNullException()
		{
			Assert.That(() => _workplace.Save<PCComponent>(null),
				Throws.InstanceOf<ArgumentNullException>());
		}
	}
}