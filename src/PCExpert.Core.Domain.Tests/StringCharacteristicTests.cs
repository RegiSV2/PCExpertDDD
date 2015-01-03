using System;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class StringCharacteristicTests
	{
		[Test]
		[TestCase("")]
		[TestCase("abc123:\"{")]
		[TestCase("   ")]
		public void CreateValue_AnyArg_ShouldReturnNewCharacteristicValueWithSpecifiedValue(string valueArg)
		{
			//Arrange
			var characteristic = new StringCharacteristic("some name", ComponentType.HardDiskDrive).WithId(Guid.NewGuid());
			var value = characteristic.CreateValue(valueArg);

			//Assert
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Value, Is.EqualTo(valueArg));
			Assert.That(value.Characteristic.SameIdentityAs(characteristic));
			Assert.That(value.CharacteristicId, Is.EqualTo(characteristic.Id));
		}
	}
}