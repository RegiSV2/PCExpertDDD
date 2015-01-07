using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Validation;

namespace PCExpert.Core.DomainFramework.Tests.Validation
{
	[TestFixture]
	public class SpecificationValidationRuleBaseTests : ValidationRuleTests
	{
		private MockSpecificationValidationRuleBase _testedRule;

		[SetUp]
		public override void EstablishContext()
		{
			base.EstablishContext();
			_testedRule = new MockSpecificationValidationRuleBase();
		}

		[Test]
		public void ApplyCondition_NullCondition_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _testedRule.ApplyCondition(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Validate_AppliedConditionIsSatisfied_ShouldPerformValidation()
		{
			//Arrange
			_testedRule.ApplyCondition(x => true);

			Assert.That(_testedRule.Validate(ValidationContext).Count() == 1);
			Assert.That(_testedRule.IsValidationPerformed);
		}

		[Test]
		public void Validate_AppliedConditionNotSatisfied_ShouldNotValidate()
		{
			//Arrange
			_testedRule.ApplyCondition(x => false);

			Assert.That(!_testedRule.IsValidationPerformed);
		}

		private class MockSpecificationValidationRuleBase : SpecificationValidationRuleBase<string>
		{
			public bool IsValidationPerformed { get; private set; }

			protected override IEnumerable<ValidationFailure> DoValidate(string instanceToValidate)
			{
				IsValidationPerformed = true;
				return new[] {new ValidationFailure("some prop", "some error")};
			}
		}
	}
}