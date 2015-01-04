using FluentValidation;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests.Validation
{
	public class ValidatorTests<TValidator>
		where TValidator : IValidator, new()
	{
		protected readonly TValidator Validator = new TValidator();

		protected void AssertErrorsCount(object instance, int expectedErrorsCount)
		{
			Assert.That(Validator.Validate(instance).Errors.Count == expectedErrorsCount);
		}
	}
}