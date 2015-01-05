using NUnit.Framework;
using PCExpert.Core.Domain.Validation;

namespace PCExpert.Core.Domain.Tests.Validation
{
	[TestFixture]
	public class PCComponentValidatorTests
		: ValidatorTests<PCComponentValidator>
	{
		private const string ValidName = "some name";

		[Test]
		[TestCase(4, 1)]
		[TestCase(255, 1)]
		[TestCase(200, 0)]
		public void NameLengthValidationTests(int nameLength, int expectedErrorsCount)
		{
			var component = new PCComponent("".PadLeft(nameLength, '*'), ComponentType.PowerSupply);
			AssertErrorsCount(component, expectedErrorsCount);
		}

		[Test]
		[TestCase(999999999, 1)]
		[TestCase(300, 0)]
		public void AveragePriceValidationTests(decimal price, int expectedErrorsCount)
		{
			//Arrange
			var component = ComponentWithValidDefaults();
			component.WithAveragePrice(price);

			AssertErrorsCount(component, expectedErrorsCount);
		}

		private PCComponent ComponentWithValidDefaults()
		{
			return new PCComponent(ValidName, ComponentType.PowerSupply);
		}
	}
}