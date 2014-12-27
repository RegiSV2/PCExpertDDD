using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	public class ConfigurationNameMaxLengthSpecificationTests :
		PCConfigurationSpecificationsTests<ConfigurationNameMaxLengthSpecification>
	{
		private const int MaxNameLength = 55;

		public override void EstablishContext()
		{
			base.EstablishContext();
			Specification = new ConfigurationNameMaxLengthSpecification(MaxNameLength);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void IsSatisfied_NameIsEmpty_ShouldPass(string name)
		{
			//Arrange
			Configuration.WithName(name);

			//Assert
			Assert.That(Specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_NameTooLong_ShouldFail()
		{
			//Arrange
			Configuration.WithName("".PadLeft(MaxNameLength + 1));

			//Assert
			Assert.That(!Specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_NameNotTooLong_ShouldPass()
		{
			//Arrange
			Configuration.WithName("".PadLeft(MaxNameLength));

			//Assert
			Assert.That(Specification.IsSatisfiedBy(Configuration));
		}
	}
}