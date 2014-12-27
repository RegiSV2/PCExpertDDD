using System;
using System.Reflection;
using Moq;
using PCExpert.Core.DomainFramework;

namespace PCExpert.Core.Tests.Utils
{
	public static class EntityExtensions
	{
		public static TEntity WithId<TEntity>(this TEntity entity, Guid id)
			where TEntity : Entity
		{
			var idProperty = typeof (Entity).GetProperty("Id",
				BindingFlags.GetProperty | BindingFlags.Instance |
				BindingFlags.NonPublic | BindingFlags.Public);
			idProperty.SetValue(entity, id);

			return entity;
		}

		public static Mock<TEntity> WithId<TEntity>(this Mock<TEntity> entity, Guid id)
			where TEntity : Entity
		{
			entity.SetupGet(x => x.Id).Returns(id);
			return entity;
		}
	}
}