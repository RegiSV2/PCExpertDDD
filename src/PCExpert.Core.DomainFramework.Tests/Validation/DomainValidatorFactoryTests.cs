using System;
using FluentValidation;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.DomainFramework.Validation;
using TestType = System.String;

namespace PCExpert.Core.DomainFramework.Tests.Validation
{
	[TestFixture]
	public class DomainValidatorFactoryTests
	{
		private DomainValidatorFactory _factory;

		[SetUp]
		public void EstablishContext()
		{
			_factory = new DomainValidatorFactory();
		}

		[Test]
		public void AddValidator_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _factory.AddValidator<TestType>(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AddValidator_ValidatorForTypeNotAdded_ShouldRegisterValidator()
		{
			Assert.That(() => _factory.AddValidator(new Mock<IValidator<TestType>>().Object),
				Throws.Nothing);
		}

		[Test]
		public void AddValidator_ValidatorForTypeAlreadyAdded_ShouldThrowDuplicateElementException()
		{
			//Arrange
			_factory.AddValidator(new Mock<IValidator<TestType>>().Object);

			Assert.That(() => _factory.AddValidator(new Mock<IValidator<TestType>>().Object),
				Throws.InstanceOf<DuplicateElementException>());
		}

		[Test]
		public void GetValidator_ValidatorForRequestedTypeNotRegistered_ShouldReturnNull()
		{
			Assert.That(_factory.GetValidator(typeof(TestType)), Is.Null);
		}

		[Test]
		public void GetValidator_ValidatorForRequestedTypeReqgistered_ShouldReturnRegisteredValidator()
		{
			//Arrange
			var validator = new Mock<IValidator<TestType>>().Object;
			_factory.AddValidator(validator);

			Assert.That(_factory.GetValidator(typeof (TestType)), Is.SameAs(validator));
		}
	}
}