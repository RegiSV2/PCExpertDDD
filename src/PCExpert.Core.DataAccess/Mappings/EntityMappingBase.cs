using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class EntityMappingBase<TEntity> : MappingBase<TEntity>
		where TEntity: Entity
	{
		protected override void Map(EntityTypeConfiguration<TEntity> mapping)
		{
			mapping.HasKey(x => x.Id);
		}
	}
}