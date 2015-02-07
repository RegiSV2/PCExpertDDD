using System;
using System.Data.Entity;
using PCExpert.Core.DataAccess;
using PCExpert.Core.DataAccess.Tests;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.DataAccess;
using PCExpert.DomainFramework.EF;
using PCExpert.DomainFramework.Validation;

namespace PCExpert.TestDBCreator
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Initializing DB structure...");
			using (var context = new PCExpertContext("dbToFill", 
				new DropCreateDatabaseAlways<PCExpertContext>(),
				new DomainValidatorFactory()))
			{
				Console.WriteLine("DB structure initialized");
				Console.WriteLine("Generating test data");

				var workplace = new EfWorkplace(new TestDbContextProvider(context));
				var dataCreator = new TestDataGenerator();
				dataCreator.CreateRandomData(workplace);

				context.SaveChanges();

				Console.WriteLine("Test data generated");
			}
		}
	}
}
