using System;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain.Exceptions;
using PCExpert.Core.Domain.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class PCComponentTests
	{
		private const decimal ComponentPrice = 100m;

		[Test]
		public void Constructor_Called_CanCreatePCComponent()
		{
			var componentName = NamesGenerator.ComponentName();
			var component = new PCComponent(componentName, ComponentPrice);

			Assert.That(component.Name, Is.EqualTo(componentName));
			Assert.That(component.AveragePrice, Is.EqualTo(ComponentPrice));
		}

		[Test]
		public void ContainedComponents_NoComponentsAdded_ShouldReturnEmptyCollection()
		{
			//Arrange
			var parentComponent = CreateComponent(1);

			//Assert
			Assert.That(parentComponent.ContainedComponents, Is.Empty);
		}

		[Test]
		public void AddContainedComponent_NotNullComponent_ComponentShouldBeAddedToContainedComponents()
		{
			//Arrange
			var parentComponent = CreateComponent(1);
			var childComponent = CreateComponent(2);

			//Act
			parentComponent.WithContainsComponent(childComponent);

			//Assert
			Assert.That(parentComponent.ContainedComponents.Count, Is.EqualTo(1));
			Assert.That(parentComponent.ContainedComponents.Contains(childComponent));
		}

		[Test]
		public void AddContainedComponent_NullComponent_ShouldThrowArgumentNullException()
		{
			//Arrange
			var parentComponent = CreateComponent(1);

			//Assert
			Assert.That(() => parentComponent.WithContainsComponent(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AddContainedComponent_ComponentAlreadyAdded_ShouldThrowDuplicateElementException()
		{
			//Arrange
			var parentComponent = CreateComponent(1);
			var childComponent = CreateComponent(2);
			parentComponent.WithContainsComponent(childComponent);

			//Assert
			Assert.That(() => parentComponent.WithContainsComponent(childComponent),
				Throws.InstanceOf<DuplicateElementException>());
		}

		private static PCComponent CreateComponent(int componentNameValue)
		{
			return new PCComponent(NamesGenerator.ComponentName(componentNameValue), ComponentPrice);
		}
	}
}