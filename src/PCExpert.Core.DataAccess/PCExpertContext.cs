using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using PCExpert.Core.DataAccess.Mappings;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess
{
	public class PCExpertContext : DbContext
	{
		public PCExpertContext(IDatabaseInitializer<PCExpertContext> initializer)
		{
			Database.SetInitializer(initializer);
		}

		public PCExpertContext(string connectionStringName,
			IDatabaseInitializer<PCExpertContext> initializer)
			: base(connectionStringName)
		{
			Database.SetInitializer(initializer);
		}

		public DbSet<PCComponent> PCComponents { get; set; }
		public DbSet<ComponentInterface> ComponentInterfaces { get; set; }
		public DbSet<PCConfiguration> PCConfigurations { get; set; }
		public DbSet<ComponentCharacteristic> Characteristics { get; set; }
		public DbSet<CharacteristicValue> CharacteristicValues { get; set; } 

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations
				.Add(new ComponentInterfaceConfiguration())
				.Add(new PCComponentConfiguration())
				.Add(new PCConfigurationConfiguration())
				.Add(new ComponentCharacteristicConfiguration())
				.Add(new CharacteristicValueConfiguration())
				.Add(new IntCharacteristicValueConfiguration())
				.Add(new BoolCharacteristicValueConfiguration())
				.Add(new StringCharacteristicValueConfiguration());
		}
	}
}