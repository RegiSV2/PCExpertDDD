using System.Data.Entity;
using System.Web;

namespace PCExpert.DomainFramework.EF
{
	public abstract class HttpContextDbContextProvider : IDbContextProvider
	{
		private const string ContextKey = "DB context";

		public DbContext DbContext
		{
			get
			{
				if (!HttpContext.Current.Items.Contains(ContextKey))
				{
					HttpContext.Current.Items[ContextKey] = CreateDbContext();
				}
				return (DbContext) HttpContext.Current.Items[ContextKey];
			}
		}

		protected abstract DbContext CreateDbContext();
	}
}