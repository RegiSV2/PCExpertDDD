using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class NumericCharacteristicValueConfiguration : EntityTypeConfiguration<NumericCharacteristicValue>
	{
		public NumericCharacteristicValueConfiguration()
		{
			Property(x => x.Value).HasColumnName("IntValue");
		}
	}
}