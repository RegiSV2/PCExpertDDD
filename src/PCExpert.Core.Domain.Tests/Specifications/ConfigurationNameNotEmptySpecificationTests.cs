using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	public class ConfigurationNameNotEmptySpecificationTests :
		PCConfigurationSpecificationsTests<ConfigurationNameNotEmptySpecification>
	{
		[SetUp]
		public override void EstablishContext()
		{
			base.EstablishContext();
			Specification = new ConfigurationNameNotEmptySpecification();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void IsSatisfied_NullOrEmptyName_ShouldFail(string name)
		{
			//Arrange
			Configuration.WithName(name);

			//Assert
			Assert.That(!Specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_NotEmptyName_ShouldPass()
		{
			//Arrange
			Configuration.WithName("some name");

			//Assert
			Assert.That(Specification.IsSatisfiedBy(Configuration));
		}
	}
}