using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class PCConfigurationTests
	{
		protected PCConfiguration DefaultConfiguration;

		[SetUp]
		public void EstablishContext()
		{
			DefaultConfiguration = new PCConfiguration();
		}
	}

	public class PCConfigurationConstructorTests : PCConfigurationTests
	{
		[Test]
		public void Constructor_JustCreated_ShouldHaveNullName()
		{
			Assert.That(DefaultConfiguration.Name, Is.EqualTo(null));
		}

		[Test]
		public void Constructor_JustCreated_ShouldHaveEmptyComponentsList()
		{
			Assert.That(DefaultConfiguration.Components, Is.Not.Null.And.Empty);
		}

		[Test]
		public void Constructor_JustCreated_ShouldHaveZeroPrice()
		{
			Assert.That(DefaultConfiguration.CalculatePrice(), Is.EqualTo(0));
		}
	}

	//public class PCConfigurationNameTests : PCConfigurationTests
	//{
	//	[Test]
	//	public void WithName_NullNameAndPrivateStatus_
	//}
}