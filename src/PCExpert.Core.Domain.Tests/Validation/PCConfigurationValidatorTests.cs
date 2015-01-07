using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Domain.Validation;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Tests.Validation
{
	[TestFixture]
	public class PCConfigurationValidatorTests
	{
		private Mock<PublishedPCConfigurationDetailedSpecification> _publishedSpecificationMock;
		private PCConfigurationValidator _validator;

		[SetUp]
		public void EstablishContext()
		{
			_publishedSpecificationMock = new Mock<PublishedPCConfigurationDetailedSpecification>(
				new Mock<IPCConfigurationRepository>().Object);
			_validator = new PCConfigurationValidator(_publishedSpecificationMock.Object);
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
			_publishedSpecificationMock.As<IDetailedSpecification<PCConfiguration, PublishedPCConfigurationCheckDetails>>()
				.Setup(x => x.IsSatisfiedBy(It.IsAny<PCConfiguration>()))
				.Returns(CreateResultWithOneInvalidDetail()).Verifiable();

			var configuration = new PCConfiguration();
			configuration.MoveToStatus(PCConfigurationStatus.Published);

			AssertErrorsCount(configuration, 1);
			_publishedSpecificationMock.Verify(x => x.IsSatisfiedBy(configuration), Times.Once());
		}

		private SpecificationDetailedCheckResult<PublishedPCConfigurationCheckDetails> CreateResultWithOneInvalidDetail()
		{
			return new SpecificationDetailedCheckResult<PublishedPCConfigurationCheckDetails>(
				false, new PublishedPCConfigurationCheckDetails(true, false, false,
					new List<ComponentType>(), new List<ComponentType>(),
					new List<InterfaceDeficitInfo>(), new List<PCComponent>()));
		}

		private void AssertErrorsCount(PCConfiguration instance, int expectedErrorsCount)
		{
			Assert.That(_validator.Validate(instance).Errors.Count == expectedErrorsCount);
		}
	}
}