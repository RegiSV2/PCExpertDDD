using System;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Validation;
using TestType = System.String;

namespace PCExpert.Core.DomainFramework.Tests.Validation
{
	[TestFixture]
	public class DetailedSpecificationValidationRuleTests : ValidationRuleTests
	{
		private Mock<ISpecificationDetailsInterpreter<TestTypeDetails>> _interpreterMock;
		private Mock<IDetailedSpecification<TestType, TestTypeDetails>> _specificationMock;

		private TestTypeDetails DetailsWithError
		{
			get { return new TestTypeDetails {Error1 = true}; }
		}

		[SetUp]
		public override void EstablishContext()
		{
			base.EstablishContext();
			_interpreterMock = new Mock<ISpecificationDetailsInterpreter<TestTypeDetails>>();
			_specificationMock = new Mock<IDetailedSpecification<TestType, TestTypeDetails>>();
		}

		[Test]
		public void Constructor_NullSpecification_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new DetailedSpecificationValidationRule<TestType, TestTypeDetails>(
				null, _interpreterMock.Object),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Constructor_NullInterpreter_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new DetailedSpecificationValidationRule<TestType, TestTypeDetails>(
				_specificationMock.Object, null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Validate_SpecificationIsSatisfied_ShouldReturnEmptyEnumerable()
		{
			//Arrange
			SetupSpecificationIsSatisfied();
			var rule = CreateRuleWithMocks();

			//Act
			var validationResult = rule.Validate(ValidationContext);

			//Assert
			Assert.That(validationResult, Is.Empty);
			_specificationMock.Verify(x => x.IsSatisfiedBy(It.IsAny<TestType>()), Times.Once());
		}

		[Test]
		public void Validate_SpecificationIsNotSatisfied_ShouldReturnErrorsProducedByInterpreter()
		{
			//Arrange
			var interpreterResult = new[] {new ValidationFailure("some prop", "some error")};
			_interpreterMock.Setup(x => x.Interpret(It.IsAny<TestTypeDetails>()))
				.Returns(interpreterResult).Verifiable();
			SetupSpecificationNotSatisfied();
			var rule = CreateRuleWithMocks();

			//Act
			var validationResult = rule.Validate(ValidationContext);

			//Assert
			Assert.That(validationResult == interpreterResult);
			_interpreterMock.Verify(x => x.Interpret(It.IsAny<TestTypeDetails>()), Times.Once());
		}

		private DetailedSpecificationValidationRule<TestType, TestTypeDetails> CreateRuleWithMocks()
		{
			return new DetailedSpecificationValidationRule<TestType, TestTypeDetails>(
				_specificationMock.Object, _interpreterMock.Object);
		}

		private void SetupSpecificationIsSatisfied()
		{
			SetupSpecificationMockWithResult(true);
		}

		private void SetupSpecificationNotSatisfied()
		{
			SetupSpecificationMockWithResult(false);
		}

		private void SetupSpecificationMockWithResult(bool isSatisfied)
		{
			_specificationMock.Setup(x => x.IsSatisfiedBy(It.IsAny<TestType>()))
				.Returns(new SpecificationDetailedCheckResult<TestTypeDetails>(isSatisfied, DetailsWithError))
				.Verifiable();
		}

		public class TestTypeDetails
		{
			public bool Error1 { get; set; }
		}
	}
}