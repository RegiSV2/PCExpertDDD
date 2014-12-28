using System;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DomainFramework.Tests
{
	[TestFixture]
	public class PersistenceWorkplaceTests
	{
		private TestPersistenceWorkplace _workplace;

		[SetUp]
		public void EstablishContext()
		{
			_workplace = new TestPersistenceWorkplace();
		}

		[Test]
		public void Save_NullArgument_ShouldFail()
		{
			Assert.That(() => _workplace.Save<Entity>(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Save_PersistedArgument_ShouldCallInsert()
		{
			//Arrange
			var entity = new Mock<Entity>().WithId(Guid.NewGuid()).Object;

			//Act
			_workplace.Save(entity);

			//Assert
			Assert.That(_workplace.IsInsertCalled);
			Assert.That(!_workplace.IsUpdateCalled);
		}

		[Test]
		public void Save_NotPersistedArgument_ShouldCallUpdate()
		{
			//Arrange
			var entity = new Mock<Entity>().Object;

			//Act
			_workplace.Save(entity);

			//Assert
			Assert.That(_workplace.IsUpdateCalled);
			Assert.That(!_workplace.IsInsertCalled);
		}
	}
}