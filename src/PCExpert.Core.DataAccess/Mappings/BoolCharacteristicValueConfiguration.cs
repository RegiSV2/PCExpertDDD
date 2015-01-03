using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class BoolCharacteristicValueConfiguration : EntityTypeConfiguration<BoolCharacteristicValue>
	{
		public BoolCharacteristicValueConfiguration()
		{
			Property(x => x.Value).HasColumnName("BoolValue");
		}
	}
}