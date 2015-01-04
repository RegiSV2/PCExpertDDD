using System;
using System.Data.Entity.Validation;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using PCExpert.Core.DataAccess.Tests.Utils;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class PCExpertContextTests
	{
		private PCExpertContext _context;

		private Mock<IValidatorFactory> _validatorFactory;

		[SetUp]
		public void EstablishContext()
		{
			_validatorFactory = new Mock<IValidatorFactory>();
			_context = TestContextCreator.Create(_validatorFactory.Object);
		}

		[Test]
		public void Save_ValidatorForEntityNotSpecified_ShouldProceedDirectlyToDbOperation()
		{
			//Arrange
			var characteristic = new NumericCharacteristic("some name", ComponentType.PowerSupply);

			//Act
			_context.Characteristics.Add(characteristic);
			_context.SaveChanges();

			Assert.That(_context.Characteristics.ToList().Count == 1);
		}

		[Test]
		public void Save_ValidatorIsSpecified_ShouldThrowExceptionIfEntityIsNotValid()
		{
			//Arrange
			var entity = new ComponentInterface("some name");

			const string propName = "some prop";
			const string errorMessage = "some error message";
			var validator = new Mock<IValidator>();
			validator.Setup(x => x.Validate(entity))
				.Returns(new ValidationResult(new[] { new ValidationFailure(propName, errorMessage) }));
			_validatorFactory.Setup(x => x.GetValidator(typeof (ComponentInterface)))
				.Returns(validator.Object);

			try
			{
				_context.ComponentInterfaces.Add(entity);
				_context.SaveChanges();
				Assert.Fail();
			}
			catch (DbEntityValidationException ex)
			{
				Assert.That(ex.EntityValidationErrors.Count() == 1);
				Assert.That(ex.EntityValidationErrors.First().IsValid == false);
				Assert.That(ex.EntityValidationErrors.First().ValidationErrors.Count == 1);
				Assert.That(ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage == errorMessage);
				Assert.That(ex.EntityValidationErrors.First().ValidationErrors.First().PropertyName == propName);
			}
		}
	}
}