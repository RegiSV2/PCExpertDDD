using System;
using Moq;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
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

		private Entity CreateMockEntity(Guid entityId)
		{
			var mock = new Mock<Entity>(entityId);
			mock.SetupGet(x => x.Id).Returns(entityId);
			return mock.Object;
		}
	}
}