using System.Data.Entity.Migrations;

namespace PCExpert.Core.DataAccess.Migrations
{
	public partial class CreateIndexes : DbMigration
	{
		public override void Up()
		{
			Sql("CREATE UNIQUE NONCLUSTERED INDEX Idx_PCConfigurations_PublicName " +
			    "ON PCConfigurations(PublicName) " +
			    "WHERE PublicName IS NOT NULL;");
		}

		public override void Down()
		{
		}
	}
}