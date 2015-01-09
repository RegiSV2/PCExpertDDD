using System;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	public class PCConfigurationTests
	{
		protected PCConfiguration DefaultConfiguration;

		[SetUp]
		public void EstablishContext()
		{
			DefaultConfiguration = new PCConfiguration();
		}
	}

	[TestFixture]
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

	[TestFixture]
	public class PCConfigurationNameTests : PCConfigurationTests
	{
		[Test]
		[TestCase(null)]
		[TestCase("")]
		[TestCase("new name")]
		public void WithName_AnyName_ShouldChangeName(string newName)
		{
			//Arrange
			DefaultConfiguration.WithName("initial name");
			Assert.That(DefaultConfiguration.Name, Is.Not.EqualTo(newName));

			//Act
			DefaultConfiguration.WithName(newName);

			//Assert
			Assert.That(DefaultConfiguration.Name, Is.EqualTo(newName));
		}
	}

	[TestFixture]
	public class PCConfigurationComponentsTests : PCConfigurationTests
	{
		[Test]
		public void WithComponent_NullComponent_ShouldThrowArgumentNullException()
		{
			Assert.That(() => DefaultConfiguration.WithComponent(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithComponent_NewComponent_ShouldBeAddedToCollection()
		{
			//Arrange
			var firstComponent = DomainObjectsCreator.CreateComponent(0, ComponentType.PowerSupply);
			var secondComponent = DomainObjectsCreator.CreateComponent(1, ComponentType.Motherboard);

			//Act
			DefaultConfiguration
				.WithComponent(firstComponent)
				.WithComponent(secondComponent);

			//Assert
			Assert.That(DefaultConfiguration.Components.Count, Is.EqualTo(2));
			Assert.That(DefaultConfiguration.Components.Contains(firstComponent));
			Assert.That(DefaultConfiguration.Components.Contains(secondComponent));
		}

		[Test]
		public void WithComponent_ComponentWithSameIdentity_ShouldThrowDuplicateElementException()
		{
			//Arrange
			var component = DomainObjectsCreator.CreateComponent(0, ComponentType.Motherboard);
			DefaultConfiguration.WithComponent(component);

			//Assert
			Assert.That(() => DefaultConfiguration.WithComponent(component), Throws.InstanceOf<DuplicateElementException>());
		}
	}

	[TestFixture]
	public class PCConfigurationCalculatePriceTests : PCConfigurationTests
	{
		[Test]
		public void CalculatePrice_SomeComponentsAdded_ShouldReturnSumPriceOfAllComponents()
		{
			//Arrange
			DefaultConfiguration
				.WithComponent(
					DomainObjectsCreator.CreateComponent(0, ComponentType.Motherboard).WithAveragePrice(100m))
				.WithComponent(
					DomainObjectsCreator.CreateComponent(1, ComponentType.PowerSupply).WithAveragePrice(200m))
				.WithComponent(
					DomainObjectsCreator.CreateComponent(2, ComponentType.SolidStateDrice).WithAveragePrice(350.5m));

			//Assert
			Assert.That(DefaultConfiguration.CalculatePrice(), Is.EqualTo(650.5m));
		}
	}

	[TestFixture]
	public class PCConfigurationStatusTests : PCConfigurationTests
	{
		[Test]
		public void Status_AfterCreation_ShouldBeEqualToPersonal()
		{
			Assert.That(DefaultConfiguration.Status, Is.EqualTo(PCConfigurationStatus.Personal));
		}

		[Test]
		public void MoveToStatus_ValidStatus_ShouldChangeStatus()
		{
			//Act
			Assert.That(DefaultConfiguration.Status, Is.Not.EqualTo(PCConfigurationStatus.Published));
			DefaultConfiguration.MoveToStatus(PCConfigurationStatus.Published);

			//Assert
			Assert.That(DefaultConfiguration.Status, Is.EqualTo(PCConfigurationStatus.Published));
		}

		[Test]
		public void MoveToStatus_InvalidStatus_ShouldThrowArgumentException()
		{
			Assert.That(() => DefaultConfiguration.MoveToStatus((PCConfigurationStatus) 1234),
				Throws.InstanceOf<ArgumentException>());
		}
	}
}