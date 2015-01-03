using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class CharacteristicValueConfiguration : EntityTypeConfiguration<CharacteristicValue>
	{
		private const string DiscriminatorColumn = "type";

		public CharacteristicValueConfiguration()
		{
			HasKey(x => new { x.CharacteristicId, x.ComponentId });
			HasRequired(x => x.Characteristic).WithMany().HasForeignKey(x => x.CharacteristicId);
			HasRequired(x => x.Component).WithMany().HasForeignKey(x => x.ComponentId);
			Map<IntCharacteristicValue>(m => m.Requires(DiscriminatorColumn).HasValue(1));
			Map<BoolCharacteristicValue>(m => m.Requires(DiscriminatorColumn).HasValue(2));
		}
	}
}