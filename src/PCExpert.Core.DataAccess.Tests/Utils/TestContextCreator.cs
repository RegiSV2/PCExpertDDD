using System.Data.Entity;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	public static class TestContextCreator
	{
		public static PCExpertContext Create()
		{
			return new PCExpertContext("testConString",
				new DropCreateDatabaseAlways<PCExpertContext>());
		}
	}
}