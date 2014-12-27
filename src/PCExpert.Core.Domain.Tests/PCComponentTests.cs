using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class PCComponentTests
	{
		protected const decimal ComponentPrice = 100m;
		protected const ComponentType ValidComponentType = ComponentType.HardDiskDrive;
		protected PCComponent DefaultComponent;

		[SetUp]
		public void EstablishContext()
		{
			DefaultComponent = CreateComponent(1);
		}

		protected static PCComponent CreateComponent(int componentNameValue)
		{
			return DomainObjectsCreator.CreateComponent(componentNameValue, ComponentType.SoundCard);
		}

		protected static Mock<ComponentInterface> CreateInterface(int interfaceNameValue)
		{
			var interfaceMock = new Mock<ComponentInterface>();
			interfaceMock.SetupGet(x => x.Id).Returns(Guid.NewGuid());
			interfaceMock.Setup(x => x.Name).Returns(NamesGenerator.ComponentInterfaceName(interfaceNameValue));
			return interfaceMock;
		}
	}

	public class PCComponentConstructorTests : PCComponentTests
	{
		[Test]
		public void Constructor_Called_CanCreatePCComponent()
		{
			//Arrange
			var componentName = NamesGenerator.ComponentName();
			var component = new PCComponent(componentName, ValidComponentType);

			//Assert
			Assert.That(component.Name, Is.EqualTo(componentName));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Constructor_CalledWithEmptyName_ShouldThrowArgumentNullException(string name)
		{
			Assert.That(() => new PCComponent(name, ValidComponentType), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Constructor_CalledWithInvalidType_ShouldThrowArgumentException()
		{
			Assert.That(() => new PCComponent(
				NamesGenerator.ComponentName(),
				(ComponentType) 234567),
				Throws.ArgumentException);
		}
	}

	public class PCComponentAveragePriceTests : PCComponentTests
	{
		[Test]
		public void AveragePrice_AfterInstanceCreation_ShouldBeZero()
		{
			Assert.That(DefaultComponent.AveragePrice, Is.EqualTo(0));
		}

		[Test]
		public void SetAveragePrice_Called_ShouldSetAveragePrice()
		{
			//Act
			DefaultComponent.WithAveragePrice(ComponentPrice);

			//Assert
			Assert.That(DefaultComponent.AveragePrice, Is.EqualTo(ComponentPrice));
		}

		[Test]
		public void SetAveragePrice_NegativeValue_ShouldThrowArgumentOutOfRangeException()
		{
			Assert.That(() => DefaultComponent.WithAveragePrice(-15m), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}

	public class PCComponentNameTests : PCComponentTests
	{
		[Test]
		public void SetName_Called_ShouldSetNewName()
		{
			//Arrange
			var newComponentName = NamesGenerator.ComponentName(2);

			//Act
			DefaultComponent.WithName(newComponentName);

			//Assert
			Assert.That(DefaultComponent.Name, Is.EqualTo(newComponentName));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void SetName_EmptyString_ShouldThrowArgumentNullException(string name)
		{
			Assert.That(() => DefaultComponent.WithName(name), Throws.InstanceOf<ArgumentNullException>());
		}
	}

	public class PCComponentContainedComponentsTests : PCComponentTests
	{
		[Test]
		public void ContainedComponents_NoComponentsAdded_ShouldReturnEmptyCollection()
		{
			Assert.That(DefaultComponent.ContainedComponents, Is.Empty);
		}

		[Test]
		public void WithContainedComponent_NotNullComponent_ComponentShouldBeAddedToContainedComponents()
		{
			//Arrange
			var parentComponent = CreateComponent(1);
			var childComponent = CreateComponent(2);

			//Act
			parentComponent.WithContainedComponent(childComponent);

			//Assert
			Assert.That(parentComponent.ContainedComponents.Count, Is.EqualTo(1));
			Assert.That(parentComponent.ContainedComponents.Contains(childComponent));
		}

		[Test]
		public void WithContainedComponent_NullComponent_ShouldThrowArgumentNullException()
		{
			Assert.That(() => DefaultComponent.WithContainedComponent(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithContainedComponent_ComponentAlreadyAdded_ShouldThrowDuplicateElementException()
		{
			//Arrange
			var parentComponent = CreateComponent(1);
			var childComponent = CreateComponent(2);
			parentComponent.WithContainedComponent(childComponent);

			//Assert
			Assert.That(() => parentComponent.WithContainedComponent(childComponent),
				Throws.InstanceOf<DuplicateElementException>());
		}

		[Test]
		public void WithContainedComponent_SameReferenceAsParent_ShouldThrowArgumentException()
		{
			Assert.That(() => DefaultComponent.WithContainedComponent(DefaultComponent),
				Throws.InstanceOf<ArgumentException>());
		}

		[Test]
		public void WithContainedComponent_ComponentsWithSameIdentities_ShouldThrowArgumentException()
		{
			//Arrange
			var commonId = Guid.NewGuid();
			var parentComponent = CreateComponent(1).WithId(commonId);
			var childComponent = CreateComponent(2).WithId(commonId);

			//Assert
			Assert.That(() => parentComponent.WithContainedComponent(childComponent),
				Throws.InstanceOf<ArgumentException>());
		}

		[Test]
		public void WithContainedComponent_ComponentWithSameIdentityAdded_ShouldThrowArgumentException()
		{
			//Arrange
			var commonId = Guid.NewGuid();
			var firstComponent = CreateComponent(1).WithId(commonId);
			var secondComponent = CreateComponent(2).WithId(commonId);
			DefaultComponent.WithContainedComponent(firstComponent);

			//Assert
			Assert.That(() => DefaultComponent.WithContainedComponent(secondComponent),
				Throws.InstanceOf<DuplicateElementException>());
		}
	}

	public class PCComponentInterfaceConnectionsTests : PCComponentTests
	{
		[Test]
		public void PlugSlot_NoPlugSlotSet_ShouldReturnNull()
		{
			//Assert
			Assert.That(DefaultComponent.PlugSlot.SameIdentityAs(ComponentInterface.NullObject));
		}

		[Test]
		public void WithPlugSlot_NullArgument_ShouldThrowArgumentNullException()
		{
			//Assert
			Assert.That(() => DefaultComponent.WithPlugSlot(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithPlugSlot_NotNullArgument_ShouldSetCorrectValue()
		{
			//Arrange
			var componentInterface = CreateInterface(1);

			//Act
			DefaultComponent.WithPlugSlot(componentInterface.Object);

			//Assert
			Assert.That(DefaultComponent.PlugSlot, Is.EqualTo(componentInterface.Object));
		}

		[Test]
		public void ContainedSlots_NoSlotsAdded_ShouldReturnEmptyCollection()
		{
			Assert.That(DefaultComponent.ContainedSlots.Count, Is.EqualTo(0));
		}

		[Test]
		public void WithContainedSlot_NullSlot_ShouldThrowArgumentNullException()
		{
			Assert.That(() => DefaultComponent.WithContainedSlot(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithContainedSlot_NotNullSlot_ShouldAddSlotToContainedSLots()
		{
			//Arrange
			var slotToAdd = CreateInterface(1);
			DefaultComponent.WithContainedSlot(slotToAdd.Object);

			//Assert
			Assert.That(DefaultComponent.ContainedSlots.Count, Is.EqualTo(1));
			Assert.That(DefaultComponent.ContainedSlots.Contains(slotToAdd.Object));
		}

		[Test]
		public void WithContainedSlot_DifferentSlotWithSameIdentityAdded_ShouldThrowDuplicateElementException()
		{
			//Arrange
			var commonId = Guid.NewGuid();
			var firstSlot = CreateInterface(1).WithId(commonId).Object;
			DefaultComponent.WithContainedSlot(firstSlot);

			//Act
			var secondSlot = CreateInterface(2).WithId(commonId).Object;
			DefaultComponent.WithContainedSlot(secondSlot);

			//Assert
			Assert.That(DefaultComponent.ContainedSlots.Count(x => x.SameIdentityAs(firstSlot)) == 2);
		}
	}

	public class PCComponentComponentTypeTests : PCComponentTests
	{
		[Test]
		public void WithType_ValidComponentType_ShouldSetTypeProperty()
		{
			//Arrange
			const ComponentType newType = ComponentType.Motherboard;
			Assert.That(DefaultComponent.Type, Is.Not.EqualTo(newType));
			DefaultComponent.WithType(newType);

			//Assert
			Assert.That(DefaultComponent.Type, Is.EqualTo(newType));
		}

		[Test]
		public void WithType_InvalidComponentType_ShouldThrowArgumentException()
		{
			//Arrange
			const ComponentType newType = (ComponentType) 2345678;

			//Assert
			Assert.That(() => DefaultComponent.WithType(newType), Throws.ArgumentException);
		}
	}
}