using System;
using System.Reflection;
using PCExpert.Core.Domain;

namespace PCExpert.Core.Tests.Utils
{
	public static class EntityExtensions
	{
		public static TEntity WithId<TEntity>(this TEntity entity, Guid id)
			where TEntity : Entity
		{
			var idProperty = typeof (Entity).GetProperty("Id", BindingFlags.GetProperty | BindingFlags.Instance);
			idProperty.SetValue(entity, id);

			return entity;
		}
	}
}