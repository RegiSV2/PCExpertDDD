using System.Runtime.InteropServices;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class BoolCharacteristicTests
	{
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void CreateValue_AnyArg_ShouldReturnNewCharacteristicValueWithSpecifiedValue(bool valueArg)
		{
			//Arrange
			var characteristic = new BoolCharacteristic("some name", ComponentType.HardDiskDrive);
			var value = characteristic.CreateValue(valueArg);

			//Assert
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Value, Is.EqualTo(valueArg));
		}
	}
}