using System;
using Moq;
using NUnit.Framework;

namespace PCExpert.Core.DomainFramework.Tests
{
	[TestFixture]
	public class EntityTests
	{
		[Test]
		public void SameIdentityAs_NullArgument_ShouldReturnFalse()
		{
			//Arrange
			var entityMock1 = CreateMockEntity(Guid.NewGuid());

			//Assert
			Assert.That(entityMock1.SameIdentityAs(null), Is.EqualTo(false));
		}

		[Test]
		public void SameIdentityAs_ArgumentHasSameId_ShouldReturnTrue()
		{
			//Arrange
			var commonId = Guid.NewGuid();
			var entityMock1 = CreateMockEntity(commonId);
			var entityMock2 = CreateMockEntity(commonId);

			//Assert
			Assert.That(entityMock1.SameIdentityAs(entityMock2));
		}

		[Test]
		public void SameIdentityAs_ArgumentHasOtherId_ShouldReturnFalse()
		{
			//Arrange
			var entityMock1 = CreateMockEntity(Guid.NewGuid());
			var entityMock2 = CreateMockEntity(Guid.NewGuid());

			//Assert
			Assert.That(entityMock1.SameIdentityAs(entityMock2), Is.EqualTo(false));
		}

		[Test]
		public void SameIdentityAs_NotPersistedButDifferentEntities_ShouldReturnFalse()
		{
			//Arrange
			var entityMock1 = CreateMockEntity(Guid.Empty);
			var entityMock2 = CreateMockEntity(Guid.Empty);

			//Assert
			Assert.That(entityMock1.SameIdentityAs(entityMock2), Is.EqualTo(false));
		}

		[Test]
		public void IsPersisted_IdIsEmpty_ShouldReturnFalse()
		{
			//Arrange
			var entityMock = CreateMockEntity(Guid.Empty);

			//Assert
			Assert.That(entityMock.IsPersisted, Is.False);
		}

		[Test]
		public void IsPersisted_IdIsNotEmpty_ShouldReturnTrue()
		{
			//Arrange
			var entityMock = CreateMockEntity(Guid.NewGuid());

			//Assert
			Assert.That(entityMock.IsPersisted);
		}

		private Entity CreateMockEntity(Guid entityId)
		{
			var mock = new Mock<Entity>();
			mock.SetupGet(x => x.Id).Returns(entityId);
			return mock.Object;
		}
	}
}