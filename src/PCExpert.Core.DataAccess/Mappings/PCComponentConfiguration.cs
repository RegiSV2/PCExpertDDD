using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class PCComponentConfiguration : EntityTypeConfiguration<PCComponent>
	{
		public PCComponentConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			HasMany(this.PrivateProperty<PCComponent, ICollection<PCComponent>>("Components"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("ParentComponent_id");
					m.MapRightKey("ChildComponent_id");
					m.ToTable("ParentToChildComponents");
				});
			HasMany(this.PrivateProperty<PCComponent, ICollection<ComponentInterface>>("ContainedInterfaces"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("Component_id");
					m.MapRightKey("ContainedSlot_id");
					m.ToTable("ComponentToContainedSlots");
				});
			HasMany(this.PrivateProperty<PCComponent, ICollection<ComponentInterface>>("PlugInterfaces"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("Component_id");
					m.MapRightKey("ContainedSlot_id");
					m.ToTable("ComponentToPlugSlots");
				});
			HasMany(this.PrivateProperty<PCComponent, ICollection<CharacteristicValue>>("CharacteristicVals"))
				.WithRequired(x => x.Component)
				.WillCascadeOnDelete();
		}
	}
}