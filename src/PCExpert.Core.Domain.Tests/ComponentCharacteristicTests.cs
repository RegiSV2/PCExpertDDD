using System;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class ComponentCharacteristicTests
	{
		[Test]
		public void Constructor_ValidArguments_ShouldCreateNewCharacteristic()
		{
			//Act
			const ComponentType type = ComponentType.SolidStateDrice;
			var name = NamesGenerator.CharacteristicName();
			var characteristic = new ComponentCharacteristic(
				name, type);

			//Assert
			Assert.That(characteristic.ComponentType, Is.EqualTo(type));
			Assert.That(characteristic.Name, Is.EqualTo(name));
		}

		[Test]
		public void Constructor_InvalidType_ShouldThrowArgumentException()
		{
			Assert.That(() => new ComponentCharacteristic(
				NamesGenerator.CharacteristicName(),
				(ComponentType) 12345),
				Throws.ArgumentException);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Constructor_EmptyName_ShouldThrowArgumentNullException(string name)
		{
			Assert.That(() => new ComponentCharacteristic(
				name, ComponentType.Motherboard),
				Throws.InstanceOf<ArgumentNullException>());
		}
	}
}