using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class ComponentCharacteristicConfiguration : EntityTypeConfiguration<ComponentCharacteristic>
	{
		private const string DiscriminatorColumn = "type";

		public ComponentCharacteristicConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Map<IntCharacteristic>(m => m.Requires(DiscriminatorColumn).HasValue(1));
			Map<BoolCharacteristic>(m => m.Requires(DiscriminatorColumn).HasValue(2));
		}
	}
}