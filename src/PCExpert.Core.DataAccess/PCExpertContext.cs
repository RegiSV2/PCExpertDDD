using System.Data;
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
			:base(connectionStringName)
		{
			Database.SetInitializer(initializer);
		}

		public DbSet<PCComponent> PCComponents { get; set; }

		public DbSet<ComponentInterface> ComponentInterfaces { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations
				.Add(new ComponentInterfaceConfiguration())
				.Add(new PCComponentConfiguration());
		}
	}
}