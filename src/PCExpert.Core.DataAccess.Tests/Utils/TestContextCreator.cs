using System.Data.Entity;
using FluentValidation;
using Moq;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	public static class TestContextCreator
	{
		public static PCExpertContext Create()
		{
			return new PCExpertContext("testConString",
				new DropCreateDatabaseAlways<PCExpertContext>(),
				new Mock<IValidatorFactory>().Object);
		}

		public static PCExpertContext Create(IValidatorFactory factory)
		{
			return new PCExpertContext("testConString",
				new DropCreateDatabaseAlways<PCExpertContext>(),
				factory);
		}
	}
}