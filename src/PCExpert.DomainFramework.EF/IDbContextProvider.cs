using System.Data.Entity;

namespace PCExpert.DomainFramework.EF
{
	public interface IDbContextProvider
	{
		DbContext DbContext { get; }
	}
}