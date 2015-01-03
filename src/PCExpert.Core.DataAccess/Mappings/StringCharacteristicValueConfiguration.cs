using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class StringCharacteristicValueConfiguration : EntityTypeConfiguration<StringCharacteristicValue>
	{
		public StringCharacteristicValueConfiguration()
		{
			Property(x => x.Value).HasColumnName("StringValue");
		}
	}
}