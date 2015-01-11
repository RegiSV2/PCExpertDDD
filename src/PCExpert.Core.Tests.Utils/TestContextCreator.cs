using System.Data.Entity;
using FluentValidation;
using Moq;
using PCExpert.Core.DataAccess;
using PCExpert.DomainFramework.EF;

namespace PCExpert.Core.Tests.Utils
{
	public static class TestContextCreator
	{
		public static TestDbContextProvider Create()
		{
			return new TestDbContextProvider(
				new PCExpertContext("testConString",
					new DropCreateDatabaseAlways<PCExpertContext>(),
					new Mock<IValidatorFactory>().Object));
		}

		public static TestDbContextProvider Create(IValidatorFactory factory)
		{
			return new TestDbContextProvider(
				new PCExpertContext("testConString",
					new DropCreateDatabaseAlways<PCExpertContext>(),
					factory));
		}
	}

	public class TestDbContextProvider : IDbContextProvider
	{
		public TestDbContextProvider(PCExpertContext context)
		{
			PCExpertContext = context;
		}

		public PCExpertContext PCExpertContext { get; private set; }

		public DbContext DbContext
		{
			get { return PCExpertContext; }
		}
	}
}