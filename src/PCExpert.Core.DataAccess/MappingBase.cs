using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PCExpert.Core.DataAccess
{
	public abstract class MappingBase<TEntity> : IMapping
		where TEntity : class
	{
		public virtual void MapEntity(DbModelBuilder modelBuilder)
		{
			Map(modelBuilder.Entity<TEntity>());
		}

		protected abstract void Map(EntityTypeConfiguration<TEntity> mapping);
	}
}