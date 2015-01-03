using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class IntCharacteristicValueConfiguration : EntityTypeConfiguration<DecimalCharacteristicValue>
	{
		public IntCharacteristicValueConfiguration()
		{
			Property(x => x.Value).HasColumnName("IntValue");
		}
	}
}