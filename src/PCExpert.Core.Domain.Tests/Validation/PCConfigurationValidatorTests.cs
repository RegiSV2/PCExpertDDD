using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Domain.Validation;
using PCExpert.DomainFramework.Specifications;
using PCExpert.DomainFramework.Validation;

namespace PCExpert.Core.Domain.Tests.Validation
{
	[TestFixture]
	public class PCConfigurationValidatorTests
	{
		private Mock<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>> _interpreterMock;
		private Mock<PublishedPCConfigurationSpecification> _publishedSpecificationMock;
		private PCConfigurationValidator _validator;

		[SetUp]
		public void EstablishContext()
		{
			_publishedSpecificationMock = new Mock<PublishedPCConfigurationSpecification>();
			_interpreterMock = new Mock<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>>();
			_validator = new PCConfigurationValidator(_publishedSpecificationMock.Object, _interpreterMock.Object);
		}

		[Test]
		[TestCase(2, 1)]
		[TestCase(255, 1)]
		[TestCase(10, 0)]
		public void NameLengthValidationTests(int nameLength, int expectedErrorsCount)
		{
			//Arrange
			var configuration = new PCConfiguration().WithName("".PadLeft(nameLength, '*'));

			AssertErrorsCount(configuration, expectedErrorsCount);
		}

		[Test]
		public void Status_Undefined_ShouldFail()
		{
			//Arrange
			var configuration = new PCConfiguration();
			configuration.MoveToStatus(PCConfigurationStatus.Undefined);

			AssertErrorsCount(configuration, 1);
		}

		[Test]
		public void InPersonalState_NoAdditionalValidationShouldBeExecuted()
		{
			//Arrange
			var configuration = new PCConfiguration();

			AssertErrorsCount(configuration, 0);
		}

		[Test]
		public void InPublishedState_ShouldValidateWithPublishedPCConfigurationSpecification()
		{
			//Arrange
			_publishedSpecificationMock.As<IDetailedSpecification<PCConfiguration, IPublishedPCConfigurationCheckDetails>>()
				.Setup(x => x.IsSatisfiedBy(It.IsAny<PCConfiguration>()))
				.Returns(CreateResultWithOneInvalidDetail()).Verifiable();
			_interpreterMock.Setup(x => x.Interpret(It.IsAny<IPublishedPCConfigurationCheckDetails>()))
				.Returns(new[] {new ValidationFailure("some prop", "some error")}).Verifiable();

			var configuration = new PCConfiguration();
			configuration.MoveToStatus(PCConfigurationStatus.Published);

			AssertErrorsCount(configuration, 1);
			_publishedSpecificationMock.As<IDetailedSpecification<PCConfiguration, IPublishedPCConfigurationCheckDetails>>()
				.Verify(x => x.IsSatisfiedBy(configuration), Times.Once());
			_interpreterMock.Verify(
				x => x.Interpret(It.IsAny<IPublishedPCConfigurationCheckDetails>()),
				Times.Once());
		}

		private SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails> CreateResultWithOneInvalidDetail()
		{
			var detailsMock = new Mock<IPublishedPCConfigurationCheckDetails>();
			detailsMock.Setup(x => x.NameNotEmptyFailure).Returns(true);
			return new SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>(
				false, detailsMock.Object);
		}

		private void AssertErrorsCount(PCConfiguration instance, int expectedErrorsCount)
		{
			Assert.That(_validator.Validate(instance).Errors.Count == expectedErrorsCount);
		}
	}
}