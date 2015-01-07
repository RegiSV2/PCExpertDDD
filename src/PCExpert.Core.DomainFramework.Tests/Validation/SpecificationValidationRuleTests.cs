using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Validation;
using TestType = System.String;

namespace PCExpert.Core.DomainFramework.Tests.Validation
{
	[TestFixture]
	public class SpecificationValidationRuleTests : ValidationRuleTests
	{
		private const string ValidationErrorMessage = "some message";
		private Mock<Specification<TestType>> _specificationMock;
		private SpecificationValidationRule<TestType> _testedRule;

		[SetUp]
		public override void EstablishContext()
		{
			base.EstablishContext();
			_specificationMock = new Mock<Specification<TestType>>();
			_testedRule = new SpecificationValidationRule<TestType>(_specificationMock.Object, ValidationErrorMessage);
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

			Assert.That(!_testedRule.Validate(ValidationContext).Any());
		}

		[Test]
		public void Validate_SpecificationIsNotSatisfied_ShouldReturnOneErrorWithSpecifiedMessage()
		{
			//Arrange
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>())).Returns(false);

			//Act
			var validationErrors = _testedRule.Validate(ValidationContext).ToList();

			Assert.That(validationErrors.Count == 1);
			Assert.That(validationErrors.First().ErrorMessage == ValidationErrorMessage);
		}
	}
}