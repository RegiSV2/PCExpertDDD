using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class PCConfigurationConfiguration : EntityTypeConfiguration<PCConfiguration>
	{
		public PCConfigurationConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Name).IsOptional().HasMaxLength(250);
			Property(x => x.PublicName).IsOptional().HasMaxLength(250);
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