using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Reflection;

namespace PCExpert.Core.DataAccess.Mappings
{
	public static class EntityTypeConfigurationExtensions
	{
		public static Expression<Func<TEntity, T>> PrivateProperty<TEntity, T>(this EntityTypeConfiguration<TEntity> config,
			string propertyName)
			where TEntity : class
		{
			var property = typeof (TEntity).GetProperty(propertyName,
				BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
			var param = Expression.Parameter(typeof (TEntity));
			return Expression.Lambda<Func<TEntity, T>>(
				Expression.Property(param, property), param);
		}
	}
}