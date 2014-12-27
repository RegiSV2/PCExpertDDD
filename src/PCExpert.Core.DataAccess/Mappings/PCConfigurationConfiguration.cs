using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class PCConfigurationConfiguration : EntityTypeConfiguration<PCConfiguration>
	{
		public PCConfigurationConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			HasMany(this.PrivateProperty<PCConfiguration, ICollection<PCComponent>>("ContainedComponents"))
				.WithMany()
				.Map(x =>
				{
					x.MapLeftKey("config_Id");
					x.MapRightKey("component_Id");
					x.ToTable("ComponentToConfiguration");
				});
		}
	}
}