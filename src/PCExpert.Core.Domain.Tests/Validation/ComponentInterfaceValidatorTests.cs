using NUnit.Framework;
using PCExpert.Core.Domain.Validation;

namespace PCExpert.Core.Domain.Tests.Validation
{
	[TestFixture]
	public class ComponentInterfaceValidatorTests :
		ValidatorTests<ComponentInterfaceValidator>
	{
		[Test]
		[TestCase(2, 1)]
		[TestCase(255, 1)]
		[TestCase(55, 0)]
		public void NameValidationTests(int nameLength, int expectedErrorsCount)
		{
			var entity = new ComponentInterface("".PadLeft(nameLength, '*'));
			AssertErrorsCount(entity, expectedErrorsCount);
		}
	}
}