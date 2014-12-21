using System.Data.Entity;

namespace PCExpert.Core.DataAccess
{
	public interface IMapping
	{
		void MapEntity(DbModelBuilder modelBuilder);
	}
}