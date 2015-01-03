using System;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class IntCharacteristicTests
	{
		[Test]
		[TestCase(0)]
		[TestCase(150)]
		public void CreateValue_AnyArg_ShouldReturnNewCharacteristicValueWithSpecifiedValue(int valueArg)
		{
			//Arrange
			var characteristic = new IntCharacteristic("some name", ComponentType.HardDiskDrive).WithId(Guid.NewGuid());
			var value = characteristic.CreateValue(valueArg);

			//Assert
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Value, Is.EqualTo(valueArg));
			Assert.That(value.Characteristic.SameIdentityAs(characteristic));
			Assert.That(value.CharacteristicId, Is.EqualTo(characteristic.Id));
		}
	}
}