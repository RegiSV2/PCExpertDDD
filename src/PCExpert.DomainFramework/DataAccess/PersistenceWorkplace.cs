using System.Collections.Generic;
using System.Linq;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.DataAccess
{
	public abstract class PersistenceWorkplace
	{
		public void Save<TEntity>(TEntity entity)
			where TEntity : Entity
		{
			Argument.NotNull(entity);
			if (entity.IsPersisted)
				Insert(entity);
			else
				Update(entity);
		}

		public abstract IQueryable<TEntity> Query<TEntity>()
			where TEntity : class;

		public abstract void Insert<TEntity>(TEntity entity)
			where TEntity : class;

		public abstract void Insert<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class;

		public abstract void Update<TEntity>(TEntity entity)
			where TEntity : class;

		public abstract void Delete<TEntity>(TEntity entity)
			where TEntity : class;

		public abstract void Delete<TEntity>(IEnumerable<TEntity> entity)
			where TEntity : class;
	}
}