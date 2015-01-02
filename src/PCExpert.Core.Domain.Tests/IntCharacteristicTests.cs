using NUnit.Framework;

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
			var characteristic = new IntCharacteristic("some name", ComponentType.HardDiskDrive);
			var value = characteristic.CreateValue(valueArg);

			//Assert
			Assert.That(value, Is.Not.Null);
			Assert.That(value.Value, Is.EqualTo(valueArg));
		}
	}
}