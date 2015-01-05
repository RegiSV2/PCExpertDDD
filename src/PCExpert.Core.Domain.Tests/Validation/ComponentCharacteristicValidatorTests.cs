using NUnit.Framework;
using PCExpert.Core.Domain.Validation;

namespace PCExpert.Core.Domain.Tests.Validation
{
	public class TestCharacteristic : ComponentCharacteristic
	{
		public TestCharacteristic(string name, ComponentType type)
			: base(name, type)
		{
		}
	}

	[TestFixture]
	public class ComponentCharacteristicValidatorTests
		: ValidatorTests<ComponentCharacteristicValidator<TestCharacteristic>>
	{
		[Test]
		[TestCase(256, 1)]
		[TestCase(2, 1)]
		[TestCase(40, 0)]
		public void NameLength_Tests(int nameLength, int expectedErrorCount)
		{
			//Arrange
			var characteristic = new TestCharacteristic("".PadLeft(nameLength, '*'), ComponentType.PowerSupply);

			AssertErrorsCount(characteristic, expectedErrorCount);
		}
	}
}