using System.Data.Entity;
using FluentValidation;
using PCExpert.DomainFramework.EF;

namespace PCExpert.Core.DataAccess
{
	public class HttpContextPCExpertContextProvider : HttpContextDbContextProvider
	{
		private readonly string _connectionStringName;
		private readonly IValidatorFactory _validatorFactory;

		public HttpContextPCExpertContextProvider(string connectionStringName, IValidatorFactory validatorFactory)
		{
			_validatorFactory = validatorFactory;
			_connectionStringName = connectionStringName;
		}

		protected override DbContext CreateDbContext()
		{
			return new PCExpertContext(_connectionStringName,
				new NullDatabaseInitializer<PCExpertContext>(),
				_validatorFactory);
		}
	}
}