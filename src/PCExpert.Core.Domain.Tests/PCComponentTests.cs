﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.Exceptions;

namespace PCExpert.Core.Domain.Tests
{
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

	[TestFixture]
	public class PCComponentConstructorTests : PCComponentTests
	{
		[Test]
		public void Constructor_Called_CanCreatePCComponent()
		{
			//Arrange
			var componentName = NamesGenerator.ComponentName();
			var component = new PCComponent(componentName, ValidComponentType);

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

	[TestFixture]
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

			Assert.That(DefaultComponent.AveragePrice, Is.EqualTo(ComponentPrice));
		}

		[Test]
		public void SetAveragePrice_NegativeValue_ShouldThrowArgumentOutOfRangeException()
		{
			Assert.That(() => DefaultComponent.WithAveragePrice(-15m), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}

	[TestFixture]
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

	[TestFixture]
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

			Assert.That(() => DefaultComponent.WithContainedComponent(secondComponent),
				Throws.InstanceOf<DuplicateElementException>());
		}

		[Test]
		public void EnumerateContainedSlots_AnyConfiguration_ShouldEnumerateAllSlotsFromComponentAndContainedComponents()
		{
			//Arrange
			const int interfacesCount = 5;
			var interfaces = new ComponentInterface[interfacesCount];
			for (var i = 0; i < interfacesCount; i++)
				interfaces[i] = CreateInterface(i).Object;
			var parentComponent = CreateComponent(1)
				.WithContainedSlot(interfaces[0])
				.WithContainedSlot(interfaces[1])
				.WithContainedComponent(
					CreateComponent(2)
						.WithContainedSlot(interfaces[1])
						.WithContainedSlot(interfaces[2])
						.WithContainedComponent(
							CreateComponent(3).WithContainedSlot(interfaces[4]))
						.WithContainedComponent(
							CreateComponent(4).WithContainedSlot(interfaces[4])));

			//Act
			var actualInterfaceCounts = parentComponent.EnumerateContainedSlots()
				.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

			//Assert
			var assertedInterfaceCounts = new Dictionary<ComponentInterface, int>
			{
				{interfaces[0], 1},
				{interfaces[1], 2},
				{interfaces[2], 1},
				{interfaces[4], 2}
			};
			Assert.That(!actualInterfaceCounts.ContainsKey(interfaces[3]));
			foreach (var assertedCount in assertedInterfaceCounts)
				Assert.That(actualInterfaceCounts[assertedCount.Key], Is.EqualTo(assertedCount.Value));
		}
	}

	[TestFixture]
	public class PCComponentInterfaceConnectionsTests : PCComponentTests
	{
		[Test]
		public void PlugSlots_NoPlugSlotsSet_ShouldReturnEmptyCollection()
		{
			Assert.That(DefaultComponent.PlugSlots.Count, Is.EqualTo(0));
		}

		[Test]
		public void WithPlugSlot_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => DefaultComponent.WithPlugSlot(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithPlugSlot_NotNullArgument_ShouldAddNewSlot()
		{
			//Arrange
			var componentInterface = CreateInterface(1).Object;

			//Act
			DefaultComponent.WithPlugSlot(componentInterface);

			//Assert
			Assert.That(DefaultComponent.PlugSlots.Contains(componentInterface));
		}

		[Test]
		public void WithPlugSlot_SlotAlreadyAdded_ShouldAddDuplicateSlot()
		{
			//Arrange
			var componentInterface = CreateInterface(1).Object;
			DefaultComponent.WithPlugSlot(componentInterface);

			//Act
			DefaultComponent.WithPlugSlot(componentInterface);

			//Assert
			Assert.That(DefaultComponent.PlugSlots.Count(x => x.SameIdentityAs(componentInterface)) == 2);
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

	[TestFixture]
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

			Assert.That(() => DefaultComponent.WithType(newType), Throws.ArgumentException);
		}
	}

	[TestFixture]
	public class PCComponentCharacteristicsTests : PCComponentTests
	{
		[Test]
		public void CharacteristicValues_AfterCreation_ShouldBeEmpty()
		{
			Assert.That(DefaultComponent.CharacteristicValues.Count == 0);
		}

		[Test]
		public void Characteristics_AfterCreation_ShouldBeEmpty()
		{
			Assert.That(DefaultComponent.Characteristics.Count == 0);
		}

		[Test]
		public void WithCharacteristicValue_NullCharacteristic_ShouldThrowArgumentNullException()
		{
			Assert.That(() => DefaultComponent.WithCharacteristicValue(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithCharacteristicValue_ValueForThisCharacteristicNotSpecifiedYet_ShouldAddValueToCharacteristicValues()
		{
			//Arrange
			var characteristicValue = CreateCharacteristicValue();
			DefaultComponent.WithCharacteristicValue(characteristicValue);

			Assert.That(DefaultComponent.CharacteristicValues.Contains(characteristicValue));
		}

		[Test]
		public void WithCharacteristicValue_ValueForCharacteristicNotSpecifiedYet_ShouldAddValueToCharacteristics()
		{
			//Arrange
			var characteristicValue = CreateCharacteristicValue();
			DefaultComponent.WithCharacteristicValue(characteristicValue);

			Assert.That(DefaultComponent.Characteristics.Values.Contains(characteristicValue));
			Assert.That(DefaultComponent.Characteristics.ContainsKey(characteristicValue.Characteristic));
		}

		[Test]
		public void WithCharacteristicValue_ValueForCharacteristicNotSpecifiedYet_ShouldAttachCharacteristicToComponent()
		{
			//Arrange
			var characteristicValue = CreateCharacteristicValue();
			DefaultComponent.WithCharacteristicValue(characteristicValue);

			Assert.That(characteristicValue.Component.SameIdentityAs(DefaultComponent));
		}

		[Test]
		public void Characteristics_CharacteristicValuesArePredefined_ShouldContainAllCharacteristics()
		{
			//Arrange
			var valuesList = new List<CharacteristicValue>();
			for (var i = 0; i < 5; i++)
				valuesList.Add(CreateCharacteristicValue());

			DefaultComponent.GetType().GetProperty("CharacteristicVals",
				BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.SetProperty)
				.SetValue(DefaultComponent, valuesList);

			//Assert
			Assert.That(DefaultComponent.Characteristics.Count == valuesList.Count);
			foreach (var value in valuesList)
			{
				Assert.That(DefaultComponent.Characteristics.ContainsKey(value.Characteristic));
				Assert.That(DefaultComponent.Characteristics[value.Characteristic] == value);
			}
		}

		private static CharacteristicValue CreateCharacteristicValue()
		{
			var characteristic = new Mock<ComponentCharacteristic>().WithId(Guid.NewGuid());
			var characteristicValue = new Mock<CharacteristicValue>(characteristic.Object).Object;
			return characteristicValue;
		}
	}
}