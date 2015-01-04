using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Validation;
using TestType = System.String;

namespace PCExpert.Core.DomainFramework.Tests.Validation
{
	[TestFixture]
	public class SpecificationValidationRuleTests
	{
		private const string ValidationErrorMessage = "some message";
		private Mock<Specification<TestType>> _specificationMock;
		private SpecificationValidationRule<TestType> _testedRule;
		private ValidationContext _validationContext;
		private readonly string _validatedInstance = "some string";

		[SetUp]
		public void EstablishContext()
		{
			_specificationMock = new Mock<Specification<TestType>>();
			_testedRule = new SpecificationValidationRule<string>(_specificationMock.Object, ValidationErrorMessage);
			_validationContext = new ValidationContext(_validatedInstance, new PropertyChain(Enumerable.Empty<TestType>()),
				new Mock<IValidatorSelector>().Object);
		}

		[Test]
		public void Constructor_NullSpecification_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new SpecificationValidationRule<TestType>(null, ValidationErrorMessage),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Constructor_NullErrorMessage_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new SpecificationValidationRule<TestType>(new Mock<Specification<TestType>>().Object, null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Validate_SpecificationIsSatisfied_ShouldReturnEmptyEnumerable()
		{
			//Arrange
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>())).Returns(true);

			Assert.That(!_testedRule.Validate(_validationContext).Any());
		}

		[Test]
		public void Validate_SpecificationIsNotSatisfied_ShouldReturnOneErrorWithSpecifiedMessage()
		{
			//Arrange
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>())).Returns(false);

			//Act
			var validationErrors = _testedRule.Validate(_validationContext).ToList();

			Assert.That(validationErrors.Count == 1);
			Assert.That(validationErrors.First().ErrorMessage == ValidationErrorMessage);
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
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>())).Returns(false);

			Assert.That(_testedRule.Validate(_validationContext).Count() == 1);
		}

		[Test]
		public void Validate_AppliedConditionNotSatisfied_ShouldNotValidate()
		{
			//Arrange
			_testedRule.ApplyCondition(x => false);
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>())).Returns(false);

			Assert.That(!_testedRule.Validate(_validationContext).Any());
		}
	}
}