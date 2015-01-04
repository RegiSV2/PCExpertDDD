using System.Globalization;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Validation;

namespace PCExpert.Core.Domain.Tests.Validation
{
	public class TestCharacteristicValue : CharacteristicValue
	{
		protected override string DoFormat(CultureInfo cultureInfo)
		{
			return "";
		}
	}

	[TestFixture]
	public class CharacteristicValueTests
		: ValidatorTests<CharacteristicValueValidator<TestCharacteristicValue>>
	{
		[Test]
		public void NotAttachedToComponent_ShouldFail()
		{
			//Arrange
			var characteristic = new TestCharacteristicValue();

			AssertErrorsCount(characteristic, 1);
		}

		[Test]
		public void AttachedToComponent_ShouldPass()
		{
			//Arrange
			var characteristic = new TestCharacteristicValue();
			var component = new Mock<PCComponent>().Object;
			characteristic.AttachToComponent(component);

			AssertErrorsCount(characteristic, 0);
		}
	}
}